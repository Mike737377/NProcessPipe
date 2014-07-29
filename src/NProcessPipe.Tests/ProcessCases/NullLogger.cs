using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Tests.ProcessCases
{
    public class NullLogger : IProcessLogger
    {
        public void Debug(string message)
        { }

        public void Debug(string messageFormat, params object[] args)
        { }

        public void Trace(string message)
        { }

        public void Trace(string messageFormat, params object[] args)
        { }

        public void Info(string message)
        { }

        public void Info(string messageFormat, params object[] args)
        { }

        public void Warn(string message)
        { }

        public void Warn(string messageFormat, params object[] args)
        { }

        public void Error(string message)
        { }

        public void Error(string messageFormat, params object[] args)
        { }

        public void Error(Exception ex)
        { }

        public void Error(Exception ex, string message)
        { }

        public void Error(Exception ex, string messageFormat, params object[] args)
        { }
    }
}
