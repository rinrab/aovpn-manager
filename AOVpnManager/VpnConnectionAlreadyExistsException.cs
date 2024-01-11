using System;

namespace AOVpnManager
{
    [Serializable]
    public class VpnConnectionAlreadyExistsException : Exception
    {
        public VpnConnectionAlreadyExistsException(string connectionName) :
            base(string.Format("Vpn Connection '{0}' already exists.", connectionName))
        {
        }
    }
}