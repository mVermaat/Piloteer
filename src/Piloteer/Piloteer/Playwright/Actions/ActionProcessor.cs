using Microsoft.Playwright;
using Reqnroll;

namespace Piloteer.Playwright.Actions
{
    public class ActionProcessor : IActionProcessor
    {
        private readonly IReqnrollOutputHelper _logger;

        public ActionProcessor(IReqnrollOutputHelper logger)
        {
            _logger = logger;
        }

        public async Task ExecuteActionAsync(IPage page, IPlaywrightAction action)
        {
            _logger.WriteLine($"Executing action: {action.GetType().Name}");
            await InternalExecuteActionAsync(page, action, 3);
        }

        public Task<T> ExecuteActionAsync<T>(IPage page, IPlaywrightAction<T> action)
        {
            return InternalExecuteActionAsync(page, action, 3);
        }

        private async Task InternalExecuteActionAsync(IPage page, IPlaywrightAction action, int retries)
        {
            _logger.WriteLine($"Executing action: {action.GetType().Name}. Retries left: {retries}");
            var result = await action.ExecuteAsync(page);

            if (!result.IsSuccessfull)
            {
                if (retries > 0 && result.AllowRetry)
                {
                    _logger.WriteLine($"Action {action.GetType().Name} failed with error {ErrorCodes.GetErrorMessage(result.ErrorCode, result.ErrorMessageFormatArgs)}. Retrying after a second.");
                    await Task.Delay(1000);
                    await InternalExecuteActionAsync(page, action, retries - 1);
                }
                else
                {
                    throw new TestExecutionException(result.ErrorCode, result.ErrorMessageFormatArgs);
                }
            }

        }

        private async Task<T> InternalExecuteActionAsync<T>(IPage page, IPlaywrightAction<T> action, int retries)
        {
            _logger.WriteLine($"Executing action: {action.GetType().Name}. Retries left: {retries}");
            var result = await action.ExecuteAsync(page);

            if(result.IsSuccessfull)
            {
                if (result.Result == null)
                    throw new TestExecutionException(Constants.ErrorCodes.ActionWithoutResult, action.GetType().Name);

                return result.Result;
            }
            else
            {
                if (retries > 0 && result.AllowRetry)
                {
                    _logger.WriteLine($"Action {action.GetType().Name} failed with error {ErrorCodes.GetErrorMessage(result.ErrorCode, result.ErrorMessageFormatArgs)}. Retrying after a second.");
                    await Task.Delay(1000);
                    return await InternalExecuteActionAsync(page, action, retries - 1);
                }
                else
                {
                    throw new TestExecutionException(result.ErrorCode, result.ErrorMessageFormatArgs);
                }
            }

        }
    }
}
