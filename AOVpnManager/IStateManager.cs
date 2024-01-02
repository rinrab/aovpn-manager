namespace AOVpnManager
{
    public interface IStateManager
    {
        string ReadLastConnectionName();
        void UpdateLastConnectionName(string connectionName);
    }
}
