using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Shared
{
    public class Listener : IListener
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _subcriptionName;
        private readonly IMemoryCache _cache;
        private readonly ILogger<Listener> _logger;
        private readonly RetryPolicy _retryPolicy;
        private readonly MessageHandlerOptions _options;

        private static readonly Dictionary<string, IClientEntity> Queues = new Dictionary<string, IClientEntity>();

        public Listener(IOptions<AppSettings> settings, IMemoryCache cache, ILogger<Listener> logger)
        {
            _serviceBusConnectionString = settings.Value.ServiceBusEndpoint;
            _subcriptionName = settings.Value.SubscriberName;
            _cache = cache;
            _logger = logger;
            _options = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            _retryPolicy = new RetryExponential(
                minimumBackoff: TimeSpan.FromSeconds(1),
                maximumBackoff: TimeSpan.FromSeconds(30),
                maximumRetryCount: 3);
        }

        /// <summary>
        /// Send message to a topic
        /// </summary>
        /// <param name="obj">Object to serialize and send</param>
        /// <returns>Task</returns>
        public async Task SendMessage(object obj)
        {
            var topicName = obj.GetType().Name;

            var queue = GetQueue(topicName, (c, n, t) => new TopicClient(c, n));

            await SendMessage(queue, obj);
        }

        /// <summary>
        /// Send message to a queue
        /// </summary>
        /// <param name="obj">Object to serialize and send</param>
        /// <param name="queueName">Name of queue</param>
        /// <returns>Task</returns>
        public async Task QueueMessage(object obj, string queueName)
        {
            var queue = GetQueue(queueName, (c, n, t) => new QueueClient(c, n));

            await SendMessage(queue, obj);
        }

        public void RegisterListener<T>(Func<T, Task> callback, bool isQueue = false) where T : new()
        {
            var name = typeof(T).Name.ToLower();

            RegisterListener<T>(name, callback, isQueue);
        }

        public void RegisterListener<T>(string name, Func<T, Task> callback, bool isQueue = false) where T : new()
        {
            var receiver = isQueue ? GetQueue<IReceiverClient>(name, (c, n, t) => new QueueClient(c, n, ReceiveMode.PeekLock, _retryPolicy)) :
                GetQueue<IReceiverClient>(name, (c, n, t) => new SubscriptionClient(c, n, t, ReceiveMode.PeekLock, _retryPolicy));

            var fullname = $"{receiver.GetType().Name.ToLower()}_{name}";

            receiver.RegisterMessageHandler(Handle(fullname, callback), _options);
        }


        private Func<Message, CancellationToken, Task> Handle<T>(string queuename, Func<T, Task> callback) where T : new()
        {
            return async (message, cancellationToken) =>
            {
                var serialized = Encoding.UTF8.GetString(message.Body);
                var obj = JsonConvert.DeserializeObject<T>(serialized);

                await callback(obj);

                Queues.TryGetValue(queuename, out var client);
                if (client == null)
                    throw new ArgumentException("Queue/Topic was not registered.", nameof(queuename));

                if (!(client is IReceiverClient queue))
                    throw new ArgumentException("Queue is not a valid receiver client.", nameof(queuename));

                await queue.CompleteAsync(message.SystemProperties.LockToken);
            };
            
        }

        private async Task SendMessage(ISenderClient queue, object obj)
        {
            var serialized = JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            var message = new Message(Encoding.UTF8.GetBytes(serialized));

            try
            {
                await queue.SendAsync(message);
            }
            catch (Exception)
            {
                var key = $"{queue.Path}_cache";

                var messages = _cache.Get<List<Message>>(key);
                messages ??= new List<Message>();

                messages.Add(message);

                _cache.Set(key, messages, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
                });
            }
        }

        private T GetQueue<T>(string name, Func<string, string, string, T> func) where T : class, IClientEntity
        {
            var queuename = $"{typeof(T).Name.ToLower()}_{name.ToLower()}";
            Queues.TryGetValue(queuename, out IClientEntity queue);

            if (queue == null)
            {
                queue = func(_serviceBusConnectionString, name, _subcriptionName);
                if (!Queues.ContainsKey(queuename))
                    Queues.Add(queuename, queue);
            }

            return queue as T;
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(exceptionReceivedEventArgs.Exception, exceptionReceivedEventArgs.Exception.Message);

            return Task.CompletedTask;
        }
    }
}
