using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Piloteer.Playwright;
using Reqnroll;
using Reqnroll.Infrastructure;

namespace Piloteer.PowerPlatform.Playwright
{
    [Binding]
    public class ScreenshotProvider
    {
        private readonly IBrowserSession _session;
        private readonly IReqnrollOutputHelper _logger;
        private readonly IReqnrollAttachmentHandler _attachmentHandler;

        public ScreenshotProvider(IBrowserSession session, IReqnrollOutputHelper logger, IReqnrollAttachmentHandler attachmentHandler)
        {
            _session = session;
            _logger = logger;
            _attachmentHandler = attachmentHandler;
        }

        [AfterScenario]
        public async Task CaptureScreenshotAfterScenarioAsync()
        {
            if (!_session.IsInitialized)
                return;

            var page = await _session.GetCurrentPage();
            var bytes = await page.ScreenshotAsync(new PageScreenshotOptions
            {
                FullPage = true
            });
            _logger.WriteLine(Convert.ToBase64String(bytes));
        }
    }
}
