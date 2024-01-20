using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace AOVpnManager.Tests
{
    [TestClass]
    public class RasApiTests
    {
        [TestMethod]
        public void HangupAllTest()
        {
            if (!Debugger.IsAttached)
            {
                return;
            }

            RasApi.DisconnectAll((x) =>
            {
                Console.WriteLine(x);
                return true;
            });
        }
    }
}
