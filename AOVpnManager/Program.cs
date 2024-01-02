﻿using Microsoft.Win32;
using System;

namespace AOVpnManager
{
    public class Program
    {
        static int Main(string[] args)
        {
            CommandLineOptions options = CommandLineOptions.Parse(args);

            ILogger logger = CreateLogger(options.IsConsole);
            IGroupPolicyProvider policyProvider = new GroupPolicyProvider(Registry.LocalMachine, @"SOFTWARE\Policies\AOVpnManager");
            IStateManager stateManager = new StateManager(Registry.LocalMachine, @"SOFTWARE\AOVpnManager");

            logger.Started();
            int exitCode = 0;

            try
            {
                GroupPolicySettings settings = policyProvider.ReadSettings();
                string lastConnectionName = stateManager.GetLastConnectionName();

                using (IVpnManager vpnManager = VpnManager.Create())
                {
                    if (string.IsNullOrEmpty(settings.VpnProfileXml))
                    {
                        if (lastConnectionName != null)
                        {
                            vpnManager.DeleteVpnConnection(lastConnectionName);
                            logger.VpnConnectionDeleted(lastConnectionName);
                            stateManager.SetLastConnectionName(null);
                        }
                    }
                    else
                    {
                        if (lastConnectionName != null && lastConnectionName != settings.VpnConnectionName)
                        {
                            vpnManager.DeleteVpnConnection(lastConnectionName);
                            logger.VpnConnectionDeleted(lastConnectionName);
                            stateManager.SetLastConnectionName(null);
                        }

                        VpnConnectionInfo oldConnection = FindVpnConnection(vpnManager, settings.VpnConnectionName);

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

                        stateManager.SetLastConnectionName(settings.VpnConnectionName);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Exception(ex.Message, ex.StackTrace);

                exitCode = 1;
            }

            logger.Finished(exitCode);

            return exitCode;
        }

        static ILogger CreateLogger(bool isConsole)
        {
            if (isConsole)
            {
                return new ConsoleLogger();
            }
            else
            {
                return new EventSourceLogger();
            }
        }

        static VpnConnectionInfo FindVpnConnection(IVpnManager vpnManager, string connectionName)
        {
            foreach (VpnConnectionInfo connection in vpnManager.EnumarateVpnConnections())
            {
                if (connection.ConnectionName == connectionName)
                {
                    return connection;
                }
            }
            return null;
        }
    }
}
