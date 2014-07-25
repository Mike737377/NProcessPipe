using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.BasicPipeline
{
    public class BasicPipelineTests
    {

        public void Demo()
        {

            var rows = new List<BasicPipelineRow>();
            rows.Add(new BasicPipelineRow() { Message = "'This is the data'" });

            var process = new BasicPipelineProcess();
            process.Execute(rows);

        }

    }
}
