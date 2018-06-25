using NBsoft.Logs.Interfaces;
using System;
using System.Threading.Tasks;

namespace NBsoft.Logs
{
    public class ConsoleLogger : ILogger
    {
        private const string DefaultFormat = "{0} -- {1} - {2}|{3}|{4}|{5}|{6}";
        private readonly string _logFormat;

        public ConsoleLogger()            
            :this (DefaultFormat)
        {            
        }
        public ConsoleLogger(string logFormat)
        {
            _logFormat = logFormat;
            string logTest = string.Format(_logFormat, "1", "2", "3", "4", "5", "6", "7");
        }

        private Task WriteLogAsync(LogType level, string component, string process, string context, string message, string stack, string type, DateTime? dateTime = default(DateTime?))
        {
            WriteLog(level, component, process, context, message, stack, type, dateTime);
            return Task.FromResult(0);
        }
        private void WriteLog(LogType level, string component, string process, string context, string message, string stack, string type, DateTime? dateTime = default(DateTime?))
        {
            
            if (dateTime == null)
                dateTime = DateTime.UtcNow;
            Console.WriteLine(_logFormat,
                dateTime.Value.ToString("HH:mm:ss.fff"),
                level.ToString().ToUpper().Substring(0, 3),
                component,
                process,
                context,
                type,
                message);
            if (stack != null)
            {
                Console.WriteLine("-------STACK------");
                Console.WriteLine(stack);
                Console.WriteLine("-------EOS------");
            }
        }

        public Task WriteLogAsync(ILogItem item)
        {
            return WriteLogAsync(item.Level, item.Component, item.Process, item.Context, item.Message, item.Stack, item.Type, item.DateTime);
        }        
        public Task WriteInfoAsync(string component, string process, string context, string message, DateTime? dateTime = default(DateTime?))
        {
            return WriteLogAsync(LogType.Info, component, process, context, message, null, null, dateTime);
        }
        public Task WriteWarningAsync(string component, string process, string context, string message, DateTime? dateTime = default(DateTime?))
        {
            return WriteLogAsync(LogType.Warning, component, process, context, message, null, null, dateTime);
        }
        public Task WriteErrorAsync(string component, string process, string context, Exception exception, DateTime? dateTime = default(DateTime?))
        {
            return WriteLogAsync(LogType.Error, component, process, context, exception.Message, exception.GetBaseException().StackTrace, exception.GetBaseException().GetType().ToString(), dateTime);
        }
        public Task WriteFatalErrorAsync(string component, string process, string context, Exception exception, DateTime? dateTime = default(DateTime?))
        {
            return WriteLogAsync(LogType.FatalError, component, process, context, exception.Message, exception.GetBaseException().StackTrace, exception.GetBaseException().GetType().ToString(), dateTime);
        }
                
        public void WriteLog(ILogItem item)
        {
            WriteLog(item.Level, item.Component, item.Process, item.Context, item.Message, item.Stack, item.Type, item.DateTime);
        }
        public void WriteInfo(string component, string process, string context, string message, DateTime? dateTime = null)
        {
            WriteLog(LogType.Info, component, process, context, message, null, null, dateTime);
        }
        public void WriteWarning(string component, string process, string context, string message, DateTime? dateTime = null)
        {
            WriteLog(LogType.Warning, component, process, context, message, null, null, dateTime);
        }
        public void WriteError(string component, string process, string context, Exception exception, DateTime? dateTime = null)
        {
            WriteLog(LogType.Error, component, process, context, exception.Message, exception.GetBaseException().StackTrace, exception.GetBaseException().GetType().ToString(), dateTime);
        }
        public void WriteFatalError(string component, string process, string context, Exception exception, DateTime? dateTime = null)
        {
            WriteLog(LogType.FatalError, component, process, context, exception.Message, exception.GetBaseException().StackTrace, exception.GetBaseException().GetType().ToString(), dateTime);
        }

        public void Dispose()
        {
        }
    }
}
