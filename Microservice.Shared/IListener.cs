using System;
using System.Threading.Tasks;

namespace Microservice.Shared
{
    public interface IListener
    {
        Task SendMessage(object obj);
        Task QueueMessage(object obj, string queueName);
        void RegisterListener<T>(Func<T, Task> callback, bool isQueue = false) where T : new();
        void RegisterListener<T>(string name, Func<T, Task> callback, bool isQueue = false) where T : new();
    }
}
