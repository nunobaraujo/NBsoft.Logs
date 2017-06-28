using System;

namespace NBsoft.Logs.Interfaces
{
    public interface ILogItem
    {
        DateTime DateTime { get; }
        LogType Level { get; }
        string Component { get; }
        string Process { get; }
        string Context { get; }
        string Type { get; }
        string Stack { get; }
        string Message { get; }
    }
}
