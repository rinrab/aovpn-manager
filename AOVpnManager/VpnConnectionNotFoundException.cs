using System;

namespace AOVpnManager
{
    [Serializable]
    public class VpnConnectionNotFoundException : Exception
    {
        public VpnConnectionNotFoundException(string connectionName) :
            base(string.Format("Vpn Connection '{0}' not found.", connectionName))
        {
        }
    }
}