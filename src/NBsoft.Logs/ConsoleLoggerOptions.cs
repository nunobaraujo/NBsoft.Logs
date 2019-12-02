using System;
using System.Collections.Generic;
using System.Text;

namespace NBsoft.Logs
{
    public class ConsoleLoggerOptions
    {
        public string DateFormat { get; set; }
        public bool ShowComponent { get; set; }
        public bool ShowProcess { get; set; }
        public bool ShowContext { get; set; }
        public bool ShowType { get; set; }

    }
}
