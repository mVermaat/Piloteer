using Piloteer.Commands;
using Piloteer.UnitTests.Mocks;
using NSubstitute;

namespace Piloteer.UnitTests.Commands
{
    public class CommandFuncBaseClassTests
    {
        private readonly IAppSettingsProvider _appSettingsProvider;

        public CommandFuncBaseClassTests()
        {
            _appSettingsProvider = Substitute.For<IAppSettingsProvider>();
        }

        [Fact]
        public async Task ApiCommandShouldExecute()
        {
            var processor = new CommandProcessor(new ReqnrollOutputHelperMock());
            var command = new TestApiCommandFunc();

            Assert.True(await processor.ExecuteCommandAsync(command));
        }

        [Theory]
        [InlineData("API", true, null)]
        [InlineData("api", true, null)]
        [InlineData("Api", true, null)]
        [InlineData("CHROME", false, null)]
        [InlineData("chrome", false, null)]
        [InlineData("Chrome", false, null)]
        [InlineData("Different", null, 3)]
        [InlineData(null, null, 3)]
        public async Task MixedCommandShouldExecuteCorrectPath(string target, bool? expectedResult, int? errorCode)
        {
            var processor = new CommandProcessor(new ReqnrollOutputHelperMock());
            _appSettingsProvider.GetRequiredAppSettingsValue(Arg.Any<string>()).Returns(target);

            var command = new TestMixedCommandFunc(_appSettingsProvider);
            try
            {
                Assert.Equal(expectedResult, await processor.ExecuteCommandAsync(command));
                Assert.Null(errorCode);
            }
            catch (TestExecutionException ex)
            {
                Assert.NotNull(errorCode);
                Assert.Equal(errorCode, ex.ErrorCode);
            }
        }

        [Theory]
        [InlineData("API", 5)]
        [InlineData("api",5)]
        [InlineData("Api", 5)]
        [InlineData("CHROME", null)]
        [InlineData("chrome", null)]
        [InlineData("Chrome", null)]
        [InlineData("Different", 3)]
        [InlineData(null, 3)]
        public async Task BrowserCommandShouldExecuteCorrectPath(string target, int? errorCode)
        {
            var processor = new CommandProcessor(new ReqnrollOutputHelperMock());
            _appSettingsProvider.GetRequiredAppSettingsValue(Arg.Any<string>()).Returns(target);

            var command = new TestBrowserCommandFunc(_appSettingsProvider);
            try
            {                
                Assert.Equal(true, await processor.ExecuteCommandAsync(command));
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
