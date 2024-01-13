using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Networking.Vpn;

namespace AdvancedVpnManager
{
    public class ListConnectionsViewModel : INotifyPropertyChanged
    {
        private readonly VpnManagementAgent vpnAgent;
        private VpnConnection[] vpnConnections;

        public event PropertyChangedEventHandler PropertyChanged;

        public ListConnectionsViewModel()
        {
            vpnAgent = new VpnManagementAgent();
        }

        public VpnConnection[] VpnConnections
        {
            get
            {
                return vpnConnections;
            }
        }

        public async Task RefreshAsync()
        {
            vpnConnections = await GetVpnConnections();

            InvokePropertyChanged(nameof(VpnConnections));
        }

        private async Task<VpnConnection[]> GetVpnConnections()
        {
            var profiles = await vpnAgent.GetProfilesAsync();

            List<VpnConnection> rv = new List<VpnConnection>(profiles.Count);

            foreach (IVpnProfile profile in profiles)
            {
                rv.Add(new VpnConnection
                {
                    Name = profile.ProfileName
                });
            }

            return rv.ToArray();
        }

        private void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class VpnConnection
    {
        public string Name { get; set; }
    }
}
