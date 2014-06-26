using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.DependencyAnalysis
{
    public class Edge<T>
    {
        public Edge(Node<T> source, Node<T> target)
        {
            //source depends on target
            Source = source;
            Target = target;
        }


        public Node<T> Source { get; private set; }
        public Node<T> Target { get; private set; }

        public bool Equals(Edge<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Source, Source) && Equals(other.Target, Target);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Edge<T>)) return false;
            return Equals((Edge<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Source != null ? Source.GetHashCode() : 0) * 397) ^ (Target != null ? Target.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("Edge: {0}->{1}", Source, Target);
        }
    }
}
