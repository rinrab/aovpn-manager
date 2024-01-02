using System;

namespace AOVpnManager
{
    public class CommandLineArguments
    {
        public bool IsConsole { get; set; }

        public static CommandLineArguments Parse(params string[] args)
        {
            var rv = new CommandLineArguments();

            for (int i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], "/console", StringComparison.OrdinalIgnoreCase))
                {
                    rv.IsConsole = true;
                }

                if (i + 1 < args.Length)
                {
                    if (string.Equals(args[i], "/log", StringComparison.OrdinalIgnoreCase))
                    {
                        rv.IsConsole = string.Equals(args[i + 1], "console", StringComparison.OrdinalIgnoreCase);
                        i++;
                    }
                }
            }

            return rv;
        }
    }
}
