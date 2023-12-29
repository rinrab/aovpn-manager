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
            Assert.AreEqual(true, Options.ReadFromArgs(new string[] { "/log", "console" }).IsConsole);
            Assert.AreEqual(false, Options.ReadFromArgs(new string[] { "/log", "test" }).IsConsole); // ?
            Assert.AreEqual(false, Options.ReadFromArgs(new string[] { "/log", "event" }).IsConsole);
            Assert.AreEqual(false, Options.ReadFromArgs(new string[] { "/test", "abc" }).IsConsole);
            Assert.AreEqual(false, Options.ReadFromArgs(new string[] { "/test", "console" }).IsConsole);
            Assert.AreEqual(false, Options.ReadFromArgs(Array.Empty<string>()).IsConsole);
            Assert.AreEqual(true, Options.ReadFromArgs(new string[] { "/TEST", "VAL", "/LOG", "CONSOLE" }).IsConsole);

            Assert.AreEqual(true, Options.ReadFromArgs(new string[] { "/console" }).IsConsole);
            Assert.AreEqual(true, Options.ReadFromArgs(new string[] { "/test", "/console" }).IsConsole);
            Assert.AreEqual(true, Options.ReadFromArgs(new string[] { "/log", "event", "/console" }).IsConsole); // ?
            Assert.AreEqual(true, Options.ReadFromArgs(new string[] { "/log", "event", "/console" }).IsConsole);
        }
    }
}
