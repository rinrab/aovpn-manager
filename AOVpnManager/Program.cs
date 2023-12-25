using System;

namespace AOVpnManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            MinimalEventSource.Log.Started();
            int exitCode = 0;

            try
            {
                Console.WriteLine("Hello, world!");
            }
            catch (Exception ex)
            {
                MinimalEventSource.Log.Exception(ex.ToString());
            }
            finally
            {
                MinimalEventSource.Log.Finished(exitCode);
            }
        }
    }
}
