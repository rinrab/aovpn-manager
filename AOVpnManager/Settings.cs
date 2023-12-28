using Microsoft.Win32;

namespace AOVpnManager
{
    public class Settings
    {
        public string Profile { get; set; }
        public string ConnectionName { get; set; }

        public Settings(string profile, string connectionName)
        {
            Profile = profile;
            ConnectionName = connectionName;
        }

        public static Settings Load()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\AOVpnManager"))
            {
                string[] profile = (string[])key?.GetValue("Profile");
                string connectionName = (string)key?.GetValue("ConnectionName");

                return new Settings((profile == null) ? null : string.Join("\n", profile), connectionName);
            }
        }
    }
}
