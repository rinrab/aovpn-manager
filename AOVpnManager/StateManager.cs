using Microsoft.Win32;

namespace AOVpnManager
{
    public class StateManager : IStateManager
    {
        private const string LastConnectionName = nameof(LastConnectionName);

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

        public void SetLastConnectionName(string connectionName)
        {
            using (RegistryKey key = root.CreateSubKey(path, true))
            {
                if (connectionName == null)
                {
                    key.DeleteValue(LastConnectionName, false);
                }
                else
                {
                    key.SetValue(LastConnectionName, connectionName);
                }
            }
        }
    }
}
