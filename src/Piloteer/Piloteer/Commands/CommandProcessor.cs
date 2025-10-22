using Reqnroll;

namespace Piloteer.Commands
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly IReqnrollOutputHelper _logger;

        public CommandProcessor(IReqnrollOutputHelper logger)
        {
            _logger = logger;
        }

        public Task ExecuteCommandAsync(ICommand command)
        {
            _logger.WriteLine($"Executing command: {command.GetType().Name}");
            return command.ExecuteAsync();
        }

        public Task<TResult> ExecuteCommandAsync<TResult>(ICommandFunc<TResult> command)
        {
            _logger.WriteLine($"Executing command: {command.GetType().Name}");
            return command.ExecuteAsync();
        }
    }
}
