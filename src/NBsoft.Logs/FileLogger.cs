using NBsoft.Logs.Interfaces;
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

        public async Task WriteLogAsync(ILogItem item)
        {
            ILogItem[] buffer = null;
            await fileSemaphore.WaitAsync();
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
                await WriteBufferToFile(buffer);
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

        private async Task WriteBufferToFile(ILogItem[] buffer)
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
                        await textstream.WriteLineAsync(line);
                        if (item.Stack != null)
                        {
                            await textstream.WriteLineAsync("------STACK-----");
                            await textstream.WriteLineAsync(item.Stack);
                            await textstream.WriteLineAsync("------EOSTACK-----");
                        }
                    }
                }
            }
        }

        public async void Dispose()
        {
            if (logQueue.Count > 0)
            {
                await WriteBufferToFile(logQueue.ToArray());
            }
        }
    }
}
