using System;

namespace AOVpnManager
{
    public class Options
    {
        public bool IsConsole { get; set; }

        public static Options ReadFromArgs(string[] args)
        {
            return new Options()
            {
                IsConsole = args.Length > 0 && string.Equals(args[0], "/console", StringComparison.OrdinalIgnoreCase)
            };
        }
    }
}
