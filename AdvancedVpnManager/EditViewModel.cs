using System;
using System.ComponentModel;
using Windows.Networking.Vpn;
using Windows.UI.Popups;

namespace AdvancedVpnManager
{
    public class EditViewModel : INotifyPropertyChanged
    {
        private readonly VpnNativeProfile vpnProfile;

        public EditViewModel(VpnNativeProfile vpnProfile)
        {
            this.vpnProfile = vpnProfile;
        }

        public async void UpdateProfile()
        {
            // TODO:
        }

        public string Name
        {
            get
            {
                return vpnProfile.ProfileName;
            }
            set
            {
                // TODO:
            }
        }

        public string Servers
        {
            get
            {
                return string.Join(';', vpnProfile.Servers);
            }
            set
            {
                vpnProfile.Servers.Clear();

                foreach (string server in value.Split(';', StringSplitOptions.RemoveEmptyEntries))
                {
                    vpnProfile.Servers.Add(server);
                }
            }
        }

        public int ProtocolTypeIndex
        {
            get
            {
                return (int)vpnProfile.NativeProtocolType;
            }
            set
            {
                vpnProfile.NativeProtocolType = (VpnNativeProtocolType)value;
            }
        }

        public int AuthenticationMethodIndex
        {
            get
            {
                return (int)vpnProfile.UserAuthenticationMethod;
            }
            set
            {
                vpnProfile.UserAuthenticationMethod = (VpnAuthenticationMethod)value;
                InvokePropertyChanged(nameof(IsEapConfigurationEnabled));
            }
        }

        public string EapConfiguration
        {
            get
            {
                return vpnProfile.EapConfiguration;
            }
            set
            {
                vpnProfile.EapConfiguration = value;
            }
        }

        public bool IsEapConfigurationEnabled
        {
            get
            {
                return vpnProfile.UserAuthenticationMethod == VpnAuthenticationMethod.Eap;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
