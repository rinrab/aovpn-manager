using Microsoft.Win32;

namespace AOVpnManager
{
    public class PolicySettings
    {
        public string Profile { get; }
        public string ConnectionName { get; }

        public PolicySettings(string profile, string connectionName)
        {
            Profile = profile;
            ConnectionName = connectionName;
        }
    }
}
