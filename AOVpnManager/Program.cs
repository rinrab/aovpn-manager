using Microsoft.Management.Infrastructure;
using Microsoft.Win32;
using System;

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

                if (string.IsNullOrEmpty(settings.Profile) || string.IsNullOrEmpty(settings.ConnectionName))
                {
                    logger.VpnCreationSkipped();
                }
                else
                {
                    using (VpnManager vpnManager = new VpnManager())
                    {
                        stateManager.UpdateLastConnectionName(settings.ConnectionName);
                        using (CimInstance oldInstance = vpnManager.GetVpnConnection(settings.ConnectionName))
                        {
                            logger.Trace("oldInstance: " + oldInstance?.ToString());

                            if (oldInstance == null)
                            {
                                vpnManager.CreateVpnConnection(settings.ConnectionName, settings.Profile);
                                logger.VpnConnectionCreated(settings.ConnectionName);
                            }
                            else
                            {
                                vpnManager.UpdateVpnConnection(settings.ConnectionName, settings.Profile);
                                logger.VpnConnectionUpdated(settings.ConnectionName);
                            }
                        }
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
