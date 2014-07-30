using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.InternalOperations
{
    internal class RowProcessingEndingOperation<TOperationRow, TOperationContext> : IOperation<TOperationRow, TOperationContext>
        where TOperationRow : class
        where TOperationContext : IProcessContext
    {

        private readonly IProcessAccessor _process;
        private readonly int _notifyEveryNumberOfRows = 25;

        public int RowsProcessed { get; private set; }
        public bool DisplayMessages { get; set; }

        public RowProcessingEndingOperation(IProcessAccessor process)
        {
            _process = process;
            DisplayMessages = true;
        }

        public IEnumerable<TOperationRow> Execute(TOperationContext context, IEnumerable<TOperationRow> data)
        {
            foreach (var row in data)
            {
                RowsProcessed++;

                if (DisplayMessages)
                {
                    _process.Log.Trace("Row {0} completed for {1}", RowsProcessed, _process.ProcessName);

                    if (RowsProcessed % _notifyEveryNumberOfRows == 0)
                    {
                        _process.Log.Info("{0} rows processed in {1}", RowsProcessed, _process.ProcessName);
                    }
                }

                yield return row;
            }
        }

    }
}
