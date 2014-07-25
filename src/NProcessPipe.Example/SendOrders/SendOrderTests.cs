﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.SendOrders
{
    public class SendOrderTests
    {

        public void Test()
        {

            var ordersToSend = new List<SendOrderRow>();
            ordersToSend.Add(new SendOrderRow() { Order = new Order() });

            var process = new SendOrderProcess();
            process.Execute(ordersToSend);

        }

    }
}
