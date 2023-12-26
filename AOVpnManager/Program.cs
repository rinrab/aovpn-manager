using System;

namespace AOVpnManager
{
    public class Program
    {
        static int Main(string[] args)
        {
            MinimalEventSource.Log.Started();
            int exitCode = 1;

            try
            {
                Console.WriteLine("Hello, world!");

                throw new Exception("hello world");
            }
            catch (Exception ex)
            {
                MinimalEventSource.Log.Exception(ex.Message, ex.StackTrace);

                exitCode = 1;
            }

            MinimalEventSource.Log.Finished(exitCode);

            return exitCode;
        }
    }
}
