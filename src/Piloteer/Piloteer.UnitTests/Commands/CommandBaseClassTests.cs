using Piloteer.Commands;
using Piloteer.UnitTests.Mocks;
using NSubstitute;
using System.Runtime.CompilerServices;

namespace Piloteer.UnitTests.Commands
{
    public class CommandBaseClassTests
    {
        private readonly IAppSettingsProvider _appSettingsProvider;

        public CommandBaseClassTests()
        {
            _appSettingsProvider = Substitute.For<IAppSettingsProvider>();
        }

        [Fact]
        public async Task ApiCommandShouldExecute()
        {
            var processor = new CommandProcessor(new ReqnrollOutputHelperMock());
            var command = new TestApiCommand();
            await processor.ExecuteCommandAsync(command);
            Assert.True(command.Executed);
        }

        [Theory]
        [InlineData("API", true, false, null)]
        [InlineData("api", true, false, null)]
        [InlineData("Api", true, false, null)]
        [InlineData("CHROME", false, true, null)]
        [InlineData("chrome", false, true, null)]
        [InlineData("Chrome", false, true, null)]
        [InlineData("Different", false, false, 3)]
        [InlineData(null, false, false, 3)]
        public async Task MixedCommandShouldExecuteCorrectPath(string target, bool apiExecuted, bool browserExecuted, int? errorCode)
        {
            var processor = new CommandProcessor(new ReqnrollOutputHelperMock());
            _appSettingsProvider.GetRequiredAppSettingsValue(Arg.Any<string>()).Returns(target);

            var command = new TestMixedCommand(_appSettingsProvider);
            try
            {
                await processor.ExecuteCommandAsync(command);
                Assert.Equal(apiExecuted, command.ApiExecuted);
                Assert.Equal(browserExecuted, command.BrowserExecuted);
                Assert.Null(errorCode);
            }
            catch (TestExecutionException ex)
            {
                Assert.NotNull(errorCode);
                Assert.Equal(errorCode, ex.ErrorCode);
            }
        }

        [Theory]
        [InlineData("API", false, 5)]
        [InlineData("api", false, 5)]
        [InlineData("Api", false, 5)]
        [InlineData("CHROME", true, null)]
        [InlineData("chrome", true, null)]
        [InlineData("Chrome", true, null)]
        [InlineData("Different", false, 3)]
        [InlineData(null, false, 3)]
        public async Task BrowserCommandShouldExecuteCorrectPath(string target, bool executed, int? errorCode)
        {
            var processor = new CommandProcessor(new ReqnrollOutputHelperMock());
            _appSettingsProvider.GetRequiredAppSettingsValue(Arg.Any<string>()).Returns(target);

            var command = new TestBrowserCommand(_appSettingsProvider);
            try
            {
                await processor.ExecuteCommandAsync(command);
                Assert.Equal(executed, command.Executed);
                Assert.Null(errorCode);
            }
            catch (TestExecutionException ex)
            {
                Assert.NotNull(errorCode);
                Assert.Equal(errorCode, ex.ErrorCode);
            }
        }

    }
}
