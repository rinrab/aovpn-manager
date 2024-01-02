using Microsoft.Win32;
using System;

namespace AOVpnManager
{
    public static class RegistryKeyExtensions
    {
        public static T GetValue<T>(this RegistryKey key, string name, T defaultValue)
        {
            object obj = key.GetValue(name);

            if (obj == null)
            {
                return defaultValue;
            }

            if (obj is T rv)
            {
                return rv;
            }
            else
            {
                throw new Exception(string.Format("Invalid type of registry value '{0}'", name));
            }
        }
    }
}
