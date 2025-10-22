namespace Piloteer.Commands
{
    public abstract class MixedCommand : ICommand
    {
        public MixedCommand(IAppSettingsProvider appSettingsProvider)
        {
            AppSettingsProvider = appSettingsProvider;
        }

        protected IAppSettingsProvider AppSettingsProvider { get; }

        public Task ExecuteAsync()
        {
            var target = TestTargetLoader.Get(AppSettingsProvider);
            switch (target)
            {
                case TestTarget.Chrome:
                    return ExecuteBrowserAsync();
                case TestTarget.API:
                    return ExecuteApiAsync();
                default:
                    throw new TestExecutionException(Constants.ErrorCodes.UnknownTarget, target);
            }
        }

        protected abstract Task ExecuteApiAsync();
        protected abstract Task ExecuteBrowserAsync();
    }

    public abstract class MixedCommandFunc<TResult> : ICommandFunc<TResult>
    {
        public MixedCommandFunc(IAppSettingsProvider appSettingsProvider)
        {
            AppSettingsProvider = appSettingsProvider;
        }

        protected IAppSettingsProvider AppSettingsProvider { get; }

        public Task<TResult> ExecuteAsync()
        {
            var target = TestTargetLoader.Get(AppSettingsProvider);
            switch (target)
            {
                case TestTarget.Chrome:
                    return ExecuteBrowserAsync();
                case TestTarget.API:
                    return ExecuteApiAsync();
                default:
                    throw new TestExecutionException(Constants.ErrorCodes.UnknownTarget, target);
            }
        }

        protected abstract Task<TResult> ExecuteApiAsync();
        protected abstract Task<TResult> ExecuteBrowserAsync();
    }
}
