using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.DependencyAnalysis
{
    public class CircularDependencyException : Exception
    {
        public IEnumerable<string> CircularDependencies { get; private set; }

        public CircularDependencyException(IEnumerable<string> circularDependencies)
            : base("Circular dependencies found")
        {
            CircularDependencies = circularDependencies;
        }
    }
}
