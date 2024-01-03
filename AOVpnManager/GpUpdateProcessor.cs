namespace AOVpnManager
{
    public class GpUpdateProcessor
    {
        private readonly IVpnManager vpnManager;
        private readonly IGroupPolicyProvider policyProvider;
        private readonly IStateManager stateManager;
        private readonly ILogger logger;

        public GpUpdateProcessor(IVpnManager vpnManager, IGroupPolicyProvider policyProvider, IStateManager stateManager, ILogger logger)
        {
            this.vpnManager = vpnManager;
            this.policyProvider = policyProvider;
            this.stateManager = stateManager;
            this.logger = logger;
        }

        public void Run()
        {
            GroupPolicySettings settings = policyProvider.ReadSettings();
            string lastConnectionName = stateManager.GetLastConnectionName();

            using (IVpnManager vpnManager = VpnManager.Create())
            {
                if (string.IsNullOrEmpty(settings.VpnProfileXml))
                {
                    if (lastConnectionName != null)
                    {
                        try
                        {
                            vpnManager.DeleteVpnConnection(lastConnectionName);
                            logger.VpnConnectionDeleted(lastConnectionName);
                        }
                        catch (VpnConnectionNotFoundException)
                        {
                        }

                        stateManager.SetLastConnectionName(null);
                    }
                }
                else
                {
                    if (lastConnectionName != null && lastConnectionName != settings.VpnConnectionName)
                    {
                        try
                        {
                            vpnManager.DeleteVpnConnection(lastConnectionName);
                            logger.VpnConnectionDeleted(lastConnectionName);
                        }
                        catch (VpnConnectionNotFoundException)
                        {
                        }

                        stateManager.SetLastConnectionName(null);
                    }

                    VpnConnectionInfo oldConnection = FindVpnConnection(settings.VpnConnectionName);

                    logger.Trace(string.Format("oldConnection: {0}", oldConnection));

                    if (oldConnection == null)
                    {
                        vpnManager.CreateVpnConnection(settings.VpnConnectionName, settings.VpnProfileXml);
                        logger.VpnConnectionCreated(settings.VpnConnectionName);
                    }
                    else
                    {
                        vpnManager.UpdateVpnConnection(settings.VpnConnectionName, settings.VpnProfileXml);
                        logger.VpnConnectionUpdated(settings.VpnConnectionName);
                    }

                    stateManager.SetLastConnectionName(settings.VpnConnectionName);
                }
            }
        }

        private VpnConnectionInfo FindVpnConnection(string connectionName)
        {
            foreach (VpnConnectionInfo connection in vpnManager.EnumarateVpnConnections())
            {
                if (connection.ConnectionName == connectionName)
                {
                    return connection;
                }
            }
            return null;
        }
    }
}
