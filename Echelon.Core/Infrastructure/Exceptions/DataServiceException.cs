using System;

namespace Echelon.Core.Infrastructure.Exceptions
{
    internal class DataServiceException : Exception
    {
        public Exception GeneratedException { get; }

        public string ErrorMessage { get; }

        public DataServiceException(Exception generatedException)
        {
            GeneratedException = generatedException;
            ErrorMessage = $"Dataservice threw and error with message: {generatedException.Message}";
        }
    }
}