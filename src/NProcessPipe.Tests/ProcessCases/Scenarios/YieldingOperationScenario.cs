using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;

namespace NProcessPipe.Tests.ProcessCases.Scenarios
{
    public class YieldingOperationProcess : Process<YieldingOperationRow>
    {
        private readonly IProcessLogger _log;
        public static ProcessEventLog ExecutionOrder { get; private set; }

        public YieldingOperationProcess(IProcessLogger log)
        {
            _log = log;
            ExecutionOrder = new ProcessEventLog();
        }

        protected override IProcessLogger CreateLog()
        {
            return _log;
        }
        
        protected override void BeginningExecution(DefaultProcessContext context)
        {
            ExecutionOrder.Add(ProcessEvent.Begun);
        }

        protected override void CompletingExecution(DefaultProcessContext context)
        {
            ExecutionOrder.Add(ProcessEvent.Complete);
        }

        public class FirstYieldingOperation : IOperation<YieldingOperationRow>
        {
            public IEnumerable<YieldingOperationRow> Execute(DefaultProcessContext context, IEnumerable<YieldingOperationRow> data)
            {
                Console.WriteLine("Start of first yielding operation");
                ExecutionOrder.Add(ProcessEvent.OperationStart);

                foreach (var row in data)
                {
                    Console.WriteLine("Running first yielding operation for row " + row.RowId);
                    ExecutionOrder.Add(ProcessEvent.RowProcessed);
                    yield return row;
                }

                Console.WriteLine("End of first yielding operation");
                ExecutionOrder.Add(ProcessEvent.OperationEnd);
            }
        }

        public class LastYieldingOperation : IOperation<YieldingOperationRow>, IOperationDependsOn<FirstYieldingOperation>
        {
            public IEnumerable<YieldingOperationRow> Execute(DefaultProcessContext context, IEnumerable<YieldingOperationRow> data)
            {
                Console.WriteLine("Start of first yielding operation");
                ExecutionOrder.Add(ProcessEvent.OperationStart);

                foreach (var row in data)
                {
                    Console.WriteLine("Running first yielding operation for row " + row.RowId);
                    ExecutionOrder.Add(ProcessEvent.RowProcessed);
                    yield return row;
                }

                Console.WriteLine("End of first yielding operation");
                ExecutionOrder.Add(ProcessEvent.OperationEnd);
            }
        }

    }

    public class YieldingOperationRow
    {
        public YieldingOperationRow()
        {
            RowId = Guid.NewGuid();
        }

        public Guid RowId { get; private set; }
    }

    public class YieldingOperationScenarioTests
    {
        private readonly YieldingOperationRow[] _data;
        private readonly YieldingOperationProcess _process;
        private readonly IProcessLogger _logger;

        public YieldingOperationScenarioTests()
        {
            _logger = Substitute.For<IProcessLogger>();

            _data = new YieldingOperationRow[] { new YieldingOperationRow(), new YieldingOperationRow() };
            _process = new YieldingOperationProcess(_logger);
            _process.Execute(_data);
        }

        [Test]
        public void ShouldNotHaveDisplayedNonYieldingWarning()
        {
            _logger.DidNotReceiveWithAnyArgs().Warn(string.Empty);
        }

        [Test]
        public void ShouldHaveCalledBeginningExecutionFirst()
        {
            YieldingOperationProcess.ExecutionOrder.First().ShouldBe(ProcessEvent.Begun);
        }

        [Test]
        public void ShouldHaveCalledCompletingExecutionLast()
        {
            YieldingOperationProcess.ExecutionOrder.Last().ShouldBe(ProcessEvent.Complete);
        }

        [Test]
        public void ShouldCreateCorrectWorkflowGraph()
        {
            var graph = _process.Diagnostics.GetWorkflowGraph();

            graph.Trim().ShouldBe(@"digraph G {
0 [label=""NProcessPipe.Tests.ProcessCases.Scenarios.YieldingOperationProcess+FirstYieldingOperation""];
1 [label=""NProcessPipe.Tests.ProcessCases.Scenarios.YieldingOperationProcess+LastYieldingOperation""];
0 -> 1 [];
}"
);
        }

    }
}
