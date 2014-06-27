using NProcessPipe.DependencyAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NProcessPipe
{
    public class OperationRegistry<TOperationRow> : IEnumerable<IOperation<TOperationRow>>
            where TOperationRow : class
    {

        private readonly List<IOperation<TOperationRow>> _operationsList = new List<IOperation<TOperationRow>>();
        private readonly string _diagraph;

        public OperationRegistry(IEnumerable<IOperation<TOperationRow>> operations, string diagraph)
        {
            _operationsList.AddRange(operations);
            _diagraph = diagraph;
        }

        public IEnumerator<IOperation<TOperationRow>> GetEnumerator()
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
