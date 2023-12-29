using System;

namespace AOVpnManager
{
    public class Options
    {
        public bool IsConsole { get; set; }

        public static Options ReadFromArgs(string[] args)
        {
            Options rv = new Options();

            for (int i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], "/console", StringComparison.OrdinalIgnoreCase))
                {
                    rv.IsConsole = true;
                }
            }

            return rv;
        }
    }
}
