using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe
{
    public interface IProcessLogger
    {
        void Debug(string message);
        void Debug(string messsageFormat, params string[] args);

        void Trace(string message);
        void Trace(string messsageFormat, params string[] args);

        void Info(string message);
        void Info(string messsageFormat, params string[] args);

        void Warn(string message);
        void Warn(string messsageFormat, params string[] args);

        void Error(string message);
        void Error(string messsageFormat, params string[] args);
        void Error(Exception ex);
        void Error(Exception ex, string message);
        void Error(Exception ex, string messsageFormat, params string[] args);
    }
}
