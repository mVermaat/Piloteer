namespace Piloteer.Commands
{
    public abstract class BrowserOnlyCommand : ICommand
    {
        public BrowserOnlyCommand(IAppSettingsProvider appSettingsProvider)
        {
            AppSettingsProvider = appSettingsProvider;
        }

        protected IAppSettingsProvider AppSettingsProvider { get; }

        public async Task ExecuteAsync()
        {
            var target = TestTargetLoader.Get(AppSettingsProvider);
            switch (target)
            {
                case TestTarget.Chrome:
                    await ExecuteBrowserAsync();
                    break;
                case TestTarget.API:
                    throw new TestExecutionException(Constants.ErrorCodes.BrowserNotSupportedForApiTesting, GetType().Name);
                default:
                    throw new TestExecutionException(Constants.ErrorCodes.UnknownTarget, target);
            }
        }

        protected abstract Task ExecuteBrowserAsync();
    }

    public abstract class BrowserOnlyCommandFunc<TResult> : ICommandFunc<TResult>
    {
        public BrowserOnlyCommandFunc(IAppSettingsProvider appSettingsProvider)
        {
            AppSettingsProvider = appSettingsProvider;
        }

        protected IAppSettingsProvider AppSettingsProvider { get; }

        public async Task<TResult> ExecuteAsync()
        {
            var target = TestTargetLoader.Get(AppSettingsProvider);
            switch (target)
            {
                case TestTarget.Chrome:
                    return await ExecuteBrowserAsync();
                case TestTarget.API:
                    throw new TestExecutionException(Constants.ErrorCodes.BrowserNotSupportedForApiTesting, GetType().Name);
                default:
                    throw new TestExecutionException(Constants.ErrorCodes.UnknownTarget, target);
            }
        }

        protected abstract Task<TResult> ExecuteBrowserAsync();
    }
}
