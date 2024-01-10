using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace AOVpnManager
{
    public class Program
    {
        static int Main(string[] args)
        {
            CommandLineOptions options = CommandLineOptions.Parse(args);

            ILogger logger = CreateLogger(options.IsConsole);
            IGroupPolicyProvider policyProvider = new GroupPolicyProvider(Registry.LocalMachine, @"SOFTWARE\Policies\AOVpnManager");
            IStateManager stateManager = new StateManager(Registry.LocalMachine, @"SOFTWARE\AOVpnManager");

            Trace.Listeners.Add(new EventSourceTraceListener(logger));

            logger.Started();
            int exitCode = 0;

            try
            {
                using (IVpnManager vpnManager = VpnManager.Create())
                {
                    GpUpdateProcessor processor = new GpUpdateProcessor(vpnManager, policyProvider, stateManager, logger);

                    processor.Run();
                }
            }
            catch (Exception ex)
            {
                logger.Exception(ex.Message, ex.StackTrace);

                exitCode = 1;
            }

            logger.Finished(exitCode);

            return exitCode;
        }

        static ILogger CreateLogger(bool isConsole)
        {
            if (isConsole)
            {
                return new ConsoleLogger();
            }
            else
            {
                return new EventSourceLogger();
            }
        }
    }
}
