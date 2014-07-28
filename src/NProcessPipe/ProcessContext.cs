using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NProcessPipe
{
    public interface IProcessContext
    {
    }

    public class DefaultProcessContext : IProcessContext
    {

        public DefaultProcessContext(IProcessLogger log, IDictionary<string, dynamic> data)
        {
            Data = data ?? new Dictionary<string, object>();
            Log = log;
        }

        public IDictionary<string, dynamic> Data { get; private set; }
        public IProcessLogger Log { get; private set; }
    }
}
