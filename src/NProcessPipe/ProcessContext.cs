using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NProcessPipe
{
    public interface IProcessContext
    {
        IDictionary<string, dynamic> Data {get;}
        IProcessLogger Log { get; }
    }

    public class ProcessContext : IProcessContext
    {

        public ProcessContext(IProcessLogger log)
        {
            Data = new Dictionary<string, object>();
            Log = log;
        }

        public IDictionary<string, dynamic> Data { get; private set; }
        public IProcessLogger Log { get; private set; }
    }
}
