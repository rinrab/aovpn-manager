using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AOVpnManager.Tests
{
    [TestClass]
    public class CommandLineOptionsTest
    {
        [TestMethod]
        public void ParseTest()
        {
            Assert.AreEqual(true, CommandLineOptions.Parse("/log", "console").IsConsole);
            Assert.AreEqual(false, CommandLineOptions.Parse("/log", "test").IsConsole); // ?
            Assert.AreEqual(false, CommandLineOptions.Parse("/log", "event").IsConsole);
            Assert.AreEqual(false, CommandLineOptions.Parse("/test", "abc").IsConsole);
            Assert.AreEqual(false, CommandLineOptions.Parse("/test", "console").IsConsole);
            Assert.AreEqual(false, CommandLineOptions.Parse().IsConsole);
            Assert.AreEqual(true, CommandLineOptions.Parse("/TEST", "VAL", "/LOG", "CONSOLE").IsConsole);

            Assert.AreEqual(true, CommandLineOptions.Parse("/console").IsConsole);
            Assert.AreEqual(true, CommandLineOptions.Parse("/test", "/console").IsConsole);
            Assert.AreEqual(true, CommandLineOptions.Parse("/log", "event", "/console").IsConsole); // ?
        }
    }
}
