using Microsoft.Management.Infrastructure;
using System;

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

        public void CreateVpnConnection(string escapedConnectionName, string escapedProfile, ILogger logger)
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

        public void UpdateVpnConnection(string escapedConnectionName, string escapedProfile, ILogger logger)
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

        public CimInstance GetVpnConnection(string connectionName, ILogger logger)
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

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
