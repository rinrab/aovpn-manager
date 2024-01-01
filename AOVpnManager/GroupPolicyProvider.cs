using Microsoft.Win32;

namespace AOVpnManager
{
    public class GroupPolicyProvider : IGroupPolicyProvider
    {
        private readonly RegistryKey root;
        private readonly string path;

        public GroupPolicyProvider(RegistryKey root, string path)
        {
            this.root = root;
            this.path = path;
        }

        public GroupPolicySettings ReadSettings()
        {
            using (RegistryKey key = root.OpenSubKey(path))
            {
                if (key != null)
                {
                    string[] profile = (string[])key.GetValue("Profile");
                    string connectionName = (string)key.GetValue("ConnectionName");

                    return new GroupPolicySettings((profile == null) ? null : string.Join("\n", profile), connectionName);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
