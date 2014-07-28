using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe
{
    public interface IProcessLogger
    {
        void Debug(string message);
        void Debug(string messageFormat, params object[] args);

        void Trace(string message);
        void Trace(string messageFormat, params object[] args);

        void Info(string message);
        void Info(string messageFormat, params object[] args);

        void Warn(string message);
        void Warn(string messageFormat, params object[] args);

        void Error(string message);
        void Error(string messageFormat, params object[] args);
        void Error(Exception ex);
        void Error(Exception ex, string message);
        void Error(Exception ex, string messageFormat, params object[] args);
    }
}
