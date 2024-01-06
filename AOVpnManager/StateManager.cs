using Microsoft.Win32;

namespace AOVpnManager
{
    public class StateManager : IStateManager
    {
        private const string LastConnectionName = nameof(LastConnectionName);
        private const string LastLastVpnProfile = nameof(LastLastVpnProfile);

        private readonly RegistryKey root;
        private readonly string path;

        public StateManager(RegistryKey root, string path)
        {
            this.root = root;
            this.path = path;
        }

        public string GetLastConnectionName()
        {
            using (RegistryKey key = root.OpenSubKey(path))
            {
                return key.GetValue<string>(LastConnectionName, null);
            }
        }

        public string GetLastVpnProfile()
        {
            using (RegistryKey key = root.OpenSubKey(path))
            {
                return key.GetValue<string>(LastLastVpnProfile, null);
            }
        }

        public void SetLastConnectionName(string connectionName)
        {
            SetValue(LastConnectionName, connectionName);
        }

        public void SetLastVpnProfile(string profileXml)
        {
            SetValue(LastLastVpnProfile, profileXml);
        }

        private void SetValue(string keyName, object value)
        {
            using (RegistryKey key = root.CreateSubKey(path, true))
            {
                if (value == null)
                {
                    key.DeleteValue(keyName, false);
                }
                else
                {
                    key.SetValue(keyName, value);
                }
            }
        }
    }
}
