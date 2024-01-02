using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AOVpnManager.Tests
{
    [TestClass]
    public class CommandLineArgumentsTest
    {
        [TestMethod]
        public void ParseTest()
        {
            Assert.AreEqual(true, CommandLineArguments.Parse("/log", "console").IsConsole);
            Assert.AreEqual(false, CommandLineArguments.Parse("/log", "test").IsConsole); // ?
            Assert.AreEqual(false, CommandLineArguments.Parse("/log", "event").IsConsole);
            Assert.AreEqual(false, CommandLineArguments.Parse("/test", "abc").IsConsole);
            Assert.AreEqual(false, CommandLineArguments.Parse("/test", "console").IsConsole);
            Assert.AreEqual(false, CommandLineArguments.Parse().IsConsole);
            Assert.AreEqual(true, CommandLineArguments.Parse("/TEST", "VAL", "/LOG", "CONSOLE").IsConsole);

            Assert.AreEqual(true, CommandLineArguments.Parse("/console").IsConsole);
            Assert.AreEqual(true, CommandLineArguments.Parse("/test", "/console").IsConsole);
            Assert.AreEqual(true, CommandLineArguments.Parse("/log", "event", "/console").IsConsole); // ?
        }
    }
}
