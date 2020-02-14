using NBsoft.Logs.Interfaces;
using NBsoft.Logs.Models;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NBsoft.Logs
{
    public static class LogExtensions
    {
        public async static Task DebugAsync(this ILogger logger, string message, DateTime? time = null, string context = null, [CallerMemberName] string callerName = "")
        {
            await logger.WriteLogAsync(new LogItem {
                DateTime = time ?? DateTime.Now,
                Level = LogType.Debug,
                Component = GetAsyncComponent(),
                Process = callerName,
                Message = message,
                Context = context
            });
        }

        public async static Task InfoAsync(this ILogger logger, string message, DateTime? time = null, string context = null, [CallerMemberName] string callerName = "")
        {   
            await logger.WriteInfoAsync(GetAsyncComponent(), callerName, context, message, time);
        }

        public async static Task WarningAsync(this ILogger logger, string message, DateTime? time = null, string context = null, [CallerMemberName] string callerName = "")
        {   
            await logger.WriteWarningAsync(GetAsyncComponent(), callerName, context, message, time);
        }

        public async static Task ErrorAsync(this ILogger logger, string message, Exception ex, DateTime? time = null, string context = null, [CallerMemberName] string callerName = "")
        {
            await logger.WriteErrorAsync(GetAsyncComponent(), callerName, context, message, ex, time);
        }

        public async static Task FatalErrorAsync(this ILogger logger, string message, Exception ex, DateTime? time = null, string context = null, [CallerMemberName] string callerName = "")
        {
            await logger.WriteFatalErrorAsync(GetAsyncComponent(), callerName, context, message, ex, time);
        }



        public static void Debug(this ILogger logger, string message, DateTime? time = null, string context = null, [CallerMemberName] string callerName = "")
        {
            logger.WriteLog(new LogItem
            {
                DateTime = time ?? DateTime.Now,
                Level = LogType.Debug,
                Component = GetComponent(),
                Process = callerName,
                Message = message,
                Context = context
            });
        }

        public static void Info(this ILogger logger, string message, DateTime? time = null, string context = null, [CallerMemberName] string callerName = "")
        {
            logger.WriteInfo(GetComponent(), callerName, context, message, time);
        }

        public static void Warning(this ILogger logger, string message, DateTime? time = null, string context = null, [CallerMemberName] string callerName = "")
        {
            logger.WriteWarning(GetComponent(), callerName, context, message, time);
        }

        public static void Error(this ILogger logger, string message, Exception ex, DateTime? time = null, string context = null, [CallerMemberName] string callerName = "")
        {
            logger.WriteError(GetComponent(), callerName, context, message, ex, time);
        }

        public static void FatalError(this ILogger logger, string message, Exception ex, DateTime? time = null, string context = null, [CallerMemberName] string callerName = "")
        {
            logger.WriteFatalError(GetComponent(), callerName, context, message, ex, time);
        }

        private static string GetAsyncComponent()
        {
            var stack = new StackTrace();
            // GetComponent is the stack line 0
            // Local caller [Info()] is stack line 1
            // Async calls use stack lines 2 and 3
            // External caller is stack line 4

            var frame = stack?.GetFrame(4);
            return frame.GetMethod().ReflectedType.ToString();
        }
        private static string GetComponent()
        {
            var stack = new StackTrace();
            // GetComponent is the stack line 0
            // Local caller [Info()] is stack line 1            
            // External caller is stack line 2

            var frame = stack?.GetFrame(2);
            return frame.GetMethod().ReflectedType.ToString();
        }
    }
}
