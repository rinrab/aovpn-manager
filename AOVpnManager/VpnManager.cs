using Microsoft.Management.Infrastructure;
using System;
using System.Security;

namespace AOVpnManager
{
    public class VpnManager : IVpnManager
    {
        private const string ClassName = "MDM_VPNv2_01";
        private const string NamespaceName = @"root\cimv2\mdm\dmmap";

        private static class PropertyNames
        {
            public const string ConnectionName = "InstanceID";
            public const string ParentId = "ParentID";
            public const string ProfileXml = "ProfileXML";
        }

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
            try
            {
                using (CimInstance newInstance = new CimInstance(ClassName, NamespaceName))
                {
                    AddKeyPropertiesToVpnConnection(newInstance, connectionName);
                    AddValuePropertiesToVpnConnection(newInstance, profile);
                    session.CreateInstance(NamespaceName, newInstance);
                }
            }
            catch (CimException ex)
            {
                throw ConvertCimException(ex);
            }
        }

        public void DeleteVpnConnection(string connectionName)
        {
            try
            {
                using (CimInstance queryInstance = new CimInstance(ClassName, NamespaceName))
                {
                    AddKeyPropertiesToVpnConnection(queryInstance, connectionName);
                    session.DeleteInstance(queryInstance);
                }
            }
            catch (CimException ex)
            {
                throw ConvertCimException(ex);
            }
        }

        private static Exception ConvertCimException(CimException ex)
        {
            if (ex.NativeErrorCode == NativeErrorCode.NotFound)
            {
                return new VpnConnectionNotFoundException();
            }
            else if (ex.NativeErrorCode == NativeErrorCode.AlreadyExists)
            {
                return new VpnConnectionAlreadyExistsException();
            }
            else
            {
                return new Exception(string.Format("{0} ({1})", ex.Message, ex.NativeErrorCode), ex);
            }
        }

        private void AddKeyPropertiesToVpnConnection(CimInstance instance, string connectionName)
        {
            instance.CimInstanceProperties.Add(CimProperty.Create(PropertyNames.ParentId, "./Vendor/MSFT/VPNv2", CimType.String, CimFlags.Key));
            instance.CimInstanceProperties.Add(CimProperty.Create(PropertyNames.ConnectionName, EscapeConnectionName(connectionName), CimType.String, CimFlags.Key));
        }

        private void AddValuePropertiesToVpnConnection(CimInstance instance, string profileXml)
        {
            instance.CimInstanceProperties.Add(CimProperty.Create(PropertyNames.ProfileXml, EscapeProfileXml(profileXml), CimType.String, CimFlags.Property));
        }

        private static string EscapeConnectionName(string connectionName)
        {
            return Uri.EscapeDataString(connectionName);
        }

        private static string UnescapeConnectionName(string connectionName)
        {
            return Uri.UnescapeDataString(connectionName);
        }

        private static string EscapeProfileXml(string profileXml)
        {
            return SecurityElement.Escape(profileXml);
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
