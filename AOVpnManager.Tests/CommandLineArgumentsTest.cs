using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AOVpnManager.Tests
{
    [TestClass]
    public class CommandLineArgumentsTest
    {
        [TestMethod]
        public void ReadFromArgsTest()
        {
            Assert.AreEqual(true, CommandLineArguments.Read("/log", "console").IsConsole);
            Assert.AreEqual(false, CommandLineArguments.Read("/log", "test").IsConsole); // ?
            Assert.AreEqual(false, CommandLineArguments.Read("/log", "event").IsConsole);
            Assert.AreEqual(false, CommandLineArguments.Read("/test", "abc").IsConsole);
            Assert.AreEqual(false, CommandLineArguments.Read("/test", "console").IsConsole);
            Assert.AreEqual(false, CommandLineArguments.Read().IsConsole);
            Assert.AreEqual(true, CommandLineArguments.Read("/TEST", "VAL", "/LOG", "CONSOLE").IsConsole);

            Assert.AreEqual(true, CommandLineArguments.Read("/console").IsConsole);
            Assert.AreEqual(true, CommandLineArguments.Read("/test", "/console").IsConsole);
            Assert.AreEqual(true, CommandLineArguments.Read("/log", "/console").IsConsole);
            Assert.AreEqual(true, CommandLineArguments.Read("/log", "event", "/console").IsConsole); // ?
        }
    }
}
