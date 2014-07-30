using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.InternalOperations
{
    internal class YieldDetectionOperation<TOperationRow, TOperationContext> : IOperation<TOperationRow, TOperationContext>
        where TOperationRow : class
        where TOperationContext : IProcessContext
    {
        public bool HasExecuted { get; set; }

        public IEnumerable<TOperationRow> Execute(TOperationContext context, IEnumerable<TOperationRow> data)
        {
            foreach (var row in data)
            {
                HasExecuted = true;
                yield return row;
            }
        }
    }
}
