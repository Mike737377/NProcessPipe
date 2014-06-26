//using NProcessPipe.DependencyAnalysis;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NProcessPipe
//{
//    public static class OperationRegistryFactory
//    {
//        public static OperationRegistryFactory<TOperationRow> Build<TOperationRow>()
//            where TOperationRow : class
//        {
//            return new OperationRegistryFactory<TOperationRow>();
//        }
//    }

//    public class OperationRegistryFactory<TOperationRow>
//        where TOperationRow : class
//    {
//        private static readonly Type _dependencyType = typeof(IOperationDependsOn<>);

//        private readonly Type _operationType = typeof(IOperation<TOperationRow>);
//        private readonly List<IOperation<TOperationRow>> _locatedOperations = new List<IOperation<TOperationRow>>();

//        public OperationRegistryFactory()
//        {
//            if (_operationType.IsClass)
//            {
//                throw new ArgumentException("TOperation must be an interface", "TOperation");
//            }
//        }

//        public OperationRegistryFactory<TOperationRow> ScanAssembly()
//        {
//            _locatedOperations.AddRange(ObjectFactory.GetAllInstances<IOperation<TOperationRow>>());
//            return this;
//        }

//        public OperationRegistryFactory<TOperationRow> AddOperation(IOperation<TOperationRow> operation)
//        {
//            _locatedOperations.Add(operation);
//            return this;
//        }

//        private IEnumerable<Edge<GraphNode>> GetDependenciesForType(Type type, Dictionary<Type, GraphNode> operationDictionary)
//        {
//            var dependencyDeclarations = type.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == _dependencyType);
//            var vertexDependencies = dependencyDeclarations.Select(x => x.GetGenericArguments()[0]);
//            var vertexEdges = vertexDependencies.Select(x => new Edge<GraphNode>(operationDictionary[x], operationDictionary[type]));

//            return vertexEdges;
//        }

//        public OperationRegistry<TOperationRow> Create()
//        {
//            var operationDictionary = _locatedOperations.Select(x => new GraphNode(x)).ToDictionary(x => x.OperationType);
//            var dependencyGraph = operationDictionary.Select(x => x.Value).ToAdjacencyGraph(x => GetDependenciesForType(x.OperationType, operationDictionary), true);
//            var dependencyList = dependencyGraph.TopologicalSort();
//            var operations = dependencyList.Select(x => x.Operation);

//            return new OperationRegistry<TOperationRow>(operations, dependencyGraph);
//        }

//        public class GraphNode
//        {
//            public GraphNode(IOperation<TOperationRow> operation)
//            {
//                Operation = operation;
//                OperationType = operation.GetType();
//            }

//            public IOperation<TOperationRow> Operation { get; private set; }
//            public Type OperationType { get; private set; }

//            public override string ToString()
//            {
//                return OperationType.Name;
//            }
//        }

//        public class OperationCreationException : Exception
//        {
//            public OperationCreationException(Type operationType, Exception innerException)
//                : base(string.Format("Failed to created operation {0}.{1}. See inner exception for more details.", operationType.Namespace, operationType.Name), innerException)
//            { }
//        }

//    }

//}
