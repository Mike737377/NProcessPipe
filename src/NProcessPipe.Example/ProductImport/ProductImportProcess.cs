using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.ProductImport
{
    public class ProductImportProcess : Process<ProductImportRow, ProductImportProcessContext>
    {
        protected override IProcessLogger CreateLog()
        {
            return new ConsoleLog();
        }

        protected override ProductImportProcessContext CreateProcessContext(IProcessLogger log, IDictionary<string, dynamic> contextData)
        {
            return new ProductImportProcessContext(log, contextData);
        }
    }
}
