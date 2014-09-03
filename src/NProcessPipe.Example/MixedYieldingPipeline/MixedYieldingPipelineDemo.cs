using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.MixedYieldingPipeline
{
    public class MixedYieldingPipelineDemo
    {
        public void Demo()
        {

            var rows = new List<MixedYieldingPipelineRow>();
            rows.Add(new MixedYieldingPipelineRow() { Message = "'First Row'" });
            rows.Add(new MixedYieldingPipelineRow() { Message = "'Second Row'" });

            var process = new MixedYieldingPipelineProcess();
            process.Execute(rows);

        }
    }
}
