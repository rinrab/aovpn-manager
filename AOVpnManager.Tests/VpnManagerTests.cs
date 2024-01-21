using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace AOVpnManager.Tests
{
    [TestClass]
    public class VpnManagerTests
    {
        // This test should be run with administrator privileges
        [TestMethod]
        public void DeleteVpnConnectionTest()
        {
            if (!Debugger.IsAttached)
            {
                return;
            }

            IVpnManager vpnManager = VpnManager.Create();

            vpnManager.DeleteVpnConnection("Test VPN");
        }
    }
}
