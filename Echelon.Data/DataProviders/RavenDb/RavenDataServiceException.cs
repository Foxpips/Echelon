using System;

namespace Echelon.Data.DataProviders.RavenDb
{
    internal class RavenDataServiceException : Exception
    {
        public Exception GeneratedException { get; }

        public string ErrorMessage { get; }

        public RavenDataServiceException(Exception generatedException)
        {
            GeneratedException = generatedException;
            ErrorMessage = $"Dataservice threw and error with message: {generatedException.Message}";
        }
    }
}