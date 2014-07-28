using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.ProcessWithCustomContext
{
    public class CustomContextProcess : Process<CustomContextProcessRow, CustomContext>
    {

        protected override IProcessLogger CreateLog()
        {
            return new ConsoleLog();
        }

        protected override CustomContext CreateProcessContext(IProcessLogger log, IDictionary<string, dynamic> data)
        {
            return new CustomContext();
        }
    }

    public class CustomContextProcessRow
    {
    }
}
