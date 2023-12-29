using System.Collections.Generic;

namespace AOVpnManager
{
    public class CommandLineArguments
    {
        public bool IsConsole { get; set; }

        public static CommandLineArguments Read(params string[] args)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            List<string> flags = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                if (i + 1 < args.Length)
                {
                    if (args[i + 1].StartsWith("/"))
                    {
                        flags.Add(args[i]);
                    }
                    else
                    {
                        parameters.Add(args[i].ToLower(), args[i + 1].ToLower());
                        i++;
                    }
                }
                else
                {
                    flags.Add(args[i].ToLower());
                }
            }

            return new CommandLineArguments
            {
                IsConsole = parameters.GetValueOrDefault("/log", "event") == "console" || flags.Contains("/console")
            };
        }
    }
}
