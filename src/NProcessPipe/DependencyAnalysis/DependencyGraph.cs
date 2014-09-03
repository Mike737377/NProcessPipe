using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.DependencyAnalysis
{
    public class DependencyGraph<T>
    {
        private readonly HashSet<Edge<T>> _edges = new HashSet<Edge<T>>();
        private readonly HashSet<Node<T>> _nodes = new HashSet<Node<T>>();
        private Guid _revisionVersion = Guid.Empty;
        private Guid _orderedRevisionVersion = Guid.Empty;
        private IEnumerable<Node<T>> orderedNodes;

        public void AddNode(T node)
        {
            AddNode(new Node<T>(node));
        }

        public void AddNode(Node<T> node)
        {
            if (!_nodes.Contains(node))
            {
                _nodes.Add(node);
                _revisionVersion = Guid.NewGuid();
            }
        }

        public void AddNodes(IEnumerable<T> nodes)
        {
            foreach (var node in nodes)
            {
                AddNode(node);
            }
        }

        public void AddNodes(IEnumerable<Node<T>> nodes)
        {
            foreach (var node in nodes)
            {
                AddNode(node);
            }
        }

        public void Connect(T source, T target)
        {
            _edges.Add(new Edge<T>(new Node<T>(source), new Node<T>(target)));
            _revisionVersion = Guid.NewGuid();
        }

        public IEnumerable<Node<T>> Order()
        {
            if (_orderedRevisionVersion != _revisionVersion)
            {
                var sort = new TopologicalSort(this);
                orderedNodes = sort.Order();
                _orderedRevisionVersion = _revisionVersion;
            }

            return orderedNodes ?? new List<Node<T>>();
        }

        public string CreateGraph()
        {
            return CreateGraph(null);
        }

        public string CreateGraph(Func<T, string> labelFormatter)
        {
            if (orderedNodes == null)
            {
                return string.Empty;
            }

            if (labelFormatter == null)
            {
                labelFormatter = new Func<T, string>(x => x.ToString());
            }

            var s = new StringBuilder();
            s.AppendLine("digraph G {");

            var operationNumber = 0;
            var nodeMap = new Dictionary<Node<T>, string>();

            foreach (var node in orderedNodes)
            {
                nodeMap.Add(node, operationNumber.ToString());
                s.AppendLine(string.Format(@"{0} [label=""{1}""];", operationNumber, labelFormatter(node.Data)));
                operationNumber++;
            }

            foreach (var edge in _edges)
            {
                s.AppendLine(string.Format(@"{0} -> {1} [];", nodeMap[edge.Target], nodeMap[edge.Source]));
            }

            s.AppendLine("}");

            return s.ToString();
        }

        private class TopologicalSort
        {

            private DependencyGraph<T> _graph;

            public TopologicalSort(DependencyGraph<T> graph)
            {
                _graph = graph;
            }

            private IEnumerable<Node<T>> GetEntryNodes(List<Edge<T>> edges, IEnumerable<Node<T>> availableNodes)
            {
                var entryNodes = new List<Node<T>>();
                foreach (var node in availableNodes)
                {
                    if (!edges.Any(edge => edge.Source.Equals(node)))
                    {
                        //you have no dependencies on you
                        edges.RemoveAll(x => x.Target.Equals(node));
                        entryNodes.Add(node);
                    }
                }
                return entryNodes;
            }

            public IEnumerable<Node<T>> Order()
            {
                var edges = _graph._edges.ToList();
                var order = new List<Node<T>>();
                var availableNodes = _graph._nodes.ToList();

                while (availableNodes.Count > 0)
                {
                    var nodes = GetEntryNodes(edges, availableNodes);

                    if (nodes.Count() == 0)
                    {
                        throw new CircularDependencyException(edges.Select(x => string.Format("{0}->{1}", x.Source, x.Target)));
                    }

                    foreach (var node in nodes)
                    {
                        availableNodes.Remove(node);
                        if (!order.Contains(node))
                        {
                            order.Add(node);
                        }
                    }
                }

                return order;
            }
        }
    }
}
