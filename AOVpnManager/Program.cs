using Microsoft.Management.Infrastructure;
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

            logger.Started();
            int exitCode = 0;

            try
            {
                var settings = Settings.Read();

                if (string.IsNullOrEmpty(settings.Profile) || string.IsNullOrEmpty(settings.ConnectionName))
                {
                    logger.VpnCreationSkipped();
                }
                else
                {
                    using (VpnManager vpnManager = new VpnManager())
                    {
                        using (CimInstance oldInstance = vpnManager.GetVpnConnection(settings.ConnectionName))
                        {
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
                logger.Exception(ex.Message, ex.StackTrace);

                Console.WriteLine(ex);

                exitCode = 1;
            }

            logger.Finished(exitCode);

            return exitCode;
        }
    }
}
