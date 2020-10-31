using System;
using System.Threading.Tasks;

namespace NBsoft.Logs
{
    public interface ILogger : IDisposable
    {
        Task WriteLogAsync(ILogItem item);        
        Task WriteInfoAsync(string component, string process, string context, string message, DateTime? dateTime = default(DateTime?));
        Task WriteWarningAsync(string component, string process, string context, string message, DateTime? dateTime = default(DateTime?));
        Task WriteErrorAsync(string component, string process, string context, string message, Exception exception, DateTime? dateTime = default(DateTime?));
        Task WriteFatalErrorAsync(string component, string process, string context, string message, Exception exception, DateTime? dateTime = default(DateTime?));

        void WriteLog(ILogItem item);
        void WriteInfo(string component, string process, string context, string message, DateTime? dateTime = default(DateTime?));
        void WriteWarning(string component, string process, string context, string message, DateTime? dateTime = default(DateTime?));
        void WriteError(string component, string process, string context, string message, Exception exception, DateTime? dateTime = default(DateTime?));
        void WriteFatalError(string component, string process, string context, string message, Exception exception, DateTime? dateTime = default(DateTime?));
    }
}
