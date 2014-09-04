using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;

namespace NProcessPipe.Tests.ProcessCases.Scenarios
{
    public class ContinueOnExceptioningRowProcess : TestProcess<ContinueOnExceptioningRowProcessRow>
    {
        public class FirstStep : IOperation<ContinueOnExceptioningRowProcessRow>
        {
            public IEnumerable<ContinueOnExceptioningRowProcessRow> Execute(DefaultProcessContext context, IEnumerable<ContinueOnExceptioningRowProcessRow> data)
            {
                foreach (var row in data)
                {
                    row.FirstOperationRun = true;
                    yield return row;
                }
            }
        }

        public class ExceptioningStep : IOperation<ContinueOnExceptioningRowProcessRow>, IOperationDependsOn<FirstStep>
        {
            public IEnumerable<ContinueOnExceptioningRowProcessRow> Execute(DefaultProcessContext context, IEnumerable<ContinueOnExceptioningRowProcessRow> data)
            {
                foreach (var row in data)
                {
                    row.ExceptionOperationRun = true;

                    if (row.CauseException)
                    {
                        throw new Exception("Row exception");
                    }

                    yield return row;
                }
            }
        }

        public class LastStep : IOperation<ContinueOnExceptioningRowProcessRow>, IOperationDependsOn<ExceptioningStep>
        {
            public IEnumerable<ContinueOnExceptioningRowProcessRow> Execute(DefaultProcessContext context, IEnumerable<ContinueOnExceptioningRowProcessRow> data)
            {
                foreach (var row in data)
                {
                    row.LastOperationRun = true;
                    yield return row;
                }
            }
        }
    }

    public class ContinueOnExceptioningRowProcessRow
    {
        public bool CauseException { get; set; }

        public bool FirstOperationRun { get; set; }
        public bool ExceptionOperationRun { get; set; }
        public bool LastOperationRun { get; set; }
    }

    public class ContinueOnExceptioningRowScenarioTests
    {
        private readonly ContinueOnExceptioningRowProcessRow[] _data;
        private readonly ContinueOnExceptioningRowProcess _process;

        public ContinueOnExceptioningRowScenarioTests()
        {
            _data = new ContinueOnExceptioningRowProcessRow[] { new ContinueOnExceptioningRowProcessRow() { CauseException = false }, new ContinueOnExceptioningRowProcessRow(), new ContinueOnExceptioningRowProcessRow() };
            _process = new ContinueOnExceptioningRowProcess();
            _process.ContinueOnError = true;
            _process.Execute(_data);
        }

        public class GetAllErrors : ForExceptioningRow
        {
            private readonly Exception[] _errors;

            public GetAllErrors()
            {
                _errors = _process.GetAllErrors().ToArray();
            }

            [Test]
            public void ShouldHaveRecordedOneError()
            {
                _errors.Length.ShouldBe(1);
            }

            public class ProcessRowExceptionForExceptioningRow : GetAllErrors
            {
                private readonly ProcessRowException _errorDetails;
                private readonly ContinueOnExceptioningRowProcessRow _exceptioningRow;

                public ProcessRowExceptionForExceptioningRow()
                {
                    _errorDetails = _errors[0] as ProcessRowException;
                    _exceptioningRow = _data.First(x => x.CauseException);
                }

                [Test]
                public void ShouldHaveRowNumber()
                {
                    _errorDetails.Row.ShouldBe(0);
                }

                [Test]
                public void ShouldHaveRowData()
                {                    
                    _errorDetails.RowData.ShouldBe(_exceptioningRow);
                }
            }
        }

        public class ForExceptioningRow : ContinueOnExceptioningRowScenarioTests
        {
            private readonly ContinueOnExceptioningRowProcessRow _exceptioningRow;

            public ForExceptioningRow()
            {
                _exceptioningRow = _data.First(x => x.CauseException);
            }

            [Test]
            public void ShouldHaveRunFirstOperation()
            {
                _exceptioningRow.FirstOperationRun.ShouldBe(true);
            }

            [Test]
            public void ShouldHaveRunExceptioningOperation()
            {
                _exceptioningRow.ExceptionOperationRun.ShouldBe(true);
            }

            [Test]
            public void ShouldNotHaveRunLastOperation()
            {
                _exceptioningRow.LastOperationRun.ShouldBe(false);
            }
        }

        public class ForNonExceptioningRow : ContinueOnExceptioningRowScenarioTests
        {
            private readonly ContinueOnExceptioningRowProcessRow _nonExceptioningRow;

            public ForNonExceptioningRow()
            {
                _nonExceptioningRow = _data.First(x => !x.CauseException);
            }

            [Test]
            public void ShouldHaveRunFirstOperation()
            {
                _nonExceptioningRow.FirstOperationRun.ShouldBe(true);
            }

            [Test]
            public void ShouldHaveRunExceptioningOperation()
            {
                _nonExceptioningRow.ExceptionOperationRun.ShouldBe(true);
            }

            [Test]
            public void ShouldHaveRunLastOperation()
            {
                _nonExceptioningRow.LastOperationRun.ShouldBe(true);
            }
        }

        [Test]
        public void ShouldCreateCorrectWorkflowGraph()
        {
            var graph = _process.Diagnostics.GetWorkflowGraph();

            graph.Trim().ShouldBe(@"digraph G {
0 [label=""NProcessPipe.Tests.ProcessCases.Scenarios.ContinueOnExceptioningRowProcess+FirstStep""];
1 [label=""NProcessPipe.Tests.ProcessCases.Scenarios.ContinueOnExceptioningRowProcess+ExceptioningStep""];
2 [label=""NProcessPipe.Tests.ProcessCases.Scenarios.ContinueOnExceptioningRowProcess+LastStep""];
0 -> 1 [];
1 -> 2 [];
}"
);
        }
    }

}
