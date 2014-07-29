using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.ProductImport
{
    public class ProductImportProcessContext : IProcessContext
    {

        public ProductImportProcessContext(IProcessLogger log, IDictionary<string, dynamic> contextData)
        {
            Log = log;
            Database = contextData["database"];
        }

        public IProcessLogger Log { get; private set; }
        public SimpleProductDatabase Database { get; private set; }

    }
}
