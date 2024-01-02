using System;

namespace AOVpnManager
{
    [Serializable]
    internal class VpnConnectionAlreadyExistsException : Exception
    {
        public VpnConnectionAlreadyExistsException() : base("Vpn Connection already exists.")
        {
        }

        public VpnConnectionAlreadyExistsException(string connectionName) :
            base(string.Format("Vpn Connection '{0}' already exists.", connectionName))
        {
        }
    }
}