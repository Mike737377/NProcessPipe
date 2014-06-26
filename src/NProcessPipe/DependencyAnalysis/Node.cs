using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.DependencyAnalysis
{
    public class Node<T>
    {
        public Node(T data)
        {
            //Index = -1;
            Data = data;
        }

        public T Data { get; private set; }

        public bool Equals(Node<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Data, Data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Node<T>)) return false;
            return Equals((Node<T>)obj);
        }

        public override int GetHashCode()
        {
            return (Data != null ? Data.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Data.ToString();
        }

        //public void SetIndex(int index)
        //{
        //    Index = index;
        //}

        //public void SetLowLink(int lowlink)
        //{
        //    LowLink = lowlink;
        //}

        //public int Index { get; private set; }
        //public int LowLink { get; private set; }
    }
}
