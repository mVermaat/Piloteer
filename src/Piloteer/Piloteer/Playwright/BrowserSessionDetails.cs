using Microsoft.Playwright;

namespace Piloteer.Playwright
{
    internal class BrowserSessionDetails
    {
        public BrowserSessionDetails(IBrowserContext browserContext)
        {
            BrowserContext = browserContext;
            SessionData = [];
        }

        public IBrowserContext BrowserContext { get; }
        public Dictionary<string, object> SessionData { get; }
    }
}
