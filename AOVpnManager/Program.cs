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
                string profile = GetProfile() ?? throw new Exception("Profile not found.");

                MinimalEventSource.Log.ProfileChanged(profile);
            }
            catch (Exception ex)
            {
                MinimalEventSource.Log.Exception(ex.Message, ex.StackTrace);

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
                if (str != null)
                {
                    return string.Join("\n", str);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
