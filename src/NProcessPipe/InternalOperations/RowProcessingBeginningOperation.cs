using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.InternalOperations
{
    internal class RowProcessingBeginningOperation<TOperationRow, TOperationContext> : IOperation<TOperationRow, TOperationContext>
        where TOperationRow : class
        where TOperationContext : IProcessContext
    {

        private readonly IProcessAccessor _process; 
        private int _rowsProcessed;
        
        public bool DisplayMessages { get; set; }

        public RowProcessingBeginningOperation(IProcessAccessor process)
        {
            _process = process;
            DisplayMessages = true;
        }

        public IEnumerable<TOperationRow> Execute(TOperationContext context, IEnumerable<TOperationRow> data)
        {
            foreach (var row in data)
            {
                _rowsProcessed++;

                if (DisplayMessages)
                {
                    _process.Log.Trace("Row {0} processing beginning for {1}", _rowsProcessed, _process.ProcessName);
                }

                yield return row;
            }
        }
    }
}
