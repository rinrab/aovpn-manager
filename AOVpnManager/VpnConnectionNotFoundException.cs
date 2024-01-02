using System;

namespace AOVpnManager
{
    public class VpnConnectionNotFoundException : Exception
    {
        public VpnConnectionNotFoundException() : base("Vpn Connection not found.")
        {
        }

        public VpnConnectionNotFoundException(string connectionName) :
            base(string.Format("Vpn Connection '{0}' not found.", connectionName))
        {
        }
    }
}