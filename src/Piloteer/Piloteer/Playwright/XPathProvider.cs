using Microsoft.Playwright;

namespace Piloteer.Playwright
{
    public static class XPathProvider
    {
        private static readonly Dictionary<string, string> _xpaths;

        static XPathProvider()
        {
            _xpaths = [];
            FillDictionary();
        }

        private static void FillDictionary()
        {
            UpsertXPath(Constants.XPaths.MicrosoftLoginUsername, "//input[@name='loginfmt']");
            UpsertXPath(Constants.XPaths.MicrosoftLoginPassword, "//input[@name='passwd']");
            UpsertXPath(Constants.XPaths.MicrosoftLoginNext, "//input[@id='idSIButton9']");
            UpsertXPath(Constants.XPaths.MicrosoftLoginTOTPInput, "//input[@id='idTxtBx_SAOTCC_OTC']");
            UpsertXPath(Constants.XPaths.MicrosoftLoginTOTPVerify, "//input[@id='idSubmit_SAOTCC_Continue']");
        }

        /// <summary>
        /// Upserts an xpath
        /// </summary>
        /// <param name="key">Key of the xpath</param>
        /// <param name="xpath">XPath value</param>
        public static void UpsertXPath(string key, string xpath)
        {
            _xpaths[key] = xpath;
        }

        /// <summary>
        /// Gets a locator for the given key and formats it with the provided arguments
        /// </summary>
        /// <param name="page">Page for the locator</param>
        /// <param name="key">XPath key</param>
        /// <param name="formatArgs">Format for the xpath</param>
        /// <returns>Locator for the found xpath</returns>
        /// <exception cref="TestExecutionException">Throws an error if the xpath key doesn't exist</exception>
        public static ILocator GetLocator(IPage page, string key, params string[] formatArgs)
        {
            if(!_xpaths.ContainsKey(key))
                throw new TestExecutionException(Constants.ErrorCodes.XPathNotFound, key);
           
            return page.Locator(string.Format(_xpaths[key], formatArgs));
        }

        /// <summary>
        /// Gets a locator for the given key and formats it with the provided arguments
        /// </summary>
        /// <param name="locator">Parent locator</param>
        /// <param name="key">XPath key</param>
        /// <param name="formatArgs">Format for the xpath</param>
        /// <returns>Locator for the found xpath</returns>
        /// <exception cref="TestExecutionException">Throws an error if the xpath key doesn't exist</exception>
        public static ILocator GetLocator(ILocator locator, string key, params string[] formatArgs)
        {
            if (!_xpaths.ContainsKey(key))
                throw new TestExecutionException(Constants.ErrorCodes.XPathNotFound, key);

            return locator.Locator(string.Format(_xpaths[key], formatArgs));
        }
    }
}
