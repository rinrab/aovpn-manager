using Microsoft.Management.Infrastructure;
using System;
using System.Linq;
using System.Security;

namespace AOVpnManager
{
    public class Program
    {
        const string ClassName = "MDM_VPNv2_01";
        const string NamespaceName = @"root\cimv2\mdm\dmmap";

        static int Main(string[] args)
        {
            MinimalEventSource.Log.Started();
            int exitCode = 0;

            try
            {
                var settings = Settings.Load();

                if (string.IsNullOrEmpty(settings.Profile) || string.IsNullOrEmpty(settings.ConnectionName))
                {
                    MinimalEventSource.Log.VpnCreationSkipped();
                }
                else
                {
                    CreateVpnConnection(settings);
                }
            }
            catch (Exception ex)
            {
                MinimalEventSource.Log.Exception(ex.Message, ex.StackTrace);

                Console.WriteLine(ex);

                exitCode = 1;
            }

            MinimalEventSource.Log.Finished(exitCode);

            return exitCode;
        }

        static void CreateVpnConnection(Settings settings)
        {
            using (CimSession session = CimSession.Create(null))
            {
                try
                {
                    string escapedConnectionName = Uri.EscapeDataString(settings.ConnectionName);
                    string escapedProfile = SecurityElement.Escape(settings.Profile);

                    using (CimInstance oldInstance = GetVpnConnection(session, escapedConnectionName))
                    {
                        using (CimInstance newInstance = new CimInstance(ClassName, NamespaceName))
                        {
                            newInstance.CimInstanceProperties.Add(CimProperty.Create("ParentID", "./Vendor/MSFT/VPNv2", CimType.String, CimFlags.Key));
                            newInstance.CimInstanceProperties.Add(CimProperty.Create("InstanceID", escapedConnectionName, CimType.String, CimFlags.Key));
                            newInstance.CimInstanceProperties.Add(CimProperty.Create("ProfileXML", escapedProfile, CimType.String, CimFlags.Property));

                            if (oldInstance == null)
                            {
                                session.CreateInstance(NamespaceName, newInstance);

                                MinimalEventSource.Log.VpnConnectionCreated(settings.ConnectionName);
                            }
                            else
                            {
                                session.ModifyInstance(NamespaceName, newInstance);

                                MinimalEventSource.Log.VpnConnectionUpdated(settings.ConnectionName);
                            }
                        }
                    }
                }
                catch (CimException ex)
                {
                    throw new Exception(string.Format("{0}", ex.NativeErrorCode), ex);
                }
            }
        }

        static CimInstance GetVpnConnection(CimSession session, string connectionName)
        {
            foreach (CimInstance instance in session.EnumerateInstances(NamespaceName, ClassName))
            {
                Console.WriteLine(instance);
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
