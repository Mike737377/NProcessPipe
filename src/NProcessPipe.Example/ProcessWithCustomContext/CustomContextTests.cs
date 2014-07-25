using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.ProcessWithCustomContext
{
    public class CustomContextTests
    {
        public void Demo()
        {

            var rows = new List<CustomContextProcessRow>();
            rows.Add(new CustomContextProcessRow());

            var process = new CustomContextProcess();
            process.Execute(rows);

        }
    }
}
