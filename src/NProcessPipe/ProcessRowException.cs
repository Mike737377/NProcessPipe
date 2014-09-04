using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NProcessPipe
{
    [Serializable]
    public class ProcessRowException : Exception
    {

        public ProcessRowException()
            : base()
        { }

        public ProcessRowException(string message)
            : base(message)
        { }

        public ProcessRowException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public ProcessRowException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        public int Row { get; internal set; }
        public object RowData { get; internal set; }
    }
}
