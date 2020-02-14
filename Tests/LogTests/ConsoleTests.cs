using NBsoft.Logs;
using NBsoft.Logs.Interfaces;
using NUnit.Framework;
using System.IO;
using System;

namespace Tests
{
    public class ConsoleTests
    {
        ILogger logger;

        [SetUp]
        public void Setup()
        {
            logger = new ConsoleLogger();
        }

        #region Sync
        [Test]
        [Category("Sync")]
        [Category("Extension")]
        public void TestInfoExtension()
        {
            var now = DateTime.UtcNow;
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);
            logger.Info("Test Info", now);
            string result = sw.ToString();
            var shouldBe = $"{now.ToString("HH:mm:ss.fff")} | [INF] | Tests.ConsoleTests | TestInfoExtension | Test Info\r\n";
            Assert.AreEqual(shouldBe, result);
        }
        #endregion

        #region Async
        [Test]
        [Category("Async")]
        [Category("Extension")]
        public void TestAsyncDebugExtension()
        {
            var now = DateTime.UtcNow;
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);
            logger.DebugAsync("Debug test", now).Wait();
            string result = sw.ToString();
            var shouldBe = $"{now.ToString("HH:mm:ss.fff")} | [DEB] | Tests.ConsoleTests | TestAsyncDebugExtension | Debug test\r\n";
            Assert.AreEqual(shouldBe, result);
        }


        [Test]
        [Category("Async")]
        [Category("Extension")]
        public void TestAsyncInfoExtension()
        {
            var now = DateTime.UtcNow;
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);
            logger.InfoAsync("lala", now).Wait();
            string result = sw.ToString();
            var shouldBe = $"{now.ToString("HH:mm:ss.fff")} | [INF] | Tests.ConsoleTests | TestAsyncInfoExtension | lala\r\n";
            Assert.AreEqual(shouldBe, result);
        }

        [Test]
        [Category("Async")]
        [Category("Extension")]
        public void TestAsyncWarningExtension()
        {
            var now = DateTime.UtcNow;
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);
            logger.WarningAsync("Test Warning Async Extensions", now).Wait();
            string result = sw.ToString();
            var shouldBe = $"{now.ToString("HH:mm:ss.fff")} | [WAR] | Tests.ConsoleTests | TestAsyncWarningExtension | Test Warning Async Extensions\r\n";
            Assert.AreEqual(shouldBe, result);
        }

        [Test]
        [Category("Async")]
        [Category("Extension")]
        public void TestAsyncErrorExtension()
        {
            var now = DateTime.UtcNow;
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);
            logger.ErrorAsync("Test Error Async Extensions",new Exception("Test Exception"), now).Wait();
            string result = sw.ToString();
            var shouldBe = $"{now.ToString("HH:mm:ss.fff")} | [ERR] | Tests.ConsoleTests | TestAsyncErrorExtension | Test Error Async Extensions - Test Exception\r\n";
            Assert.AreEqual(shouldBe, result);
        }

        [Test]
        [Category("Async")]
        [Category("Extension")]
        public void TestAsyncFatalErrorExtension()
        {
            var now = DateTime.UtcNow;
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);
            logger.FatalErrorAsync("Test FatalError Async Extensions", new Exception("Test Exception"), now).Wait();
            string result = sw.ToString();
            var shouldBe = $"{now.ToString("HH:mm:ss.fff")} | [FAT] | Tests.ConsoleTests | TestAsyncFatalErrorExtension | Test FatalError Async Extensions - Test Exception\r\n";
            Assert.AreEqual(shouldBe, result);
        }
        #endregion
    }
}