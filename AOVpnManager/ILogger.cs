namespace AOVpnManager
{
    public interface ILogger
    {
        void Exception(string message, string stackTrace);
        void Finished(int code);
        void Started();
        void VpnConnectionCreated(string connectionName);
        void VpnConnectionUpdated(string connectionName);
        void VpnCreationSkipped();
        void Trace(string message);
    }
}