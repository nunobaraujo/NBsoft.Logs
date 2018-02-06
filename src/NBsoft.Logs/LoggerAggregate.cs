using NBsoft.Logs.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NBsoft.Logs
{
    public class LoggerAggregate : ILogger
    {
        private readonly List<ILogger> loggers;
        public LoggerAggregate()
        {
            loggers = new List<ILogger>();
        }

        public bool AddLogger(ILogger logger)
        {
            if (logger == null)
                return false;
            loggers.Add(logger);
            return true;
        }
        public bool RemoveLogger(ILogger logger)
        {
            if (loggers.Contains(logger))
            {
                loggers.Remove(logger);
                return true;
            }
            else
                return false;
        }
             
        public async Task WriteLogAsync(ILogItem item)
        {
            foreach (var logger in loggers)
            {
                await logger.WriteLogAsync(item);
            }
        }
       
        public async Task WriteInfoAsync(string component, string process, string context, string message, DateTime? dateTime = default(DateTime?))
        {
            foreach (var logger in loggers)
            {
                await logger.WriteInfoAsync(component,  process, context, message, dateTime);
            }            
        }
        
        public async Task WriteWarningAsync(string component, string process, string context, string message, DateTime? dateTime = default(DateTime?))
        {
            foreach (var logger in loggers)
            {
                await logger.WriteWarningAsync(component, process, context, message, dateTime);
            }
        }
        public async Task WriteErrorAsync(string component, string process, string context, Exception exception, DateTime? dateTime = default(DateTime?))
        {
            foreach (var logger in loggers)
            {
                await logger.WriteErrorAsync(component, process, context, exception, dateTime);
            }
        }
        public async Task WriteFatalErrorAsync(string component, string process, string context, Exception exception, DateTime? dateTime = default(DateTime?))
        {
            foreach (var logger in loggers)
            {
                await logger.WriteFatalErrorAsync(component, process, context, exception, dateTime);
            }
        }

        public void Dispose()
        {
            foreach (var logger in loggers)
            {
                logger.Dispose();
            }
        }
    }
}
