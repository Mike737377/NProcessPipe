using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.BasicPipeline
{
    public class BasicPipelineDemo
    {

        public void Demo()
        {

            var rows = new List<BasicPipelineRow>();
            rows.Add(new BasicPipelineRow() { Message = "'First Row'" });
            rows.Add(new BasicPipelineRow() { Message = "'Second Row'" });

            var process = new BasicPipelineProcess();
            process.Execute(rows);

        }

    }
}
