using NProcessPipe.DependencyAnalysis;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                foreach (var o in _graph.Order())
                {
                    Console.WriteLine(o);
                }
            }
        }

        [TestFixture]
        public class WithSingleDependency
        {
            private readonly DependencyGraph<string> _graph;

            public WithSingleDependency()
            {
                _graph = new DependencyGraph<string>();
                _graph.AddNode("3");
                _graph.AddNode("2");
                _graph.AddNode("1");
                _graph.AddNode("0");
                _graph.Connect("3", "1");
                _graph.Connect("3", "2");
                _graph.Connect("1", "0");
            }

            [Test]
            public void ListsInOrder()
            {
                foreach (var o in _graph.Order())
                {
                    Console.WriteLine(o);
                }
            }
        }

    }
}
