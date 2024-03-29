﻿using System.Diagnostics.Tracing;

namespace AOVpnManager
{
    [EventSource(Name = "AOVpnManager", Guid = "73C709D4-5F64-44FD-8402-E4D25FDC90CC")]
    public sealed class EventSourceLogger : EventSource, ILogger
    {
        [Event(1, Level = EventLevel.Informational, Channel = EventChannel.Admin, Message = "AOVpnManager started")]
        public void Started()
        {
            WriteEvent(1);
        }

        [Event(2, Level = EventLevel.Informational, Channel = EventChannel.Admin, Message = "AOVpnManager finished with code {0}")]
        public void Finished(int code)
        {
            WriteEvent(2, code);
        }

        [Event(3, Level = EventLevel.Error, Channel = EventChannel.Admin, Message = "Unhandled exception: {0}")]
        public void Exception(string message, string stackTrace)
        {
            WriteEvent(3, message, stackTrace);
        }

        [Event(4, Level = EventLevel.Informational, Channel = EventChannel.Admin, Message = "Vpn connection \"{0}\" created.")]
        public void VpnConnectionCreated(string connectionName)
        {
            WriteEvent(4, connectionName);
        }

        [Event(6, Level = EventLevel.Informational, Channel = EventChannel.Admin, Message = "Vpn connection \"{0}\" deleted.")]
        public void VpnConnectionDeleted(string connectionName)
        {
            WriteEvent(6, connectionName);
        }

        [Event(7, Level = EventLevel.Informational, Channel = EventChannel.Debug, Message = "{0}")]
        public void Trace(string message)
        {
            WriteEvent(7, message);
        }
    }
}
