using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Piloteer.Playwright
{
    internal static class BrowserManager
    {
        private static readonly Lazy<Task<IBrowser>> _browser;
        private static readonly ConcurrentPool<string, BrowserSessionDetails> _browserPool;

        static BrowserManager()
        {
            _browser = new Lazy<Task<IBrowser>>(CreateBrowserAsync, false);
            _browserPool = new ConcurrentPool<string, BrowserSessionDetails>(CreateNewSessionAsync);
        }

        internal static async Task Cleanup()
        {
            var list = _browserPool.GetAllItems();
            foreach (var browser in list)
            {
                await browser.BrowserContext.CloseAsync();
            }

        }

        public static Task<(BrowserSessionDetails session, bool newSession)> GetSession(string identifier)
        {
            return _browserPool.GetItemAsync(identifier);
        }

        public static void CompleteSession(string identifier, BrowserSessionDetails session)
        {
            _browserPool.AddItem(identifier, session);
        }

        private static async Task<BrowserSessionDetails> CreateNewSessionAsync(string username)
        {
            var browser = await _browser.Value;
            return new BrowserSessionDetails(await browser.NewContextAsync());
        }

        private static async Task<IBrowser> CreateBrowserAsync()
        {
            var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            return await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
        }
    }
}
