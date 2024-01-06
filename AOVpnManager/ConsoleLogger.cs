using System;

namespace AOVpnManager
{
    public class ConsoleLogger : ILogger
    {
        public void Exception(string message, string stackTrace)
        {
            Console.WriteLine("Unhandled exception: {0}\n{1}", message, stackTrace);
        }

        public void Started()
        {
            Console.WriteLine("AOVpnManager started");
        }

        public void Finished(int code)
        {
            Console.WriteLine("AOVpnManager finished with code {0}", code);
        }

        public void VpnConnectionCreated(string connectionName)
        {
            Console.WriteLine("Vpn connection \"{0}\" created.", connectionName);
        }

        public void VpnConnectionDeleted(string connectionName)
        {
            Console.WriteLine("Vpn connection \"{0}\" deleted.", connectionName);
        }

        public void Trace(string message)
        {
            Console.WriteLine("{message}");
        }
    }
}
