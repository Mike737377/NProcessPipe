using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Tests.ProcessCases
{
    public class ProcessEventLog : IEnumerable<ProcessEvent>
    {

        private List<ProcessEvent> _events = new List<ProcessEvent>();

        public void Add(ProcessEvent eventOccurred)
        {
            _events.Add(eventOccurred);
            Console.WriteLine(eventOccurred.ToString());
        }

        public ProcessEvent this[int index]
        {
            get
            {
                return _events[index];
            }
        }

        public IEnumerator<ProcessEvent> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _events.GetEnumerator();
        }
    }

    public enum ProcessEvent
    {
        Begun,
        OperationStart,
        RowProcessed,
        OperationEnd,
        Complete
    }
}
