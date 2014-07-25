using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.ProcessWithCustomContext
{
    public class OperationWithCustomContext1 : IOperation<CustomContextProcessRow, CustomContext>
    {
        public IEnumerable<CustomContextProcessRow> Execute(CustomContext context, IEnumerable<CustomContextProcessRow> data)
        {
            context.Message = Guid.NewGuid().ToString();
            Console.WriteLine("Stuffing message '{0}' into context", context.Message);
            return data;
        }
    }

    public class OperationWithCustomContext2 : IOperation<CustomContextProcessRow, CustomContext>, IOperationDependsOn<OperationWithCustomContext1>
    {
        public IEnumerable<CustomContextProcessRow> Execute(CustomContext context, IEnumerable<CustomContextProcessRow> data)
        {
            Console.WriteLine("Retrieving message '{0}' from context", context.Message);
            return data;
        }
    }

}
