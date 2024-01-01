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
                    throw new Exception("LeaveCriticalPolicySection failed");
                }
            }

            [DllImport("Userenv.dll")]
            private static extern bool LeaveCriticalPolicySection([In] IntPtr hSection);
        }

        public static IDisposable EnterCriticalPolicySection()
        {
            IntPtr handle = EnterCriticalPolicySection(true);

            if (handle == IntPtr.Zero)
            {
                throw new Exception("EnterCriticalPolicySection failed");
            }
            else
            {
                return new GroupPolicyLock(handle);
            }
        }

        [DllImport("Userenv.dll")]
        private static extern IntPtr EnterCriticalPolicySection([In] bool bMachine);
    }
}
