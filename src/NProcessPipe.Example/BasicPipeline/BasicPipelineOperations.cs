using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.BasicPipeline
{
    public class Operation1 : IOperation<BasicPipelineRow>
    {
        public IEnumerable<BasicPipelineRow> Execute(DefaultProcessContext context, IEnumerable<BasicPipelineRow> data)
        {
            foreach (var row in data)
            {
                Console.WriteLine("Operation 1 running with message " + row.Message);
                yield return row;
            }
        }
    }

    public class Operation2 : IOperation<BasicPipelineRow>, IOperationDependsOn<Operation1>
    {
        public IEnumerable<BasicPipelineRow> Execute(DefaultProcessContext context, IEnumerable<BasicPipelineRow> data)
        {
            foreach (var row in data)
            {
                Console.WriteLine("Operation 2 running with message " + row.Message);
                yield return row;
            }
        }
    }

    public class Operation3 : IOperation<BasicPipelineRow>, IOperationDependsOn<Operation2>
    {
        public IEnumerable<BasicPipelineRow> Execute(DefaultProcessContext context, IEnumerable<BasicPipelineRow> data)
        {
            foreach (var row in data)
            {
                Console.WriteLine("Operation 3 running with message " + row.Message);
                yield return row;
            }
        }
    }
}
