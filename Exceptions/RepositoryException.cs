using System;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;

namespace AzureContainer.Exceptions
{
    public class RepositoryException : Exception
    {
        private readonly ILogger<RepositoryException> _logger;

        public RepositoryException(ILogger<RepositoryException> logger)
        {
            _logger = logger;
        }

        public RepositoryException(string message) : base(message)
        {
            _logger.LogDebug(message);
        }

        public RepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}