using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.BasicPipeline
{
    public class BasicPipelineProcess : Process<BasicPipelineRow>
    {
        public BasicPipelineProcess()
        {
            _log = new ConsoleLog();
        }
    }

    public class BasicPipelineRow
    {
        public string Message { get; set; }
    }
}
