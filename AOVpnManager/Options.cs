using System;
using System.Collections.Generic;

namespace AOVpnManager
{
    public class Options
    {
        public bool IsConsole { get; set; }

        public static Options ReadFromArgs(string args)
        {
            // TODO: quoted strings?
            return ReadFromArgs(args.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static Options ReadFromArgs(string[] args)
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
                    }
                }
                else
                {
                    flags.Add(args[i].ToLower());
                }
            }

            return new Options
            {
                IsConsole = parameters.GetValueOrDefault("/log", "event") == "console" || flags.Contains("/console")
            };
        }
    }
}
