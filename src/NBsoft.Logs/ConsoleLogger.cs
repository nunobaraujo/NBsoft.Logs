using NBsoft.Logs.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NBsoft.Logs
{
    public class ConsoleLogger : ILogger
    {   
        public readonly ConsoleLoggerOptions Options;

        public ConsoleLogger()            
            :this (new ConsoleLoggerOptions
            {
                DateFormat = "HH:mm:ss.fff",
                ShowComponent = true,
                ShowContext = true,
                ShowProcess = true,
                ShowType = false
            })
        {            
        }
        public ConsoleLogger(ConsoleLoggerOptions options)
        {
            this.Options = options;
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

            var format = "{0} | [{1}]";
            var parameters = new List<string>
            {
                dateTime.Value.ToString(Options.DateFormat),
                level.ToString().ToUpper().Substring(0, 3)
            };
            int i = 2;
            if (Options.ShowComponent && !string.IsNullOrEmpty(component))
            {
                format += $" | {{{i++}}}";
                parameters.Add(component);
            }
            if (Options.ShowProcess && !string.IsNullOrEmpty(process))
            {
                format += $" | {{{i++}}}";
                parameters.Add(process);
            }
            if (Options.ShowContext && !string.IsNullOrEmpty(context))
            {
                format += $" | {{{i++}}}";
                parameters.Add(context);
            }
            if (Options.ShowType && !string.IsNullOrEmpty(type))
            {
                format += $" | {{{i++}}}";
                parameters.Add(type);
            }
            format += $" | {{{i++}}}";
            parameters.Add(message);


            Console.WriteLine(format, parameters.ToArray());
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
        public Task WriteErrorAsync(string component, string process, string context, string message, Exception exception, DateTime? dateTime = default(DateTime?))
        {
            return WriteLogAsync(LogType.Error, component, process, context, $"{message} - {exception.Message}", exception.GetBaseException().StackTrace, exception.GetBaseException().GetType().ToString(), dateTime);
        }
        public Task WriteFatalErrorAsync(string component, string process, string context, string message, Exception exception, DateTime? dateTime = default(DateTime?))
        {
            return WriteLogAsync(LogType.FatalError, component, process, context, $"{message} - {exception.Message}", exception.GetBaseException().StackTrace, exception.GetBaseException().GetType().ToString(), dateTime);
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
        public void WriteError(string component, string process, string context, string message, Exception exception, DateTime? dateTime = null)
        {
            WriteLog(LogType.Error, component, process, context, $"{message} - {exception.Message}", exception.GetBaseException().StackTrace, exception.GetBaseException().GetType().ToString(), dateTime);
        }
        public void WriteFatalError(string component, string process, string context, string message, Exception exception, DateTime? dateTime = null)
        {
            WriteLog(LogType.FatalError, component, process, context, $"{message} - {exception.Message}", exception.GetBaseException().StackTrace, exception.GetBaseException().GetType().ToString(), dateTime);
        }

        public void Dispose()
        {
        }
    }
}
