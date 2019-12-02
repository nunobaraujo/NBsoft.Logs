using NBsoft.Logs.Interfaces;
using NBsoft.Logs.Extensions;
using System;
using System.IO;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TestInfo();
            TestAsyncConsole();
            TestAsyncError();
        }

        private static void TestInfo()
        {
            var logger = new NBsoft.Logs.ConsoleLogger();
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);

            var now = DateTime.UtcNow;

            logger.Info("lala", now);

            string result = sw.ToString();
            var shouldBe = $"{now.ToString("HH:mm:ss.fff")} | [INF] | TestConsole.Program | TestInfo | lala\r\n";
            if (result != shouldBe)
                throw new ApplicationException();
        }


        private static void TestAsyncConsole()
        {
            var logger = new NBsoft.Logs.ConsoleLogger();
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);

            var now = DateTime.UtcNow;

            logger.InfoAsync("lala", now).Wait();

            string result = sw.ToString();
            var shouldBe = $"{now.ToString("HH:mm:ss.fff")} | [INF] | TestConsole.Program | TestAsyncConsole | lala\r\n";
            if (result != shouldBe)
                throw new ApplicationException();
        }


        private static void TestAsyncError()
        {
            var logger = new NBsoft.Logs.ConsoleLogger();
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);

            var now = DateTime.UtcNow;

            logger.ErrorAsync("lala",new DriveNotFoundException("S1",new ApplicationException("S2")), now).Wait();

            string result = sw.ToString();
            var shouldBe = $"{now.ToString("HH:mm:ss.fff")} | [ERR] | TestConsole.Program | TestAsyncError | lala - S1\r\n";
            if (result != shouldBe)
                throw new ApplicationException();
        }
    }
}
