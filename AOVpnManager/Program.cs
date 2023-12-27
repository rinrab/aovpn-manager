using Microsoft.Management.Infrastructure;
using Microsoft.Win32;
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
                string profile = GetProfile();
                string connectionName = GetConnectionName();

                if (string.IsNullOrEmpty(profile) || string.IsNullOrEmpty(connectionName))
                {
                    MinimalEventSource.Log.VpnCreationSkipped();
                }
                else
                {
                    using (CimSession session = CimSession.Create(null))
                    {
                        string className = "MDM_VPNv2_01";
                        string namespaceName = @"root\cimv2\mdm\dmmap";
                        connectionName = connectionName.Replace(" ", "%20");

                        profile = profile.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

                        foreach (CimInstance insatnce in session.EnumerateInstances(namespaceName, className))
                        {
                            if ((string)insatnce.CimInstanceProperties["InstanceID"].Value == connectionName)
                            {
                                // TODO: Do not remove old connection
                                session.DeleteInstance(insatnce);
                            }
                        }

                        CimInstance newInstance = new CimInstance(className, namespaceName);
                        newInstance.CimInstanceProperties.Add(CimProperty.Create("ParentID", "./Vendor/MSFT/VPNv2", CimType.String, CimFlags.Key));
                        newInstance.CimInstanceProperties.Add(CimProperty.Create("InstanceID", connectionName, CimType.String, CimFlags.Key));
                        newInstance.CimInstanceProperties.Add(CimProperty.Create("ProfileXML", profile, CimType.String, CimFlags.Property));

                        session.CreateInstance(namespaceName, newInstance);

                        MinimalEventSource.Log.VpnConnectionCreated(connectionName);
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

        static string GetProfile()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\AOVpnManager"))
            {
                string[] str = (string[])key?.GetValue("Profile");
                return str != null ? string.Join("\n", str) : null;
            }
        }

        static string GetConnectionName()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\AOVpnManager"))
            {
                string str = (string)key?.GetValue("ConnectionName");
                return string.IsNullOrEmpty(str) ? null : str;
            }
        }
    }
}
