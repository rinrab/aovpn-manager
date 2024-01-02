namespace AOVpnManager
{
    public class VpnConnectionInfo
    {
        public string ConnectionName { get; }
        public string Profile { get; }

        public VpnConnectionInfo(string connectionName, string profile)
        {
            ConnectionName = connectionName;
            Profile = profile;
        }

        public override string ToString()
        {
            return string.Format("ConnectionName: {0}, Profile: {1}", ConnectionName, Profile);
        }
    }
}