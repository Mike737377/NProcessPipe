using NProcessPipe.DependencyAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NProcessPipe
{
    public class OperationRegistry<TOperationRow, TOperationContext> : IEnumerable<IOperation<TOperationRow, TOperationContext>>
        where TOperationRow : class
        where TOperationContext : IProcessContext
    {

        private readonly List<IOperation<TOperationRow, TOperationContext>> _operationsList = new List<IOperation<TOperationRow, TOperationContext>>();
        private readonly string _diagraph;

        public OperationRegistry(IEnumerable<IOperation<TOperationRow, TOperationContext>> operations, string diagraph)
        {
            _operationsList.AddRange(operations);
            _diagraph = diagraph;
        }

        public IEnumerator<IOperation<TOperationRow, TOperationContext>> GetEnumerator()
        {
            return _operationsList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _operationsList.GetEnumerator();
        }

        public string CreateWorkflowGraph()
        {
            return _diagraph;
        }

    }
}
