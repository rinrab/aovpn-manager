using Microsoft.Diagnostics.Tracing;

namespace AOVpnManager
{
    [EventSource(Name = "AOVpnManager", Guid = "73C709D4-5F64-44FD-8402-E4D25FDC90CC")]
    public sealed class MinimalEventSource : EventSource
    {
        [Event(1, Level = EventLevel.Informational, Channel = EventChannel.Operational, Message = "AOVpnManager started")]
        public void Started()
        {
            WriteEvent(1);
        }

        [Event(2, Level = EventLevel.Informational, Channel = EventChannel.Operational, Message = "AOVpnManager finished with code {0}")]
        public void Finished(int code)
        {
            WriteEvent(2, code);
        }

        [Event(3, Level = EventLevel.Error, Channel = EventChannel.Admin, Message = "AOVpnManager finished with exception {0}")]
        public void Exception(string message)
        {
            WriteEvent(3, message);
        }

        public static MinimalEventSource Log = new MinimalEventSource();
    }
}
