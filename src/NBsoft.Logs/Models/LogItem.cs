using NBsoft.Logs.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBsoft.Logs.Models
{
    public class LogItem : ILogItem
    {
        public DateTime DateTime { get; set; }
        public LogType Level { get; set; }
        public string Component { get; set; }
        public string Process { get; set; }
        public string Context { get; set; }
        public string Type { get; set; }
        public string Stack { get; set; }
        public string Message { get; set; }
    }
}
