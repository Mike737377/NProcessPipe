using NProcessPipe.DependencyAnalysis;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;

namespace NProcessPipe.Tests.DependencyAnalysis
{
    public class DependencyGraphTests
    {

        [TestFixture]
        public class WhenNoDependencies
        {
            private readonly DependencyGraph<string> _graph;

            public WhenNoDependencies()
            {
                _graph = new DependencyGraph<string>();
                _graph.AddNode("c");
                _graph.AddNode("a");
                _graph.AddNode("b");                
            }

            [Test]
            public void ListsInAddedOrder()
            {
                var order = _graph.Order().ToArray();

                order[0].Data.ShouldBe("c");
                order[1].Data.ShouldBe("a");
                order[2].Data.ShouldBe("b");
            }
        }

        [TestFixture]
        public class WithSingleDependencyChain
        {
            private readonly DependencyGraph<string> _graph;

            public WithSingleDependencyChain()
            {
                _graph = new DependencyGraph<string>();
                _graph.AddNode("3");                
                _graph.AddNode("0");
                _graph.AddNode("1");
                _graph.AddNode("2");
                _graph.Connect("3", "1");
                _graph.Connect("3", "2");
                _graph.Connect("2", "1");
                _graph.Connect("1", "0");
            }

            [Test]
            public void ListsInOrder()
            {
                var order = _graph.Order().ToArray();

                order[0].Data.ShouldBe("0");
                order[1].Data.ShouldBe("1");
                order[2].Data.ShouldBe("2");
                order[3].Data.ShouldBe("3");
            }
        }

    }
}
