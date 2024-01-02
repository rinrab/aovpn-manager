using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;

namespace AOVpnManager.Tests
{
    [TestClass]
    public class StateManagerTests
    {
        [TestMethod]
        public void LastConnectionName()
        {
            IStateManager stateManager = new StateManager(Registry.CurrentUser, @"Test\AOVpnManager");

            try
            {
                stateManager.SetLastConnectionName("Contosa Vpn");
                Assert.AreEqual("Contosa Vpn", stateManager.GetLastConnectionName());

                stateManager.SetLastConnectionName("Contosa Always On Vpn");
                Assert.AreEqual("Contosa Always On Vpn", stateManager.GetLastConnectionName());

                stateManager.SetLastConnectionName(null);
                Assert.AreEqual(null, stateManager.GetLastConnectionName());

                stateManager.SetLastConnectionName("Contosa Vpn v2");
                Assert.AreEqual("Contosa Vpn v2", stateManager.GetLastConnectionName());
            }
            finally
            {
                Registry.CurrentUser.DeleteSubKeyTree("Test");
            }
        }
    }
}
