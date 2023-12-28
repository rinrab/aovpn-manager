using Microsoft.Management.Infrastructure;
using System;

namespace AOVpnManager
{
    public class Program
    {
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
                    using (CimSession session = CimSession.Create(null))
                    {
                        string className = "MDM_VPNv2_01";
                        string namespaceName = @"root\cimv2\mdm\dmmap";
                        settings.ConnectionName = settings.ConnectionName.Replace(" ", "%20");

                        settings.Profile = settings.Profile.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

                        foreach (CimInstance insatnce in session.EnumerateInstances(namespaceName, className))
                        {
                            if ((string)insatnce.CimInstanceProperties["InstanceID"].Value == settings.ConnectionName)
                            {
                                // TODO: Do not remove old connection
                                session.DeleteInstance(insatnce);
                            }
                        }

                        using (CimInstance newInstance = new CimInstance(className, namespaceName))
                        {
                            newInstance.CimInstanceProperties.Add(CimProperty.Create("ParentID", "./Vendor/MSFT/VPNv2", CimType.String, CimFlags.Key));
                            newInstance.CimInstanceProperties.Add(CimProperty.Create("InstanceID", settings.ConnectionName, CimType.String, CimFlags.Key));
                            newInstance.CimInstanceProperties.Add(CimProperty.Create("ProfileXML", settings.Profile, CimType.String, CimFlags.Property));

                            session.CreateInstance(namespaceName, newInstance);
                        }

                        MinimalEventSource.Log.VpnConnectionCreated(settings.ConnectionName);
                    }
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
    }
}
