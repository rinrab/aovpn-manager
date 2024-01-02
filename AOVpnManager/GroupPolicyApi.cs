using System;
using System.Runtime.InteropServices;

namespace AOVpnManager
{
    public static class GroupPolicyApi
    {
        private class GroupPolicyLock : IDisposable
        {
            private readonly IntPtr handle;

            public GroupPolicyLock(IntPtr handle)
            {
                this.handle = handle;
            }

            public void Dispose()
            {
                if (!LeaveCriticalPolicySection(handle))
                {
                    throw new Exception(string.Format("LeaveCriticalPolicySection failed with code: {0}", Marshal.GetLastWin32Error()));
                }
            }
        }

        public static IDisposable EnterCriticalPolicySection()
        {
            IntPtr handle = EnterCriticalPolicySection(true);

            if (handle == IntPtr.Zero)
            {
                throw new Exception(string.Format("EnterCriticalPolicySection failed with code: {0}", Marshal.GetLastWin32Error()));
            }
            else
            {
                return new GroupPolicyLock(handle);
            }
        }

        [DllImport("Userenv.dll")]
        private static extern IntPtr EnterCriticalPolicySection([In] bool bMachine);

        [DllImport("Userenv.dll")]
        private static extern bool LeaveCriticalPolicySection([In] IntPtr hSection);
    }
}
