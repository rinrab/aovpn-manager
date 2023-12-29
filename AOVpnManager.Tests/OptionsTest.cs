using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AOVpnManager.Tests
{
    [TestClass]
    public class OptionsTest
    {
        [TestMethod]
        public void ReadFromArgsTest()
        {
            Assert.AreEqual(true, Options.ReadFromArgs(new string[] { "/console" }).IsConsole);
            Assert.AreEqual(false, Options.ReadFromArgs(new string[] { "/test" }).IsConsole);
            Assert.AreEqual(false, Options.ReadFromArgs(Array.Empty<string>()).IsConsole);
            Assert.AreEqual(true, Options.ReadFromArgs(new string[] { "/test", "/Console" }).IsConsole);
        }
    }
}
