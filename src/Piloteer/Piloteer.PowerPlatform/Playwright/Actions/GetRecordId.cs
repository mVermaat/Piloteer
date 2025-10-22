using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Web;
using Microsoft.Playwright;
using Piloteer.Playwright.Actions;
using Reqnroll;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class GetRecordId : IPlaywrightAction<Guid>
    {
        private readonly IPowerPlatformTestingContext _context;

        public GetRecordId(IPowerPlatformTestingContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<Guid>> ExecuteAsync(IPage page)
        {
            _context.Logger.WriteLine("Getting Record Id");
            var queryParams = HttpUtility.ParseQueryString(new Uri(page.Url).Query);

            if (Guid.TryParse(queryParams["id"], out var id))
            {
                _context.Logger.WriteLine($"Found current record id via url: {id}");
                return ActionResult<Guid>.Success(id);
            }

            var objectId = await page.EvaluateAsync("return Xrm.Page.data.entity.getId();");

            if (Guid.TryParse(objectId.ToString(), out id))
            {
                _context.Logger.WriteLine($"Found current record id via script: {id}");
                return ActionResult<Guid>.Success(id);
            }

            return ActionResult<Guid>.Fail(true, PowerPlatformConstants.ErrorCodes.RecordIdNotFound);
        }
    }
}
