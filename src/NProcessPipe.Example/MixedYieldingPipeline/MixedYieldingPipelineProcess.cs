using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.MixedYieldingPipeline
{
    public class MixedYieldingPipelineProcess : Process<MixedYieldingPipelineRow>
    {
        protected override IProcessLogger CreateLog()
        {
            return new ConsoleLog();
        }
    }

    public class MixedYieldingPipelineRow
    {
        public string Message { get; set; }
    }
}
