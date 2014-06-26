using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.SendOrders
{
    public interface IPrinter
    {
        PrintJob Print(PrinterDocument document);
    }

    public class Printer : IPrinter
    {
        public PrintJob Print(PrinterDocument document)
        {
            return new PrintJob(document);
        }
    }

    public class PrintJob
    {
        public PrintJob(PrinterDocument document)
        {
            JobId = Guid.NewGuid();
            Document = document;
        }

        public Guid JobId { get; private set; }
        public PrinterDocument Document { get; private set; }
    }

    public class PrinterDocument
    {
        public object ThingToPrint { get; set; }
    }
}
