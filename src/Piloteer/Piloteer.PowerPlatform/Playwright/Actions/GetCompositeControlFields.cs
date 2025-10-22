using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class GetCompositeControlFields : IPlaywrightAction<IEnumerable<string>>
    {
        private readonly string _compositeControlAttributeName;

        public GetCompositeControlFields(string compositeAttributeName)
        {
            _compositeControlAttributeName = compositeAttributeName;
        }

        public async Task<ActionResult<IEnumerable<string>>> ExecuteAsync(IPage page)
        {
            var locator = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityFormCompositeControls, _compositeControlAttributeName);

            await locator.First.WaitForAsync();

            var elements = await locator.AllAsync();

            var results = new List<string>();
            foreach (var element in elements)
            {
                var dataControlName = await element.GetAttributeAsync("data-control-name");
                if (dataControlName != null)
                {
                    results.Add(dataControlName.Substring(_compositeControlAttributeName.Length + 24));
                }
            }
            return ActionResult<IEnumerable<string>>.Success(results);
        }
    }
}
