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
                stateManager.UpdateLastConnectionName("Contosa Vpn");
                Assert.AreEqual("Contosa Vpn", stateManager.ReadLastConnectionName());

                stateManager.UpdateLastConnectionName("Contosa Always On Vpn");
                Assert.AreEqual("Contosa Always On Vpn", stateManager.ReadLastConnectionName());

                stateManager.UpdateLastConnectionName(null);
                Assert.AreEqual(null, stateManager.ReadLastConnectionName());

                stateManager.UpdateLastConnectionName("Contosa Vpn v2");
                Assert.AreEqual("Contosa Vpn v2", stateManager.ReadLastConnectionName());
            }
            finally
            {
                stateManager.Clean();
            }
        }
    }
}
