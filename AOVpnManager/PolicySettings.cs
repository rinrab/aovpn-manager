using Microsoft.Win32;

namespace AOVpnManager
{
    public class PolicySettings
    {
        public string Profile { get; set; }
        public string ConnectionName { get; set; }

        public PolicySettings(string profile, string connectionName)
        {
            Profile = profile;
            ConnectionName = connectionName;
        }

        public static PolicySettings Read()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\AOVpnManager"))
            {
                if (key != null)
                {
                    string[] profile = (string[])key.GetValue("Profile");
                    string connectionName = (string)key.GetValue("ConnectionName");

                    return new PolicySettings((profile == null) ? null : string.Join("\n", profile), connectionName);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
