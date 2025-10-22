using Piloteer.Commands;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Reqnroll;
using Reqnroll.BoDi;

namespace Piloteer
{
    /// <summary>
    /// Class to load dependencies for the test framework.
    /// </summary>
    [Binding]
    public class DependencyLoader
    {
        private readonly IObjectContainer _objectContainer;

        public DependencyLoader(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        /// <summary>
        /// BeforeScenario hook that loads dependencies with order 1000
        /// </summary>
        [BeforeScenario(Order = 1000)]
        public void LoadDependencies()
        {
            _objectContainer.RegisterTypeAs<AppSettingsProvider, IAppSettingsProvider>();
            _objectContainer.RegisterTypeAs<CommandProcessor, ICommandProcessor>();
            _objectContainer.RegisterTypeAs<UserContextManager, IUserContextManager>();
            _objectContainer.RegisterTypeAs<ActionProcessor, IActionProcessor>();
            _objectContainer.RegisterTypeAs<BrowserSession, IBrowserSession>();
            _objectContainer.RegisterTypeAs<ActionFactory, IActionFactory>();
            _objectContainer.RegisterTypeAs<SecretProvider, ISecretProvider>();
        }
    }
}
