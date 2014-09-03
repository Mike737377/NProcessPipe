using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.MixedYieldingPipeline
{
    public class Operation1 : IOperation<MixedYieldingPipelineRow>
    {
        public IEnumerable<MixedYieldingPipelineRow> Execute(DefaultProcessContext context, IEnumerable<MixedYieldingPipelineRow> data)
        {
            foreach (var row in data)
            {
                Console.WriteLine("Operation 1 running with message " + row.Message);
                yield return row;
            }
        }
    }

    public class Operation2 : IOperation<MixedYieldingPipelineRow>, IOperationDependsOn<Operation1>
    {
        public IEnumerable<MixedYieldingPipelineRow> Execute(DefaultProcessContext context, IEnumerable<MixedYieldingPipelineRow> data)
        {
            foreach (var row in data)
            {
                Console.WriteLine("Operation 2 running with message " + row.Message);
            }

            return data;
        }
    }

    public class Operation3 : IOperation<MixedYieldingPipelineRow>, IOperationDependsOn<Operation2>
    {
        public IEnumerable<MixedYieldingPipelineRow> Execute(DefaultProcessContext context, IEnumerable<MixedYieldingPipelineRow> data)
        {
            foreach (var row in data)
            {
                Console.WriteLine("Operation 3 running with message " + row.Message);
                yield return row;
            }
        }
    }
}
