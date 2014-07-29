using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Tests.ProcessCases.NoOperations.Scenarios
{
    public class NoOperationsProcess : TestProcess<NoOperationsRow>
    { }

    public class NoOperationsRow
    { }

    public class NoOperationsScenarioTests
    {
        private readonly NoOperationsRow[] _data;
        private readonly NoOperationsProcess _process;

        public NoOperationsScenarioTests()
        {
            _data = new NoOperationsRow[] { new NoOperationsRow() };
            _process = new NoOperationsProcess();
        }

        [Test]
        public void ShouldNotThrowAnyExceptions()
        {
            _process.Execute(_data);
        }
    }
}
