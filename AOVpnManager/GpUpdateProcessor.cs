using System.Security.Cryptography;
using System.Text;

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
                string lastVpnProfile = stateManager.GetLastVpnProfile();
                string currentVpnProfile = ComputeHash(settings.VpnProfileXml);

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

                stateManager.SetLastVpnProfile(currentVpnProfile);

                if (oldConnection == null)
                {
                    stateManager.SetLastConnectionName(settings.VpnConnectionName);
                    vpnManager.CreateVpnConnection(settings.VpnConnectionName, settings.VpnProfileXml);
                    logger.VpnConnectionCreated(settings.VpnConnectionName);
                }
                else if (lastVpnProfile != currentVpnProfile)
                {
                    stateManager.SetLastConnectionName(null);
                    vpnManager.DeleteVpnConnection(lastConnectionName);
                    logger.VpnConnectionDeleted(lastConnectionName);

                    stateManager.SetLastConnectionName(settings.VpnConnectionName);
                    vpnManager.CreateVpnConnection(settings.VpnConnectionName, settings.VpnProfileXml);
                    logger.VpnConnectionCreated(settings.VpnConnectionName);
                }
            }
        }

        private string ComputeHash(string vpnProfileXml)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(vpnProfileXml));

                StringBuilder rv = new StringBuilder(hash.Length * 2);
                string symbols = "0123456789ABCDEF";
                foreach (byte b in hash)
                {
                    rv.Append(symbols[b / 16]);
                    rv.Append(symbols[b % 16]);
                }
                return rv.ToString();
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
