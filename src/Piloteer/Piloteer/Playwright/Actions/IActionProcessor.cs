using Microsoft.Playwright;

namespace Piloteer.Playwright.Actions
{
    public interface IActionProcessor
    {
        Task ExecuteActionAsync(IPage page, IPlaywrightAction action);
        Task<T> ExecuteActionAsync<T>(IPage page, IPlaywrightAction<T> action);

    }
}