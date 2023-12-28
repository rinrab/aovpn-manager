using Microsoft.Management.Infrastructure;
using System;
using System.Security;

namespace AOVpnManager
{
    public class Program
    {
        const string ClassName = "MDM_VPNv2_01";
        const string NamespaceName = @"root\cimv2\mdm\dmmap";

        static int Main(string[] args)
        {
            ILogger logger;
            if (args.Length > 0 && args[0] == "/console")
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
                var settings = Settings.Load();

                if (string.IsNullOrEmpty(settings.Profile) || string.IsNullOrEmpty(settings.ConnectionName))
                {
                    logger.VpnCreationSkipped();
                }
                else
                {
                    string escapedConnectionName = Uri.EscapeDataString(settings.ConnectionName);
                    string escapedProfile = SecurityElement.Escape(settings.Profile);

                    using (CimSession session = CimSession.Create(null))
                    {
                        try
                        {
                            using (CimInstance oldInstance = GetVpnConnection(session, escapedConnectionName, logger))
                            {
                                if (oldInstance == null)
                                {
                                    CreateVpnConnection(session, escapedConnectionName, escapedProfile, logger);
                                }
                                else
                                {
                                    UpdateVpnConnection(session, escapedConnectionName, escapedProfile, logger);
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

        static void CreateVpnConnection(CimSession session, string escapedConnectionName, string escapedProfile, ILogger logger)
        {
            using (CimInstance newInstance = new CimInstance(ClassName, NamespaceName))
            {
                newInstance.CimInstanceProperties.Add(CimProperty.Create("ParentID", "./Vendor/MSFT/VPNv2", CimType.String, CimFlags.Key));
                newInstance.CimInstanceProperties.Add(CimProperty.Create("InstanceID", escapedConnectionName, CimType.String, CimFlags.Key));
                newInstance.CimInstanceProperties.Add(CimProperty.Create("ProfileXML", escapedProfile, CimType.String, CimFlags.Property));

                session.CreateInstance(NamespaceName, newInstance);

                logger.VpnConnectionCreated(escapedConnectionName);
            }
        }

        static void UpdateVpnConnection(CimSession session, string escapedConnectionName, string escapedProfile, ILogger logger)
        {
            using (CimInstance newInstance = new CimInstance(ClassName, NamespaceName))
            {
                newInstance.CimInstanceProperties.Add(CimProperty.Create("ParentID", "./Vendor/MSFT/VPNv2", CimType.String, CimFlags.Key));
                newInstance.CimInstanceProperties.Add(CimProperty.Create("InstanceID", escapedConnectionName, CimType.String, CimFlags.Key));
                newInstance.CimInstanceProperties.Add(CimProperty.Create("ProfileXML", escapedProfile, CimType.String, CimFlags.Property));

                session.ModifyInstance(NamespaceName, newInstance);

                logger.VpnConnectionCreated(escapedConnectionName);
            }
        }

        static CimInstance GetVpnConnection(CimSession session, string connectionName, ILogger logger)
        {
            foreach (CimInstance instance in session.EnumerateInstances(NamespaceName, ClassName))
            {
                logger.Trace(instance.ToString());
                if ((string)instance.CimInstanceProperties["InstanceID"].Value == connectionName)
                {
                    return instance;
                }

                instance.Dispose();
            }

            return null;
        }
    }
}
