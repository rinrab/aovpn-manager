using Microsoft.Management.Infrastructure;
using System;
using System.Runtime;
using System.Security;

namespace AOVpnManager
{
    public class VpnManager : IDisposable
    {
        const string ClassName = "MDM_VPNv2_01";
        const string NamespaceName = @"root\cimv2\mdm\dmmap";

        private readonly CimSession session;

        public VpnManager()
        {
            session = CimSession.Create(null);
        }

        public void CreateVpnConnection(string connectionName, string profile, ILogger logger)
        {
            using (CimInstance newInstance = new CimInstance(ClassName, NamespaceName))
            {
                newInstance.CimInstanceProperties.Add(CimProperty.Create("ParentID", "./Vendor/MSFT/VPNv2", CimType.String, CimFlags.Key));
                newInstance.CimInstanceProperties.Add(CimProperty.Create("InstanceID", EscapeConnectionName(connectionName), CimType.String, CimFlags.Key));
                newInstance.CimInstanceProperties.Add(CimProperty.Create("ProfileXML", EscapeProfileXml(profile), CimType.String, CimFlags.Property));

                session.CreateInstance(NamespaceName, newInstance);

                logger.VpnConnectionCreated(connectionName);
            }
        }

        public void UpdateVpnConnection(string connectionName, string profile, ILogger logger)
        {
            using (CimInstance newInstance = new CimInstance(ClassName, NamespaceName))
            {
                newInstance.CimInstanceProperties.Add(CimProperty.Create("ParentID", "./Vendor/MSFT/VPNv2", CimType.String, CimFlags.Key));
                newInstance.CimInstanceProperties.Add(CimProperty.Create("InstanceID", EscapeConnectionName(connectionName), CimType.String, CimFlags.Key));
                newInstance.CimInstanceProperties.Add(CimProperty.Create("ProfileXML", EscapeProfileXml(profile), CimType.String, CimFlags.Property));

                session.ModifyInstance(NamespaceName, newInstance);

                logger.VpnConnectionCreated(connectionName);
            }
        }

        public CimInstance GetVpnConnection(string connectionName, ILogger logger)
        {
            string escapedConnectionName = EscapeConnectionName(connectionName);

            foreach (CimInstance instance in session.EnumerateInstances(NamespaceName, ClassName))
            {
                logger.Trace(instance.ToString());
                if ((string)instance.CimInstanceProperties["InstanceID"].Value == escapedConnectionName)
                {
                    return instance;
                }

                instance.Dispose();
            }

            return null;
        }

        private string EscapeConnectionName(string connectionName)
        {
            return Uri.EscapeDataString(connectionName);
        }

        private string EscapeProfileXml(string profileXml)
        {
            return SecurityElement.Escape(profileXml);
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
