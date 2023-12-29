using Microsoft.Management.Infrastructure;

namespace AOVpnManager
{
    public class VpnManager
    {
        const string ClassName = "MDM_VPNv2_01";
        const string NamespaceName = @"root\cimv2\mdm\dmmap";

        public static void CreateVpnConnection(CimSession session, string escapedConnectionName, string escapedProfile, ILogger logger)
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

        public static void UpdateVpnConnection(CimSession session, string escapedConnectionName, string escapedProfile, ILogger logger)
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

        public static CimInstance GetVpnConnection(CimSession session, string connectionName, ILogger logger)
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
