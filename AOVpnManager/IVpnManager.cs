using System;
using System.Collections.Generic;

namespace AOVpnManager
{
    public interface IVpnManager : IDisposable
    {
        void CreateVpnConnection(string connectionName, string profile);
        void DeleteVpnConnection(string connectionName);
        IEnumerable<VpnConnectionInfo> EnumarateVpnConnections();
    }
}