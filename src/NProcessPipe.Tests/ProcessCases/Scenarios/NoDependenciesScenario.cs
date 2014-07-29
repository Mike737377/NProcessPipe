using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;

namespace NProcessPipe.Tests.ProcessCases.Scenarios
{
    public class NoDependenciesProcess : TestProcess<NoDependenciesRow>
    {
        public class OperationAlpha : IOperation<NoDependenciesRow>
        {
            public IEnumerable<NoDependenciesRow> Execute(DefaultProcessContext context, IEnumerable<NoDependenciesRow> data)
            {
                data.First().OperationAlphaRun = true;
                return data;
            }
        }

        public class OperationBravo : IOperation<NoDependenciesRow>
        {
            public IEnumerable<NoDependenciesRow> Execute(DefaultProcessContext context, IEnumerable<NoDependenciesRow> data)
            {
                data.First().OperationBravoRun = true;
                return data;
            }
        }
    }

    public class NoDependenciesRow
    {
        public bool OperationAlphaRun { get; set; }
        public bool OperationBravoRun { get; set; }
    }

    public class NoDependenciesScenarioTests
    {
        private readonly NoDependenciesRow[] _data;
        private readonly NoDependenciesProcess _process;

        public NoDependenciesScenarioTests()
        {
            _data = new NoDependenciesRow[] { new NoDependenciesRow() };
            _process = new NoDependenciesProcess();
            _process.Execute(_data);
        }

        [Test]
        public void ShouldHaveRunOperationAlpha()
        {
            _data[0].OperationAlphaRun.ShouldBe(true);
        }

        [Test]
        public void ShouldHaveRunOperationBravo()
        {
            _data[0].OperationBravoRun.ShouldBe(true);
        }
    }
}
