using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AOVpnManager.Tests
{
    [TestClass]
    public class OptionsTest
    {
        [TestMethod]
        public void ReadFromArgsTest()
        {
            Assert.AreEqual(true, Options.ReadFromArgs("/log", "console").IsConsole);
            Assert.AreEqual(false, Options.ReadFromArgs("/log", "test").IsConsole); // ?
            Assert.AreEqual(false, Options.ReadFromArgs("/log", "event").IsConsole);
            Assert.AreEqual(false, Options.ReadFromArgs("/test", "abc").IsConsole);
            Assert.AreEqual(false, Options.ReadFromArgs("/test", "console").IsConsole);
            Assert.AreEqual(false, Options.ReadFromArgs().IsConsole);
            Assert.AreEqual(true, Options.ReadFromArgs("/TEST", "VAL", "/LOG", "CONSOLE").IsConsole);

            Assert.AreEqual(true, Options.ReadFromArgs("/console").IsConsole);
            Assert.AreEqual(true, Options.ReadFromArgs("/test", "/console").IsConsole);
            Assert.AreEqual(true, Options.ReadFromArgs("/log", "/console").IsConsole);
            Assert.AreEqual(true, Options.ReadFromArgs("/log", "event", "/console").IsConsole); // ?
        }
    }
}
