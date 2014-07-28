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

        protected override DefaultProcessContext CreateProcessContext()
        {
            return new DefaultProcessContext(_log);
        }
    }

    public abstract class Process<T, TContext> : IProcess<T, TContext>
        where T : class
        where TContext : IProcessContext
    {
        protected IProcessLogger _log;
        private readonly List<Exception> errors = new List<Exception>();

        public string ProcessName { get; set; }

        protected virtual void Initialise(OperationRegistryFactory<T, TContext> operationRegistryFactory)
        { }

        protected virtual TContext CreateProcessContext()
        {
            throw new NotImplementedException();
        }

        public void Execute(IEnumerable<T> processData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace( ProcessName))
                {
                    ProcessName = this.GetType().Name;
                }

                var context = CreateProcessContext();
                var pipeline = PreparePipeline(context, processData);
                var enumerator = pipeline.GetEnumerator();

                try
                {
                    _log.Info("Started process {0} at {1}", ProcessName, DateTime.UtcNow);
                    var timer = Stopwatch.StartNew();

                    int rowsProcessed = 0;
#if DEBUG
                    try
                    {
#endif
                        while (enumerator.MoveNext())
                        {
                            if (rowsProcessed > 0 && rowsProcessed % 25 == 0)
                            {
                                _log.Info("{0} rows processed in {1}", rowsProcessed, ProcessName);
                            }
                            rowsProcessed++;
                        }
#if DEBUG
                    }
                    catch
                    {
                        if (Debugger.IsAttached)
                        {
                            Debugger.Break();
                        }

                        throw;
                    }
#endif

                    timer.Stop();
                    _log.Info("{0} rows processed in {1}", rowsProcessed, ProcessName);
                    _log.Info("Completed process {0} at {1} in {2}", ProcessName, DateTime.UtcNow, timer.Elapsed);
                }
                catch (Exception ex)
                {
                    string message = string.Format("Failed during execution of pipeline {0} with data {1}", ProcessName, enumerator.Current);
                    errors.Add(new ProcessException(message, ex));
                    _log.Error(ex, message);
                }

            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to create pipeline {0}", ProcessName);
                errors.Add(new ProcessException(message, ex));
                _log.Error(ex, message);
            }
        }

        protected OperationRegistry<T, TContext> PrepareOperations()
        {
            var operationFactory = OperationRegistryFactory.Build<T, TContext>().ScanAssembly();
            Initialise(operationFactory);
            return operationFactory.Create();
        }

        private IEnumerable<T> PreparePipeline(TContext context, IEnumerable<T> processData)
        {
            var ops = PrepareOperations();
            _log.Trace("Creating workflow pipeline: \r\n{0}", ops.CreateWorkflowGraph());

            foreach (var operation in ops)
            {
                var enumerator = operation.Execute(context, processData);
                processData = new ProcessEnumerable<T>(enumerator);
            }

            return processData;
        }

        public IEnumerable<Exception> GetAllErrors()
        {
            return errors.ToArray();
        }
    }
}
