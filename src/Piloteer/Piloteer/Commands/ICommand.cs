namespace Piloteer.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }

    public interface ICommandFunc<TResult>
    {
        Task<TResult> ExecuteAsync();
    }
}
