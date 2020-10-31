using NBsoft.Logs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NBsoft.Logs
{
    public class FileLogger : ILogger
    {
        private static readonly SemaphoreSlim fileSemaphore = new SemaphoreSlim(1);

        private readonly string logPath;
        private readonly int maxQueueBuffer;
        Queue<ILogItem> logQueue;

        public FileLogger(string logPath, int maxQueueBuffer = 128)
        {
            this.logPath = logPath;
            this.maxQueueBuffer = maxQueueBuffer;
            logQueue = new Queue<ILogItem>();

            System.IO.DirectoryInfo logDir = new System.IO.DirectoryInfo(logPath);
            if (!logDir.Exists)
                logDir.Create();

        }

        private void WriteLog(LogType level, string component, string process, string context, string message, string stack, string type, DateTime? dateTime = default(DateTime?))
        {
            WriteLog(new LogItem()
            {
                Level = level,
                Component = component,
                Process = process,
                Context = context,
                Message = message,
                Stack = stack,
                Type = type,
                DateTime = dateTime ?? DateTime.UtcNow
            });
        }
        private Task WriteLogAsync(LogType level, string component, string process, string context, string message, string stack, string type, DateTime? dateTime = default(DateTime?))
        {
            return WriteLogAsync(new LogItem()
            {
                Level = level,
                Component = component,
                Process = process,
                Context = context,
                Message = message,
                Stack = stack,
                Type = type,
                DateTime = dateTime ?? DateTime.UtcNow
            });
        }

        public async Task WriteLogAsync(ILogItem item)
        {
            await Task.Run(() => WriteLog(item));
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
            ILogItem[] buffer = null;
            fileSemaphore.Wait();
            try
            {
                logQueue.Enqueue(item);
                if (logQueue.Count >= maxQueueBuffer)
                {
                    buffer = new ILogItem[logQueue.Count];
                    logQueue.CopyTo(buffer, 0);
                    logQueue.Clear();
                }
            }
            finally
            {
                fileSemaphore.Release();
            }
            if (buffer != null)
                WriteBufferToFile(buffer);
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

        private void WriteBufferToFile(ILogItem[] buffer)
        {

            var days = (from l in buffer
                        select l.DateTime.ToString("yyyyMMdd")).Distinct();

            foreach (var day in days)
            {
                string assetHistoryFile = System.IO.Path.Combine(logPath, $"{day}.log");
                if (!System.IO.File.Exists(assetHistoryFile))
                {
                    var historystream = System.IO.File.Create(assetHistoryFile);
                    historystream.Dispose();
                }
                var dayBuffer = from l in buffer
                                where l.DateTime.Date.ToString("yyyyMMdd") == day
                                orderby l.DateTime
                                select l;

                using (var filestream = new System.IO.FileStream(assetHistoryFile, System.IO.FileMode.Append, System.IO.FileAccess.Write))
                using (var textstream = new System.IO.StreamWriter(filestream))
                {

                    foreach (var item in dayBuffer)
                    {
                        string line = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                            item.DateTime.ToString("HH:mm:ss.fff"), item.Level.ToString().ToUpper().Substring(0, 3), item.Component, item.Process, item.Context, item.Message, item.Type);
                        textstream.WriteLine(line);
                        if (item.Stack != null)
                        {
                            textstream.WriteLine("------STACK-----");
                            textstream.WriteLine(item.Stack);
                            textstream.WriteLine("------EOSTACK-----");
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            if (logQueue.Count > 0)
            {
                WriteBufferToFile(logQueue.ToArray());
            }
        }
                
        
    }
}
