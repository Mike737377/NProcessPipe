using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example
{
    public class ConsoleLog : IProcessLogger
    {
        public void Debug(string message)
        {
            Console.WriteLine("Debug: " + message);
        }

        public void Debug(string messageFormat, params object[] args)
        {
            Console.WriteLine("Debug: " + string.Format( messageFormat,args));
        }

        public void Trace(string message)
        {
            Console.WriteLine("Trace: " + message);
        }

        public void Trace(string messageFormat, params object[] args)
        {
            Console.WriteLine("Trace: " + string.Format(messageFormat, args));
        }

        public void Info(string message)
        {
            Console.WriteLine("Info: " + message);
        }

        public void Info(string messageFormat, params object[] args)
        {
            Console.WriteLine("Info: " + string.Format(messageFormat, args));
        }

        public void Warn(string message)
        {
            Console.WriteLine("Warn: " + message);
        }

        public void Warn(string messageFormat, params object[] args)
        {
            Console.WriteLine("Warn: " + string.Format(messageFormat, args));
        }

        public void Error(string message)
        {
            Console.WriteLine("Error: " + message);
        }

        public void Error(string messageFormat, params object[] args)
        {
            Console.WriteLine("Error: " + string.Format(messageFormat, args));
        }

        public void Error(Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }

        public void Error(Exception ex, string message)
        {
            Console.WriteLine("Error: " + message);
            Console.WriteLine(ex.ToString());
        }

        public void Error(Exception ex, string messageFormat, params object[] args)
        {
            Console.WriteLine("Error: " + string.Format(messageFormat, args));
            Console.WriteLine(ex.ToString());
        }
    }
}
