namespace AOVpnManager
{
    public class GroupPolicySettings
    {
        public string VpnProfileXml { get; }
        public string VpnConnectionName { get; }

        public GroupPolicySettings(string profile, string connectionName)
        {
            VpnProfileXml = profile;
            VpnConnectionName = connectionName;
        }
    }
}
