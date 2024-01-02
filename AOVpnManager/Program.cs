using Microsoft.Management.Infrastructure;
using Microsoft.Win32;
using System;
using System.Linq;

namespace AOVpnManager
{
    public class Program
    {
        static int Main(string[] args)
        {
            CommandLineArguments options = CommandLineArguments.Read(args);

            ILogger logger;
            if (options.IsConsole)
            {
                logger = new ConsoleLogger();
            }
            else
            {
                logger = new EventViewerLogger();
            }

            IGroupPolicyProvider policyProvider = new GroupPolicyProvider(Registry.LocalMachine, @"SOFTWARE\Policies\AOVpnManager");
            IStateManager stateManager = new StateManager(Registry.LocalMachine, @"SOFTWARE\AOVpnManager");

            logger.Started();
            int exitCode = 0;

            try
            {
                GroupPolicySettings settings = policyProvider.ReadSettings();
                string lastConnectionName = stateManager.ReadLastConnectionName();

                using (IVpnManager vpnManager = VpnManager.Create())
                {
                    if (string.IsNullOrEmpty(settings.VpnProfileXml))
                    {
                        if (lastConnectionName != null)
                        {
                            vpnManager.DeleteVpnConnection(lastConnectionName);
                            logger.VpnConnectionDeleted(lastConnectionName);
                            stateManager.UpdateLastConnectionName(null);
                        }

                        logger.VpnCreationSkipped();
                    }
                    else
                    {
                        if (lastConnectionName != null && lastConnectionName != settings.VpnConnectionName)
                        {
                            vpnManager.DeleteVpnConnection(lastConnectionName);
                            logger.VpnConnectionDeleted(lastConnectionName);
                            stateManager.UpdateLastConnectionName(null);
                        }

                        VpnConnectionInfo oldConnection = vpnManager.EnumarateVpnConnections().FirstOrDefault(
                            connection => connection.ConnectionName == settings.VpnConnectionName);

                        logger.Trace("oldConnection: " + oldConnection?.ToString());

                        if (oldConnection == null)
                        {
                            vpnManager.CreateVpnConnection(settings.VpnConnectionName, settings.VpnProfileXml);
                            logger.VpnConnectionCreated(settings.VpnConnectionName);
                        }
                        else
                        {
                            vpnManager.UpdateVpnConnection(settings.VpnConnectionName, settings.VpnProfileXml);
                            logger.VpnConnectionUpdated(settings.VpnConnectionName);
                        }

                        stateManager.UpdateLastConnectionName(settings.VpnConnectionName);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex is CimException cimException)
                {
                    message += string.Format(" ({0})", cimException.NativeErrorCode.ToString());
                }

                logger.Exception(message, ex.StackTrace);

                exitCode = 1;
            }

            logger.Finished(exitCode);

            return exitCode;
        }
    }
}
