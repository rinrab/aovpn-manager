using Microsoft.Win32;

namespace AOVpnManager
{
    public class PolicySettingsProvider
    {
        private readonly RegistryKey root;
        private readonly string path;

        public PolicySettingsProvider(RegistryKey root, string path)
        {
            this.root = root;
            this.path = path;
        }

        public PolicySettings ReadSettings()
        {
            using (RegistryKey key = root.OpenSubKey(path))
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
