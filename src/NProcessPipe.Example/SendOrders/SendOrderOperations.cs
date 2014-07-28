using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.SendOrders
{

    public class PrintDeliverySticker : IOperation<SendOrderRow>
    {
        public IEnumerable<SendOrderRow> Execute(DefaultProcessContext context, IEnumerable<SendOrderRow> data)
        {
            foreach (var row in data)
            {
                yield return row;
            }
        }
    }

    public class PrintInvoice : IOperation<SendOrderRow>
    {
        public IEnumerable<SendOrderRow> Execute(DefaultProcessContext context, IEnumerable<SendOrderRow> data)
        {
            foreach (var row in data)
            {
                yield return row;
            }
        }
    }


    public class CreatePickingSlip : IOperation<SendOrderRow>
    {
        public IEnumerable<SendOrderRow> Execute(DefaultProcessContext context, IEnumerable<SendOrderRow> data)
        {
            foreach (var row in data)
            {
                yield return row;
            }
        }
    }

    public class PrintPickingSlip : IOperation<SendOrderRow>, IOperationDependsOn<CreatePickingSlip>
    {
        public IEnumerable<SendOrderRow> Execute(DefaultProcessContext context, IEnumerable<SendOrderRow> data)
        {
            foreach (var row in data)
            {
                yield return row;
            }
        }
    }


    public class GatherPaperwork : IOperation<SendOrderRow>,
        IOperationDependsOn<PrintDeliverySticker>,
        IOperationDependsOn<PrintInvoice>,
        IOperationDependsOn<PrintPickingSlip>
    {
        public IEnumerable<SendOrderRow> Execute(DefaultProcessContext context, IEnumerable<SendOrderRow> data)
        {
            foreach (var row in data)
            {
                yield return row;
            }
        }
    }
}
