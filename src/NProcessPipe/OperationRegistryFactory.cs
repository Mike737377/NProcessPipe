using NProcessPipe.DependencyAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NProcessPipe
{
    public static class OperationRegistryFactory
    {
        public static OperationRegistryFactory<TOperationRow, TOperationContext> Build<TOperationRow, TOperationContext>()
            where TOperationRow : class
            where TOperationContext : IProcessContext
        {
            return new OperationRegistryFactory<TOperationRow, TOperationContext>();
        }
    }

    public class OperationRegistryFactory<TOperationRow, TOperationContext>
        where TOperationRow : class
        where TOperationContext : IProcessContext
    {
        private static readonly Type _dependencyType = typeof(IOperationDependsOn<>);

        private readonly Type _operationType = typeof(IOperation<TOperationRow, TOperationContext>);
        private readonly HashSet<Type> _locatedOperations = new HashSet<Type>();

        public OperationRegistryFactory()
        {
            if (_operationType.IsClass)
            {
                throw new ArgumentException("TOperation must be an interface", "TOperation");
            }
        }

        public OperationRegistryFactory<TOperationRow, TOperationContext> ScanAssembly()
        {
            return ScanAssembly(typeof(TOperationRow).Assembly);
        }

        public OperationRegistryFactory<TOperationRow, TOperationContext> ScanAssembly(Assembly assembly)
        {
            var foundOperationTypes = assembly.GetTypes().Where(x => !x.IsAbstract && _operationType.IsAssignableFrom(x));

            foreach (var foundOperation in foundOperationTypes)
            {
                _locatedOperations.Add(foundOperation);
            }

            return this;
        }

        public OperationRegistryFactory<TOperationRow, TOperationContext> AddOperation<TOperation>()
            where TOperation : IOperation<TOperationRow>
        {
            _locatedOperations.Add(typeof(TOperation));
            return this;
        }

        private IEnumerable<Type> GetDependenciesForType(Type type)
        {
            var dependencyDeclarations = type.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == _dependencyType);
            return dependencyDeclarations.Select(x => x.GetGenericArguments()[0]);
        }

        public OperationRegistry<TOperationRow, TOperationContext> CreateFor(IProcessAccessor process)
        {
            var dependencyGraph = new DependencyGraph<Type>();
            dependencyGraph.AddNodes(_locatedOperations);

            foreach (var operation in _locatedOperations)
            {
                var dependencies = GetDependenciesForType(operation);
                foreach (var target in dependencies)
                {
                    dependencyGraph.Connect(operation, target);
                }
            }

            var orderedOperations = dependencyGraph.Order();
            var operationInstances = orderedOperations.Select(x => Activator.CreateInstance(x.Data)).Cast<IOperation<TOperationRow, TOperationContext>>();
            var graph = dependencyGraph.CreateGraph();

            return new OperationRegistry<TOperationRow, TOperationContext>(process, operationInstances, graph);
        }

        public class OperationCreationException : Exception
        {
            public OperationCreationException(Type operationType, Exception innerException)
                : base(string.Format("Failed to created operation {0}.{1}. See inner exception for more details.", operationType.Namespace, operationType.Name), innerException)
            { }
        }



    }

}
