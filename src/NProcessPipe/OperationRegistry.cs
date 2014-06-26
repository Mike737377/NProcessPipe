//using NProcessPipe.DependencyAnalysis;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NProcessPipe
//{
//    public class OperationRegistry<TOperationRow> : IEnumerable<IOperation<TOperationRow>>
//            where TOperationRow : class
//    {

//        private readonly List<IOperation<TOperationRow>> _operationsList = new List<IOperation<TOperationRow>>();
//        private readonly DependencyGraph<OperationRegistryFactory<TOperationRow>.GraphNode> _dependencyGraph;

//        public OperationRegistry(IEnumerable<IOperation<TOperationRow>> operations,
//            DependencyGraph<OperationRegistryFactory<TOperationRow>.GraphNode> dependencyGraph)
//        {
//            _operationsList.AddRange(operations);
//            _dependencyGraph = dependencyGraph;
//        }

//        public IEnumerator<IOperation<TOperationRow>> GetEnumerator()
//        {
//            return _operationsList.GetEnumerator();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return _operationsList.GetEnumerator();
//        }

//        public string CreateWorkflowGraph()
//        {
//            return CreateWorkflowGraph(null);
//        }

//        /// <summary>
//        /// Creates the a graph of the workflow.
//        /// </summary>
//        /// <remarks>Use http://graphviz-dev.appspot.com/ to view an image of the graph or https://github.com/mdaines/viz.js/ to generate on the fly svg in html</remarks>
//        /// <param name="fileName">Name of the file. (optional)</param>
//        /// <returns>Dot file format notation</returns>
//        public string CreateWorkflowGraph(string fileName)
//        {
//            return _dependencyGraph.ToGraphviz(x =>
//            {
//                x.FormatVertex += Graph_FormatVertex;
//                if (fileName.IsNotEmpty())
//                {
//                    x.Generate(new FileDotEngine(), fileName);
//                }
//            });
//        }

//        void Graph_FormatVertex(object sender, FormatVertexEventArgs<OperationRegistryFactory<TOperationRow>.GraphNode> e)
//        {
//            e.VertexFormatter.Label = e.Vertex.ToString();
//        }

//    }
//}
