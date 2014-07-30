using NProcessPipe.Example;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Tests.ProcessCases
{
    public abstract class TestProcess<T> : Process<T>
        where T : class
    {
        protected override IProcessLogger CreateLog()
        {
            return new ConsoleLog();
        }
    }
}
