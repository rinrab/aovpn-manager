namespace AOVpnManager
{
    public interface IStateManager
    {
        string GetLastConnectionName();
        void SetLastConnectionName(string connectionName);

        string GetLastVpnProfile();
        void SetLastVpnProfile(string profileXml);
    }
}
