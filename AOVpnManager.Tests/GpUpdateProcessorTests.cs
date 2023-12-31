﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Rhino.Mocks;

namespace AOVpnManager.Tests
{
    [TestClass]
    public class GpUpdateProcessorTests
    {
        [TestMethod]
        public void Test()
        {
            MockRepository mocks = new MockRepository();

            IVpnManager vpnManager = mocks.StrictMock<IVpnManager>();
            IGroupPolicyProvider policyProvider = mocks.StrictMock<IGroupPolicyProvider>();
            IStateManager stateManager = new StateManager(Registry.CurrentUser, @"Test\AOVpnManager");
            stateManager.SetLastConnectionName(null);
            ILogger logger = mocks.StrictMock<ILogger>();

            // Create Vpn Connection
            {
                vpnManager.Expect(x => x.CreateVpnConnection("Name 1", "Profile 1"));
                policyProvider.Expect(x => x.ReadSettings()).Return(new GroupPolicySettings("Profile 1", "Name 1"));
                logger.Expect(x => x.Trace(null)).Repeat.Any().IgnoreArguments();
                logger.Expect(x => x.VpnConnectionCreated("Name 1"));

                mocks.ReplayAll();

                GpUpdateProcessor processor = new GpUpdateProcessor(vpnManager, policyProvider, stateManager, logger);
                processor.Run();

                mocks.VerifyAll();
            }

            // Update Vpn Profile
            {
                mocks.BackToRecordAll();

                vpnManager.Expect(x => x.DeleteVpnConnection("Name 1"));
                vpnManager.Expect(x => x.CreateVpnConnection("Name 1", "Profile 2"));
                policyProvider.Expect(x => x.ReadSettings()).Return(new GroupPolicySettings("Profile 2", "Name 1"));
                logger.Expect(x => x.Trace(null)).Repeat.Any().IgnoreArguments();
                logger.Expect(x => x.VpnConnectionDeleted("Name 1"));
                logger.Expect(x => x.VpnConnectionCreated("Name 1"));

                mocks.ReplayAll();

                GpUpdateProcessor processor = new GpUpdateProcessor(vpnManager, policyProvider, stateManager, logger);
                processor.Run();

                mocks.VerifyAll();
            }

            // Rename Vpn Connection
            {
                mocks.BackToRecordAll();

                vpnManager.Expect(x => x.DeleteVpnConnection("Name 1"));
                vpnManager.Expect(x => x.CreateVpnConnection("Name 2", "Profile 2"));
                policyProvider.Expect(x => x.ReadSettings()).Return(new GroupPolicySettings("Profile 2", "Name 2"));
                logger.Expect(x => x.Trace(null)).Repeat.Any().IgnoreArguments();
                logger.Expect(x => x.VpnConnectionDeleted("Name 1"));
                logger.Expect(x => x.VpnConnectionCreated("Name 2"));

                mocks.ReplayAll();

                GpUpdateProcessor processor = new GpUpdateProcessor(vpnManager, policyProvider, stateManager, logger);
                processor.Run();

                mocks.VerifyAll();
            }

            // Nothing changed.
            {
                mocks.BackToRecordAll();

                policyProvider.Expect(x => x.ReadSettings()).Return(new GroupPolicySettings("Profile 2", "Name 2"));
                logger.Expect(x => x.Trace(null)).Repeat.Any().IgnoreArguments();

                mocks.ReplayAll();

                GpUpdateProcessor processor = new GpUpdateProcessor(vpnManager, policyProvider, stateManager, logger);
                processor.Run();

                mocks.VerifyAll();
            }
        }
    }
}
