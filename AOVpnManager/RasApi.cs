using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace AOVpnManager
{
    public static class RasApi
    {
        public static void DisconnectAll(Predicate<string> filter)
        {
            uint dwSize = (uint)Marshal.SizeOf(typeof(RASCONN));
            uint count = 1;
            RASCONN[] connections;
            while (true)
            {
                uint cb = dwSize * count;
                connections = new RASCONN[count];
                connections[0].dwSize = dwSize;

                int err = RasEnumConnections(connections, ref cb, ref count);

                if (err == 0)
                {
                    break;
                }
                else if (err != ERROR_BUFFER_TOO_SMALL)
                {
                    throw new Win32Exception(err);
                }

                count = (cb + dwSize - 1) / dwSize;
            }

            bool wait = false;

            for (int i = 0; i < count; i++)
            {
                if (filter(connections[i].szEntryName))
                {
                    Trace.WriteLine("Disconnecting VPN connection '{0}'...", connections[i].szEntryName);
                    RasHangUp(connections[i].hrasconn);
                    wait = true;
                }
            }

            if (wait)
            {
                // Wait for disconnect. See: https://learn.microsoft.com/en-us/windows/win32/api/ras/nf-ras-rashangupa#remarks
                // TODO: wait until RasGetConnectStatus() returns disconnected
                Thread.Sleep(3000);
            }
        }

        const int RAS_MaxEntryName = 256;
        const int RAS_MaxDeviceType = 16;
        const int RAS_MaxDeviceName = 128;

        const int ERROR_BUFFER_TOO_SMALL = 603;

        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
        private struct RASCONN
        {
            public uint dwSize;

            public IntPtr hrasconn;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxEntryName + 1)]
            public string szEntryName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceType + 1)]
            public string szDeviceType;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceName + 1)]
            public string szDeviceName;

            // We don't care about other feilds.
        }

        [DllImport("Rasapi32.dll", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = false)]
        private static extern int RasEnumConnections([In, Out] RASCONN[] connections, ref uint cb, ref uint count);

        [DllImport("Rasapi32.dll", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = false)]
        private static extern int RasHangUp(IntPtr rasconn);
    }
}
