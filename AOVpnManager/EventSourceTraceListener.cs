using System.Diagnostics;

namespace AOVpnManager
{
    public class EventSourceTraceListener : TraceListener
    {
        private readonly ILogger logger;

        public EventSourceTraceListener(ILogger logger) : base()
        {
            this.logger = logger;
        }

        public override void Write(string message)
        {
            logger.Trace(message);
        }

        public override void WriteLine(string message)
        {
            logger.Trace(message);
        }
    }
}
