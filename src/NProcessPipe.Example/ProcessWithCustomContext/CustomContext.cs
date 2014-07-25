using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.ProcessWithCustomContext
{
    public class CustomContext : IProcessContext
    {
        public string Message { get; set; }
    }
}
