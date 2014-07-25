using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.SendOrders
{
    public class SendOrderProcess : Process<SendOrderRow>
    {
        public SendOrderProcess()
        {
            _log = new ConsoleLog();
        }
    }

    public class SendOrderProcessContext
    {
    }

    public class SendOrderRow
    {
        public Order Order { get; set; }
    }

    public class Order
    {
        public Order()
        {
            Lines = new List<OrderLine>();
        }

        public int OrderId { get; set; }
        public List<OrderLine> Lines { get; private set; }
    }

    public class OrderLine
    {
        public string Description { get; set; }
        public int Units { get; set; }
        public int PricePerUnit { get; set; }

        public int LineAmount
        {
            get
            {
                return PricePerUnit * Units;
            }
        }
    }
}
