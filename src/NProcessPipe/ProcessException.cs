using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NProcessPipe
{
    [Serializable]
    public class ProcessException : Exception
    {
        public ProcessException()
            : base()
        { }

        public ProcessException(string message)
            : base(message)
        { }

        public ProcessException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public ProcessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

    }
}
