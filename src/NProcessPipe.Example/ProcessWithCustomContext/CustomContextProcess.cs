using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.ProcessWithCustomContext
{
    public class CustomContextProcess : Process<CustomContextProcessRow, CustomContext>
    {

        public CustomContextProcess()
        {
            _log = new ConsoleLog();
        }

        protected override CustomContext CreateProcessContext()
        {
            return new CustomContext();
        }
    }

    public class CustomContextProcessRow 
    {
    }
}
