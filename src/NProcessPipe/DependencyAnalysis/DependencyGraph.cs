﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.DependencyAnalysis
{
    public class DependencyGraph<T>
    {
        private readonly HashSet<Edge<T>> _edges = new HashSet<Edge<T>>();
        private readonly HashSet<Node<T>> _nodes = new HashSet<Node<T>>();

        public void AddNode(T node)
        {
            AddNode(new Node<T>(node));
        }

        public void AddNode(Node<T> node)
        {
            if (!_nodes.Contains(node))
            {
                _nodes.Add(node);
            }
        }

        public void AddNodes(IEnumerable<Node<T>> nodes)
        {
            foreach (var node in nodes)
            {
                AddNode(node);
            }
        }

        public IEnumerable<Node<T>> Order()
        {
            var order = new TopologicalSort(this);
            return order.Order();
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


        //class TopologicalSort
        //{
        //    IList<Node<T>> _order;
        //    DependencyGraph<T> _graph;

        //    public TopologicalSort()
        //    {
        //        _order = new List<Node<T>>();
        //    }

        //    public IEnumerable<Node<T>> Order(DependencyGraph<T> graph)
        //    {
        //        _graph = graph.Clone();

        //        while (!_graph.IsEmpty())
        //        {
        //            var nodes = _graph.GetEntryNodes();
        //            foreach (var node in nodes)
        //            {
        //                visit(node);
        //            }
        //        }

        //        return _order;
        //    }

        //    void visit(Node<T> node)
        //    {
        //        _graph.Remove(node);
        //        AddNode(node);
        //    }

        //    void AddNode(Node<T> node)
        //    {
        //        if (!_order.Contains(node))
        //        {
        //            _order.Add(node);
        //        }
        //    }


        //}

        //void Remove(Node<T> node)
        //{
        //    _nodes.Remove(node);
        //    _edges.RemoveWhere(e => e.Target.Equals(node));
        //}

        //public IEnumerable<Node<T>> GetEntryNodes()
        //{
        //    var result = new List<Node<T>>();
        //    foreach (var node in _nodes)
        //    {
        //        if (!_edges.Any(edge => edge.Source.Equals(node)))
        //        {
        //            //you have no dependencies on you
        //            result.Add(node);
        //        }
        //    }
        //    return result;
        //}

        //public bool IsEmpty()
        //{
        //    return _nodes.Count == 0;
        //}

        //public DependencyGraph<T> Clone()
        //{
        //    var result = new DependencyGraph<T>();

        //    foreach (var node in _nodes)
        //    {
        //        var newNode = new Node<T>(node.Data);
        //        result.AddNode(newNode);
        //    }

        //    foreach (var edge in _edges)
        //    {
        //        result.Connect(edge.Source.Data, edge.Target.Data);
        //    }

        //    return result;
        //}

        public void Connect(T source, T target)
        {
            _edges.Add(new Edge<T>(new Node<T>(source), new Node<T>(target)));
        }
    }
}
