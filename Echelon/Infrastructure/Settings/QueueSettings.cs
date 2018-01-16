namespace Echelon.Infrastructure.Settings
{
    public static class QueueSettings
    {
        public const string General = "rabbitmq://localhost/general_queue";
        public static string Registration = "rabbitmq://localhost/registration_queue";
    }
}