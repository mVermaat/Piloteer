namespace Piloteer.Commands
{
    public interface ICommandProcessor
    {
        Task ExecuteCommandAsync(ICommand command);
        Task<TResult> ExecuteCommandAsync<TResult>(ICommandFunc<TResult> command);
    }
}
