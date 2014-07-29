using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;

namespace NProcessPipe.Tests.ProcessCases.Scenarios
{
    public class OrderedProcess : TestProcess<OrderedProcessRow>
    {
        public class FirstStep : IOperation<OrderedProcessRow>
        {
            public IEnumerable<OrderedProcessRow> Execute(DefaultProcessContext context, IEnumerable<OrderedProcessRow> data)
            {
                data.First().FirstOperationRun = true;
                data.First().OperationNameOrder.Add(this.GetType().Name);
                return data;
            }
        }

        public class LastStep : IOperation<OrderedProcessRow>, IOperationDependsOn<FirstStep>
        {
            public IEnumerable<OrderedProcessRow> Execute(DefaultProcessContext context, IEnumerable<OrderedProcessRow> data)
            {
                data.First().LastOperationRun = true;
                data.First().OperationNameOrder.Add(this.GetType().Name);
                return data;
            }
        }    
    }

    public class OrderedProcessRow
    {
        public OrderedProcessRow()
        {
            OperationNameOrder = new List<string>();
        }

        public bool FirstOperationRun { get; set; }
        public bool LastOperationRun { get; set; }
        public List<string> OperationNameOrder { get; private set; }
    }    

    public class OrderedProcessScenarioTests
    {
        private readonly OrderedProcessRow[] _data;
        private readonly OrderedProcess _process;

        public OrderedProcessScenarioTests()
        {
            _data = new OrderedProcessRow[] { new OrderedProcessRow() };
            _process = new OrderedProcess();
            _process.Execute(_data);
        }

        [Test]
        public void ShouldHaveRunFirstOperation()
        {
            _data[0].FirstOperationRun.ShouldBe(true);
        }

        [Test]
        public void ShouldHaveRunLastOperation()
        {
            _data[0].LastOperationRun.ShouldBe(true);
        }

        [Test]
        public void ShouldHaveExecutedFirstStepFirst()
        {
            _data[0].OperationNameOrder[0].ShouldBe("FirstStep");
        }

        [Test]
        public void ShouldHaveExecutedLastStepSecond()
        {
            _data[0].OperationNameOrder[1].ShouldBe("LastStep");
        }

    }
}
