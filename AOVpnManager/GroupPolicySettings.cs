namespace AOVpnManager
{
    public class GroupPolicySettings
    {
        public string Profile { get; }
        public string ConnectionName { get; }

        public GroupPolicySettings(string profile, string connectionName)
        {
            Profile = profile;
            ConnectionName = connectionName;
        }
    }
}
