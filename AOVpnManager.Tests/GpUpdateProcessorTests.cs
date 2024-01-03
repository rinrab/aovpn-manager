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
            IVpnManager vpnManager = MockRepository.GenerateStub<IVpnManager>();
            vpnManager.Stub(x => x.EnumarateVpnConnections()).Return(new VpnConnectionInfo[] { });

            var policyProvider = MockRepository.GenerateStub<IGroupPolicyProvider>();
            policyProvider.Stub(x => x.ReadSettings()).Return(new GroupPolicySettings("Profile 1", "Name 1"));

            var stateManager = MockRepository.GenerateStub<IStateManager>();
            stateManager.Stub(x => x.GetLastConnectionName()).Return(null);

            var logger = MockRepository.GenerateStub<ILogger>();

            GpUpdateProcessor processor = new GpUpdateProcessor(vpnManager, policyProvider, stateManager, logger);
            processor.Run();

            vpnManager.AssertWasCalled(x => x.CreateVpnConnection("Name 1", "Profile 1"));
            vpnManager.AssertWasNotCalled(x => x.UpdateVpnConnection("Name 1", "Profile 1"));
            vpnManager.AssertWasNotCalled(x => x.DeleteVpnConnection("Name 1"));
            stateManager.AssertWasCalled(x => x.SetLastConnectionName("Name 1"));
        }

        [TestMethod]
        public void UpdateVpnProfile()
        {
            IVpnManager vpnManager = MockRepository.GenerateStub<IVpnManager>();
            vpnManager.Stub(x => x.EnumarateVpnConnections()).Return(new VpnConnectionInfo[] { new VpnConnectionInfo("Name 1", "Profile 1") });

            var policyProvider = MockRepository.GenerateStub<IGroupPolicyProvider>();
            policyProvider.Stub(x => x.ReadSettings()).Return(new GroupPolicySettings("Profile 2", "Name 1"));

            var stateManager = MockRepository.GenerateStub<IStateManager>();
            stateManager.Stub(x => x.GetLastConnectionName()).Return("Name 1");

            var logger = MockRepository.GenerateStub<ILogger>();

            GpUpdateProcessor processor = new GpUpdateProcessor(vpnManager, policyProvider, stateManager, logger);
            processor.Run();

            vpnManager.AssertWasNotCalled(x => x.CreateVpnConnection("Name 1", "Profile 2"));
            vpnManager.AssertWasCalled(x => x.UpdateVpnConnection("Name 1", "Profile 2"));
            vpnManager.AssertWasNotCalled(x => x.DeleteVpnConnection("Name 1"));
            stateManager.AssertWasCalled(x => x.SetLastConnectionName("Name 1"));
        }
    }
}
