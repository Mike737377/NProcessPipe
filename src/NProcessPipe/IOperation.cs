using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NProcessPipe
{

    public interface IOperation<T> : IOperation<T, DefaultProcessContext>
    { }

    public interface IOperation<T, TContext>
        where TContext : IProcessContext
    {
        IEnumerable<T> Execute(TContext context, IEnumerable<T> data);
    }

    public interface IOperationDependsOn<T>
    { }
}
