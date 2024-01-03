using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace AOVpnManager.Tests
{
    [TestClass]
    public class GpUpdateProcessorTests
    {
        [TestMethod]
        public void CreateVpnConnectionFromEmptyState()
        {
            MockRepository mocks = new MockRepository();

            IVpnManager vpnManager = mocks.StrictMock<IVpnManager>();
            IGroupPolicyProvider policyProvider = mocks.StrictMock<IGroupPolicyProvider>();
            IStateManager stateManager = mocks.StrictMock<IStateManager>();
            ILogger logger = mocks.StrictMock<ILogger>();

            vpnManager.Expect(x => x.EnumarateVpnConnections()).Return(new VpnConnectionInfo[] { });
            vpnManager.Expect(x => x.CreateVpnConnection("Name 1", "Profile 1"));

            policyProvider.Expect(x => x.ReadSettings()).Return(new GroupPolicySettings("Profile 1", "Name 1"));

            stateManager.Expect(x => x.GetLastConnectionName()).Return(null);
            stateManager.Expect(x => x.SetLastConnectionName("Name 1"));

            logger.Expect(x => x.Trace(null)).Repeat.Any().IgnoreArguments();
            logger.Expect(x => x.VpnConnectionCreated("Name 1"));

            mocks.ReplayAll();

            GpUpdateProcessor processor = new GpUpdateProcessor(vpnManager, policyProvider, stateManager, logger);
            processor.Run();

            mocks.VerifyAll();
        }

        [TestMethod]
        public void UpdateVpnProfile()
        {
            MockRepository mocks = new MockRepository();

            IVpnManager vpnManager = mocks.StrictMock<IVpnManager>();
            IGroupPolicyProvider policyProvider = mocks.StrictMock<IGroupPolicyProvider>();
            IStateManager stateManager = mocks.StrictMock<IStateManager>();
            ILogger logger = mocks.StrictMock<ILogger>();

            vpnManager.Expect(x => x.EnumarateVpnConnections()).Return(new VpnConnectionInfo[] { new VpnConnectionInfo("Name 1", "Profile 1") });
            vpnManager.Expect(x => x.UpdateVpnConnection("Name 1", "Profile 2"));

            policyProvider.Expect(x => x.ReadSettings()).Return(new GroupPolicySettings("Profile 2", "Name 1"));

            stateManager.Expect(x => x.GetLastConnectionName()).Return("Name 1");
            stateManager.Expect(x => x.SetLastConnectionName("Name 1"));

            logger.Expect(x => x.Trace(null)).Repeat.Any().IgnoreArguments();
            logger.Expect(x => x.VpnConnectionUpdated("Name 1"));

            mocks.ReplayAll();

            GpUpdateProcessor processor = new GpUpdateProcessor(vpnManager, policyProvider, stateManager, logger);
            processor.Run();

            mocks.VerifyAll();
        }
    }
}
