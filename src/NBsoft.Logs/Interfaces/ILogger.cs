using System;
using System.Threading.Tasks;

namespace NBsoft.Logs.Interfaces
{
    public interface ILogger : IDisposable
    {
        Task WriteLogAsync(ILogItem item);
        //Task WriteLogAsync(LogType level, string component, string process, string context, string message, string stack, string type, DateTime? dateTime = default(DateTime?));
        Task WriteInfoAsync(string component, string process, string context, string message, DateTime? dateTime = default(DateTime?));
        Task WriteWarningAsync(string component, string process, string context, string message, DateTime? dateTime = default(DateTime?));
        Task WriteErrorAsync(string component, string process, string context, Exception exception, DateTime? dateTime = default(DateTime?));
        Task WriteFatalErrorAsync(string component, string process, string context, Exception exception, DateTime? dateTime = default(DateTime?));
    }
}
