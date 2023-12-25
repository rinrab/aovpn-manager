using System;
using System.Diagnostics;

namespace AOVpnManager
{
    public class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Hello, world!");

            using (Process process = Process.Start("mspaint"))
            {
                process.WaitForExit();
            }

            Console.WriteLine("Goodbye, world!");

            return 0;
        }
    }
}
