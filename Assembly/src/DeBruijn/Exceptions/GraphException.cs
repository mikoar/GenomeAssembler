using System;
using System.Runtime.Serialization;

namespace Assembly.DeBruijn.Exceptions
{
    [Serializable]
    public class GraphException : Exception
    {
        public GraphException() { }
        public GraphException(string message) : base(message) { }
        public GraphException(string message, Exception inner) : base(message, inner) { }
        protected GraphException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}