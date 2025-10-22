namespace Piloteer.Commands
{
    public abstract class ApiOnlyCommand : ICommand
    {
        public Task ExecuteAsync()
        {
            return ExecuteApiAsync();
        }

        protected abstract Task ExecuteApiAsync();
    }

    public abstract class ApiOnlyCommandFunc<TResult> : ICommandFunc<TResult>
    {
        public Task<TResult> ExecuteAsync()
            => ExecuteApiAsync();

        protected abstract Task<TResult> ExecuteApiAsync();
    }
}
