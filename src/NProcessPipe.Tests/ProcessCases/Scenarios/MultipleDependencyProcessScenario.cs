using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;

namespace NProcessPipe.Tests.ProcessCases.Scenarios
{

    public class MultipleDependencyProcess : TestProcess<MultipleDependencyProcessRow>
    {
        public class InitialStepA : IOperation<MultipleDependencyProcessRow>
        {
            public IEnumerable<MultipleDependencyProcessRow> Execute(DefaultProcessContext context, IEnumerable<MultipleDependencyProcessRow> data)
            {
                data.First().OperationNameOrder.Add(this.GetType().Name);
                return data;
            }
        }

        public class InitialStepB : IOperation<MultipleDependencyProcessRow>
        {
            public IEnumerable<MultipleDependencyProcessRow> Execute(DefaultProcessContext context, IEnumerable<MultipleDependencyProcessRow> data)
            {
                data.First().OperationNameOrder.Add(this.GetType().Name);
                return data;
            }
        }

        public class LastStep : IOperation<MultipleDependencyProcessRow>,
            IOperationDependsOn<InitialStepA>,
            IOperationDependsOn<InitialStepB>
        {
            public IEnumerable<MultipleDependencyProcessRow> Execute(DefaultProcessContext context, IEnumerable<MultipleDependencyProcessRow> data)
            {
                data.First().OperationNameOrder.Add(this.GetType().Name);
                return data;
            }
        }
    }

    public class MultipleDependencyProcessRow
    {
        public MultipleDependencyProcessRow()
        {
            OperationNameOrder = new List<string>();
        }

        public List<string> OperationNameOrder { get; private set; }
    }

    public class MultipleDependencyProcessScenarioTests
    {
        private readonly MultipleDependencyProcessRow[] _data;
        private readonly MultipleDependencyProcess _process;

        public MultipleDependencyProcessScenarioTests()
        {
            _data = new MultipleDependencyProcessRow[] { new MultipleDependencyProcessRow() };
            _process = new MultipleDependencyProcess();
            _process.Execute(_data);
        }

        [Test]
        public void ShouldHaveExecutedLastStepThird()
        {
            _data[0].OperationNameOrder[2].ShouldBe("LastStep");
        }

        [Test]
        public void ShouldCreateCorrectWorkflowGraph()
        {
            var graph = _process.Diagnostics.GetWorkflowGraph();

            graph.Trim().ShouldBe(@"digraph G {
0 [label=""NProcessPipe.Tests.ProcessCases.Scenarios.MultipleDependencyProcess+InitialStepA""];
1 [label=""NProcessPipe.Tests.ProcessCases.Scenarios.MultipleDependencyProcess+InitialStepB""];
2 [label=""NProcessPipe.Tests.ProcessCases.Scenarios.MultipleDependencyProcess+LastStep""];
0 -> 2 [];
1 -> 2 [];
}"
);
        }
    }

}
