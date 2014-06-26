//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Diagnostics;

//namespace NProcessPipe
//{
//    public abstract class ProcessWithCustomContext<TRow, TContext> : IProcess<T, TContext>
//        where TRow : class
//    {
//        private readonly IProcessLogger _log;
//        private readonly List<Exception> errors = new List<Exception>();

//        public string ProcessName { get; set; }

//        protected virtual void Initialise(OperationRegistryFactory<T> operationRegistryFactory)
//        { }

//        protected virtual TContext CreateProcessContext()
//        {
//            return new 
//        }

//        public void Execute(IEnumerable<TRow> processData)
//        {
//            try
//            {
//                database.BeginTransaction();

//                if (ProcessName.IsEmpty())
//                {
//                    ProcessName = this.GetType().Name;
//                }

//                var context = new ProcessContext(database);
//                var pipeline = PreparePipeline(context, processData);
//                var enumerator = pipeline.GetEnumerator();

//                try
//                {
//                    _log.Info("Started process {0} at {1}", ProcessName, DateTime.Now);
//                    var timer = Stopwatch.StartNew();

//                    int rowsProcessed = 0;
//#if DEBUG
//                    try
//                    {
//#endif
//                        while (enumerator.MoveNext())
//                        {
//                            if (rowsProcessed > 0 && rowsProcessed % 25 == 0)
//                            {
//                                _log.Info("{0} rows processed in {1}", rowsProcessed, ProcessName);
//                            }
//                            rowsProcessed++;
//                        }
//#if DEBUG
//                    }
//                    catch
//                    {
//                        if (Debugger.IsAttached)
//                        {
//                            Debugger.Break();
//                        }

//                        throw;
//                    }
//#endif

//                    timer.Stop();
//                    _log.Info("{0} rows processed in {1}", rowsProcessed, ProcessName);
//                    _log.Info("Completed process {0} at {1} in {2}", ProcessName, DateTime.Now, timer.Elapsed);
//                }
//                catch (Exception ex)
//                {
//                    string message = string.Format("Failed during execution of pipeline {0} with data {1}", ProcessName, enumerator.Current);
//                    errors.Add(new ProcessException(message, ex));
//                    _log.Error(message, ex);
//                }

//                database.CompleteTransaction();
//            }
//            catch (Exception ex)
//            {
//                string message = string.Format("Failed to create pipeline {0}", ProcessName);
//                errors.Add(new ProcessException(message, ex));
//                _log.Error(message, ex);
//                database.AbortTransaction();
//            }
//        }

//        protected OperationRegistry<T> PrepareOperations()
//        {
//            var operationFactory = OperationRegistryFactory.Build<T>().ScanAssembly();
//            Initialise(operationFactory);
//            return operationFactory.Create();
//        }

//        private IEnumerable<T> PreparePipeline(IProcessContext context, IEnumerable<T> processData)
//        {
//            var ops = PrepareOperations();
//            _log.Trace("Creating workflow pipeline: \r\n{0}", ops.CreateWorkflowGraph());

//            foreach (var operation in ops)
//            {
//                var enumerator = operation.Execute(context, processData);
//                processData = new ProcessEnumerable<T>(enumerator);
//            }

//            return processData;
//        }

//        public IEnumerable<Exception> GetAllErrors()
//        {
//            return errors.ToArray();
//        }

//        /// <summary>
//        /// Creates the process graph.
//        /// </summary>
//        /// <remarks>Use http://graphviz-dev.appspot.com/ to view an image of the graph or https://github.com/mdaines/viz.js/ to generate on the fly svg in html</remarks>
//        /// <param name="fileName">Name of the file. (optional)</param>
//        /// <returns>Dot file format notation</returns>
//        public string CreateProcessGraph(string fileName = null)
//        {
//            var ops = PrepareOperations();
//            return ops.CreateWorkflowGraph(fileName);
//        }
//    }
//}
