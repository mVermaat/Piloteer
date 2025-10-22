using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Piloteer.Playwright.Actions
{
    public interface IPlaywrightAction
    {
        Task<ActionResult> ExecuteAsync(IPage page);
    }

    public interface IPlaywrightAction<T>
    {
        Task<ActionResult<T>> ExecuteAsync(IPage page);
    }
}
