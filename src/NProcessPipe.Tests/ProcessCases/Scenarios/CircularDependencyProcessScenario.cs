using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using NProcessPipe.DependencyAnalysis;

namespace NProcessPipe.Tests.ProcessCases.Scenarios
{
    public class CircularDependencyProcess : TestProcess<CircularDependencyProcessRow>
    {
        public class A : IOperation<CircularDependencyProcessRow>, IOperationDependsOn<B>
        {
            public IEnumerable<CircularDependencyProcessRow> Execute(DefaultProcessContext context, IEnumerable<CircularDependencyProcessRow> data)
            {
                return data;
            }
        }

        public class B : IOperation<CircularDependencyProcessRow>, IOperationDependsOn<A>
        {
            public IEnumerable<CircularDependencyProcessRow> Execute(DefaultProcessContext context, IEnumerable<CircularDependencyProcessRow> data)
            {
                return data;
            }
        }
    }

    public class CircularDependencyProcessRow
    { }

    public class CircularDependencyProcessScenarioTests
    {
        private readonly CircularDependencyProcessRow[] _data;
        private readonly CircularDependencyProcess _process;

        public CircularDependencyProcessScenarioTests()
        {
            _data = new CircularDependencyProcessRow[] { new CircularDependencyProcessRow() };
            _process = new CircularDependencyProcess();
        }

        public class WhenThrowingCircularDependencyException : CircularDependencyProcessScenarioTests
        {
            private readonly ProcessException _ex;

            public WhenThrowingCircularDependencyException()
            {
                try
                {
                    _process.Execute(_data);
                }
                catch (ProcessException ex)
                {
                    _ex = ex;
                }
            }

            [Test]
            public void ShouldThrowCircularDependencyException()
            {
                _ex.InnerException.ShouldBeOfType<CircularDependencyException>();
            }

            [Test]
            public void ShouldContainListOfCircularDepenendencies()
            {
                ((CircularDependencyException)_ex.InnerException).CircularDependencies.Count().ShouldBe(2);
            }
        }
    }
}
