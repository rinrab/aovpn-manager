using Microsoft.Management.Infrastructure;
using System;
using System.Security;

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
                    string escapedConnectionName = Uri.EscapeDataString(settings.ConnectionName);
                    string escapedProfile = SecurityElement.Escape(settings.Profile);

                    using (VpnManager vpnManager = new VpnManager())
                    {
                        try
                        {
                            using (CimInstance oldInstance = vpnManager.GetVpnConnection(escapedConnectionName, logger))
                            {
                                if (oldInstance == null)
                                {
                                    vpnManager.CreateVpnConnection(escapedConnectionName, escapedProfile, logger);
                                }
                                else
                                {
                                    vpnManager.UpdateVpnConnection(escapedConnectionName, escapedProfile, logger);
                                }
                            }
                        }
                        catch (CimException ex)
                        {
                            throw new Exception(string.Format("{0}", ex.NativeErrorCode), ex);
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
