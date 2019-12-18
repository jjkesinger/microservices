namespace Microservice.Shared
{
    public class AppSettings
    {
        public string ServiceBusEndpoint { get; set; }
        public string SubscriberName { get; set; }
        public string PayrollQueueName { get; set; }
    }
}
