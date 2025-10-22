using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.UnitTests.Mocks;
using Reqnroll.BoDi;

namespace Piloteer.UnitTests
{
    public class BrowserSessionTest
    {
        private readonly IReqnrollOutputHelper _logger;
        private readonly IObjectContainer _container;
        private readonly ISecretProvider _secretProvider;
        private readonly UserContextManager _userContextManager;
        private readonly AppSettingsProvider _appSettingsProvider;
        private readonly IActionFactory _actionFactory;
        private readonly IActionProcessor _actionProcessor;
        private readonly IBrowserSession _session;

        public BrowserSessionTest()
        {
            _logger = new ReqnrollOutputHelperMock();
            _container = Substitute.For<IObjectContainer>();
            
            _actionFactory = new ActionFactory();
            _actionProcessor = new ActionProcessor(_logger);

            _appSettingsProvider = new AppSettingsProvider(_logger, new AppSettingsExtender(_container));
            _appSettingsProvider.LoadAppSettings();

            _secretProvider = new SecretProvider(_appSettingsProvider);
            _userContextManager = new UserContextManager(_appSettingsProvider, _logger);
            _session = new BrowserSession(_userContextManager, _appSettingsProvider, _actionFactory, _actionProcessor, _secretProvider);
        }

        [Fact]
        public async Task TestLoggingInWithMicrosoft()
        {
            _userContextManager.ChangeUserContext("Salesperson");

            var page = await _session.GetCurrentPage();
            await page.WaitForURLAsync("https://portal.azure.com/#home");
        }
    }
}
