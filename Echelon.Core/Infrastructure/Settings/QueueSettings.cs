namespace Echelon.Core.Infrastructure.Settings
{
    public static class QueueSettings
    {
        public const string QueueEndpoint = "rabbitmq://localhost/";
        public const string General = "general_queue";
        public const string Registration = "registration_queue";
    }
}