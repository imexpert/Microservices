using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Exceptions
{
    public class UnauthorizeException : Exception
    {
        public UnauthorizeException()
        {
        }

        public UnauthorizeException(string message) : base(message)
        {
        }

        public UnauthorizeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnauthorizeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
