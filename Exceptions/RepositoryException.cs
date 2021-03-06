using System;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;

namespace AzureContainer.Exceptions
{
    public class RepositoryException : Exception
    {
        public RepositoryException() { }

        public RepositoryException(string message) : base(message) { }

        public RepositoryException(string message, Exception innerException) : base(message, innerException) { }

        protected RepositoryException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}