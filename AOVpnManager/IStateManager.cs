namespace AOVpnManager
{
    public interface IStateManager
    {
        string GetLastConnectionName();
        void SetLastConnectionName(string connectionName);
    }
}
