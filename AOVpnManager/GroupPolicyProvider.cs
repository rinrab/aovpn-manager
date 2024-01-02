using Microsoft.Win32;

namespace AOVpnManager
{
    public class GroupPolicyProvider : IGroupPolicyProvider
    {
        const string DefaultConnectionName = "Always On Vpn";
        const string Profile = "Profile";
        const string ConnectionName = "ConnectionName";

        private readonly RegistryKey root;
        private readonly string path;

        public GroupPolicyProvider(RegistryKey root, string path)
        {
            this.root = root;
            this.path = path;
        }

        public GroupPolicySettings ReadSettings()
        {
            using (GroupPolicyApi.ObtainGroupPolicyLock(true))
            using (RegistryKey key = root.OpenSubKey(path))
            {
                if (key == null)
                {
                    return new GroupPolicySettings(null, null);
                }
                else
                {
                    string[] profile = key.GetValue<string[]>(Profile, null);
                    string connectionName = key.GetValue<string>(ConnectionName, DefaultConnectionName);

                    return new GroupPolicySettings((profile == null) ? null : string.Join("\n", profile), connectionName);
                }
            }
        }
    }
}
