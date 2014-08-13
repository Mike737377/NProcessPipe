using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NProcessPipe
{
    public abstract class Process<T> : Process<T, DefaultProcessContext>
        where T : class
    {
        protected override DefaultProcessContext CreateProcessContext(IProcessLogger log, IDictionary<string, dynamic> contextData)
        {
            return new DefaultProcessContext(log, contextData);
        }
    }

    public interface IProcessAccessor
    {
        string ProcessName { get; }
        IProcessLogger Log { get; }
    }

    public abstract class Process<T, TContext> : IProcess<T, TContext>, IProcessAccessor
        where T : class
        where TContext : IProcessContext
    {
        private IProcessLogger _log;
        private readonly List<Exception> errors = new List<Exception>();

        public string ProcessName { get; set; }

        protected abstract IProcessLogger CreateLog();
        protected abstract TContext CreateProcessContext(IProcessLogger log, IDictionary<string, dynamic> contextData);

        protected virtual void Initialise(OperationRegistryFactory<T, TContext> operationRegistryFactory) { }
        protected virtual void BeginningExecution(TContext context) { }
        protected virtual void CompletingExecution(TContext context) { }
        protected virtual void AbortingExecution(TContext context) { }

        public void Execute(IEnumerable<T> processData)
        {
            Execute(processData, new Dictionary<string, dynamic>());
        }

        public void Execute(IEnumerable<T> processData, IDictionary<string, dynamic> contextData)
        {
            TContext context;
            OperationRegistry<T, TContext> operations;

            try
            {
                _log = CreateLog();

                if (string.IsNullOrWhiteSpace(ProcessName))
                {
                    ProcessName = this.GetType().Name;
                }

                if (processData.Count() == 0)
                {
                    _log.Info("{0} has no data to process, ending execution", ProcessName);
                    return;
                }

                context = CreateProcessContext(_log, contextData);
                BeginningExecution(context);

                operations = GetOperations();
            }
            catch (Exception ex)
            {
                var message = string.Format("Failed to create pipeline {0}", ProcessName);
                _log.Error(ex, message);
                throw new ProcessException(message, ex);
            }

            try
            {
                _log.Info("Started process {0} at {1}", ProcessName, DateTime.UtcNow);
                var timer = Stopwatch.StartNew();
                var rowsProcessed = ExecuteOperations(operations, context, processData);

                timer.Stop();
                _log.Info("{0} rows processed in {1}", rowsProcessed, ProcessName);
                _log.Info("Completed process {0} at {1} in {2}", ProcessName, DateTime.UtcNow, timer.Elapsed);
                CompletingExecution(context);
            }
            catch (Exception ex)
            {
                var message = string.Format("Failed during execution of pipeline {0} while processing row", ProcessName);

                _log.Error(ex, message);
                AbortingExecution(context);

                throw new ProcessException(message, ex);
            }
        }

        protected OperationRegistry<T, TContext> GetOperations()
        {
            var operationFactory = OperationRegistryFactory.Build<T, TContext>().ScanAssembly();
            Initialise(operationFactory);
            var ops = operationFactory.CreateFor(this);
            _log.Trace("Creating workflow pipeline: \r\n{0}", ops.CreateWorkflowGraph());
            return ops;
        }

        private int ExecuteOperations(OperationRegistry<T, TContext> operations, TContext context, IEnumerable<T> processData)
        {
            var yeildWarning = false;
            var firstOperationHasExecuted = false;

            foreach (var operation in operations)
            {
                var operationEnumerator = operation.Execute(context, processData);

                if (!firstOperationHasExecuted && operations.HasNonYieldingOperations)
                {
                    firstOperationHasExecuted = true;
                    yeildWarning = true;
                    _log.Warn("Process has non yielding operations. Process will run per operation instead of per row.");
                    operations.DisplayYieldedMessages(false);
                }

                processData = new ProcessEnumerable<T>(operationEnumerator);
            }

            var rowsSuccessfullyProcessed = yeildWarning ? processData.Count() : 0;

            if (!yeildWarning)
            {
                var dataEnumerator = processData.GetEnumerator();
                while (dataEnumerator.MoveNext())
                {
                    rowsSuccessfullyProcessed++;
                }
            }

            return rowsSuccessfullyProcessed;
        }

        public IEnumerable<Exception> GetAllErrors()
        {
            return errors.ToArray();
        }

        IProcessLogger IProcessAccessor.Log
        {
            get
            {
                return _log;
            }
        }
    }
}
