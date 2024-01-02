using Microsoft.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security;

namespace AOVpnManager
{
    public class VpnManager : IDisposable, IVpnManager
    {
        const string ClassName = "MDM_VPNv2_01";
        const string NamespaceName = @"root\cimv2\mdm\dmmap";

        private readonly CimSession session;

        private VpnManager(CimSession session)
        {
            this.session = session;
        }

        public static IVpnManager Create()
        {
            return new VpnManager(CimSession.Create(null));
        }

        public void CreateVpnConnection(string connectionName, string profile)
        {
            using (CimInstance newInstance = new CimInstance(ClassName, NamespaceName))
            {
                AddKeyPropertiesToVpnConnection(newInstance, connectionName);
                AddValuePropertiesToVpnConnection(newInstance, profile);
                session.CreateInstance(NamespaceName, newInstance);
            }
        }

        public void UpdateVpnConnection(string connectionName, string profile)
        {
            using (CimInstance newInstance = new CimInstance(ClassName, NamespaceName))
            {
                AddKeyPropertiesToVpnConnection(newInstance, connectionName);
                AddValuePropertiesToVpnConnection(newInstance, profile);
                session.ModifyInstance(NamespaceName, newInstance);
            }
        }

        public void DeleteVpnConnection(string connectionName)
        {
            using (CimInstance queryInstance = new CimInstance(ClassName, NamespaceName))
            {
                AddKeyPropertiesToVpnConnection(queryInstance, connectionName);
                session.DeleteInstance(queryInstance);
            }
        }

        public IEnumerable<VpnConnectionInfo> EnumarateVpnConnections()
        {
            foreach (CimInstance instance in session.EnumerateInstances(NamespaceName, ClassName))
            {
                using (instance)
                {
                    string connectionName = UnescapeConnectionName((string)instance.CimInstanceProperties["InstanceID"].Value);
                    string profileXml = (string)instance.CimInstanceProperties["ProfileXML"].Value;
                    yield return new VpnConnectionInfo(connectionName, profileXml);
                }
            }
        }

        private void AddKeyPropertiesToVpnConnection(CimInstance instance, string connectionName)
        {
            instance.CimInstanceProperties.Add(CimProperty.Create("ParentID", "./Vendor/MSFT/VPNv2", CimType.String, CimFlags.Key));
            instance.CimInstanceProperties.Add(CimProperty.Create("InstanceID", EscapeConnectionName(connectionName), CimType.String, CimFlags.Key));
        }

        private void AddValuePropertiesToVpnConnection(CimInstance instance, string profileXml)
        {
            instance.CimInstanceProperties.Add(CimProperty.Create("ProfileXML", EscapeProfileXml(profileXml), CimType.String, CimFlags.Property));
        }

        private string EscapeConnectionName(string connectionName)
        {
            return Uri.EscapeDataString(connectionName);
        }

        private string UnescapeConnectionName(string connectionName)
        {
            return Uri.UnescapeDataString(connectionName);
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
