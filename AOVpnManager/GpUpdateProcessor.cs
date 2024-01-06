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
            string oldVpnProfileHash = stateManager.GetLastVpnProfile();
            string newVpnProfileHash = ComputeHash(settings.VpnProfileXml);

            bool settingsChanged = false;

            if (lastConnectionName != settings.VpnConnectionName)
            {
                settingsChanged = true;
            }
            else if (oldVpnProfileHash != newVpnProfileHash)
            {
                settingsChanged = true;
            }

            if (settingsChanged)
            {
                if (lastConnectionName != null)
                {
                    // Remove
                    try
                    {
                        vpnManager.DeleteVpnConnection(lastConnectionName);
                        logger.VpnConnectionDeleted(lastConnectionName);
                    }
                    catch (VpnConnectionNotFoundException)
                    {
                    }

                    stateManager.SetLastConnectionName(null);
                    stateManager.SetLastVpnProfile(null);
                }

                if (settings.VpnProfileXml != null)
                {
                    // Create
                    stateManager.SetLastConnectionName(settings.VpnConnectionName);
                    vpnManager.CreateVpnConnection(settings.VpnConnectionName, settings.VpnProfileXml);
                    stateManager.SetLastVpnProfile(newVpnProfileHash);

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
    }
}
