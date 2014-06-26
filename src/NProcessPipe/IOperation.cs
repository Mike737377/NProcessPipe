using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NProcessPipe
{
    public interface IOperation<T>
    {
        IEnumerable<T> Execute(IProcessContext context, IEnumerable<T> data);
    }

    public interface IOperationDependsOn<T>
    { }
}
