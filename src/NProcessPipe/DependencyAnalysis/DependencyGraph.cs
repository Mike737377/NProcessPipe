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
            if (orderedNodes == null)
            {
                return string.Empty;
            }

            var s = new StringBuilder();

            foreach (var node in orderedNodes)
            {
                s.AppendLine(node.Data.ToString());
            }

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
