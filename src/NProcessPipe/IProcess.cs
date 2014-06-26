using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NProcessPipe
{
    public interface IProcess<T>
    {
        void Execute(IEnumerable<T> processData);

        IEnumerable<Exception> GetAllErrors();
    }
}
