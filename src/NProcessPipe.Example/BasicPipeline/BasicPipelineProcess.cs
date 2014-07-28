using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.BasicPipeline
{
    public class BasicPipelineProcess : Process<BasicPipelineRow>
    {
        protected override IProcessLogger CreateLog()
        {
            return new ConsoleLog();
        }
    }

    public class BasicPipelineRow
    {
        public string Message { get; set; }
    }
}
