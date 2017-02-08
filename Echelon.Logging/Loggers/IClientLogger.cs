namespace Echelon.Core.Logging.Loggers
{
    public interface IClientLogger
    {
        void Info(object message);
        void Debug(object message);
        void Error(object message);
        void Fatal(object message);
        void Warn(object message);
        void ErrorFormat(string format, params object[] args);
        void Dispose();
    }
}