using NProcessPipe.DependencyAnalysis;
using NProcessPipe.InternalOperations;
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
        private readonly DependencyGraph<Type> _dependencyGraph;
        private readonly YieldDetectionOperation<TOperationRow, TOperationContext> _yieldDetection = new YieldDetectionOperation<TOperationRow, TOperationContext>();
        private readonly RowProcessingBeginningOperation<TOperationRow, TOperationContext> _rowProcessingBeginning;
        private readonly RowProcessingEndingOperation<TOperationRow, TOperationContext> _rowProcessingEnding;

        public OperationRegistry(IProcessAccessor process, IEnumerable<IOperation<TOperationRow, TOperationContext>> operations, DependencyGraph<Type> dependencyGraph)
        {
            _rowProcessingBeginning = new RowProcessingBeginningOperation<TOperationRow, TOperationContext>(process);
            _rowProcessingEnding = new RowProcessingEndingOperation<TOperationRow, TOperationContext>(process);

            _operationsList.Add(_yieldDetection);
            _operationsList.Add(_rowProcessingBeginning);
            _operationsList.AddRange(operations);
            _operationsList.Add(_rowProcessingEnding);
            _dependencyGraph = dependencyGraph;
        }

        public IEnumerator<IOperation<TOperationRow, TOperationContext>> GetEnumerator()
        {
            return _operationsList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _operationsList.GetEnumerator();
        }

        public bool HasNonYieldingOperations
        {
            get
            {
                return _yieldDetection.HasExecuted;
            }
        }

        public string CreateWorkflowGraph(Func<Type, string> labelFormatter)
        {
            return _dependencyGraph.CreateGraph();
        }
    }
}
