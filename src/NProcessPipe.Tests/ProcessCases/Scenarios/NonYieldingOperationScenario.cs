using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using NSubstitute;

namespace NProcessPipe.Tests.ProcessCases.Scenarios
{

    public class NonYieldingOperationProcess : Process<NonYieldingOperationRow>
    {

        private readonly IProcessLogger _log;
        public static ProcessEventLog ExecutionOrder { get; private set; }

        public NonYieldingOperationProcess(IProcessLogger log)
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

        public class FirstNonYieldingOperation : IOperation<NonYieldingOperationRow>
        {
            public IEnumerable<NonYieldingOperationRow> Execute(DefaultProcessContext context, IEnumerable<NonYieldingOperationRow> data)
            {

                ExecutionOrder.Add(ProcessEvent.OperationStart);

                Console.WriteLine("Beginning first non yielding operation");

                foreach (var row in data)
                {
                    Console.WriteLine("NonYield-1 " + row.RowId);
                    ExecutionOrder.Add(ProcessEvent.RowProcessed);
                }

                Console.WriteLine("Completed first non yielding operation");

                ExecutionOrder.Add(ProcessEvent.OperationEnd);

                return data;
            }
        }

        public class LastNonYieldingOperation : IOperation<NonYieldingOperationRow>, IOperationDependsOn<FirstNonYieldingOperation>
        {
            public IEnumerable<NonYieldingOperationRow> Execute(DefaultProcessContext context, IEnumerable<NonYieldingOperationRow> data)
            {
                ExecutionOrder.Add(ProcessEvent.OperationStart);

                Console.WriteLine("Beginning last non yielding operation");

                foreach (var row in data)
                {
                    Console.WriteLine("NonYield-2 " + row.RowId);
                    ExecutionOrder.Add(ProcessEvent.RowProcessed);
                }

                Console.WriteLine("Completed last non yielding operation");

                ExecutionOrder.Add(ProcessEvent.OperationEnd);

                return data;
            }
        }

    }

    public class NonYieldingOperationRow
    {
        public NonYieldingOperationRow()
        {
            RowId = Guid.NewGuid();
        }

        public Guid RowId { get; private set; }
    }

    public class NonYieldingOperationScenarioTests
    {
        private readonly IProcessLogger _logger;
        private readonly NonYieldingOperationRow[] _data;
        private readonly NonYieldingOperationProcess _process;

        public NonYieldingOperationScenarioTests()
        {
            _logger = Substitute.For<IProcessLogger>();
            _data = new NonYieldingOperationRow[] { new NonYieldingOperationRow(), new NonYieldingOperationRow() };
            _process = new NonYieldingOperationProcess(_logger);
            _process.Execute(_data);
        }

        [Test]
        public void ShouldHaveDisplayedNonYieldingWarning()
        {
            _logger.ReceivedWithAnyArgs().Warn(string.Empty);
        }

        [Test]
        public void ShouldHaveCalledBeginningExecutionFirst()
        {
            NonYieldingOperationProcess.ExecutionOrder.First().ShouldBe(ProcessEvent.Begun);
        }

        [Test]
        public void ShouldHaveCalledCompletingExecutionLast()
        {
            NonYieldingOperationProcess.ExecutionOrder.Last().ShouldBe(ProcessEvent.Complete);
        }

    }
}
