using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Piloteer.Playwright.Actions;
using Reqnroll;
using Reqnroll.Assist;

namespace Piloteer.Playwright
{
    [Binding]
    public class BrowserSession : IBrowserSession
    {
        private readonly IUserContextManager _userContextManager;
        private readonly IAppSettingsProvider _appSettingsProvider;
        private readonly IActionFactory _actionFactory;
        private readonly IActionProcessor _actionProcessor;
        private readonly ISecretProvider _secretProvider;
        private BrowserSessionDetails? _currentSessionDetals;
        private IPage? _currentPage;

        private bool _disposedValue;
        private UserProfile? _currentUserProfile;

        public bool IsInitialized => _currentSessionDetals != null && _currentPage != null;

        public BrowserSession(IUserContextManager userContextManager, IAppSettingsProvider appSettingsProvider, IActionFactory actionFactory, 
            IActionProcessor actionProcessor, ISecretProvider secretProvider)
        {
            _userContextManager = userContextManager;
            _appSettingsProvider = appSettingsProvider;
            _actionFactory = actionFactory;
            _actionProcessor = actionProcessor;
            _secretProvider = secretProvider;
            _userContextManager.OnUserContextChanged += ChangeUserContext;
        }


        public async Task<IPage> GetCurrentPage()
        {
            if(string.IsNullOrEmpty(_currentUserProfile?.Username) || string.IsNullOrEmpty(_currentUserProfile.Profile))
                throw new TestExecutionException(Constants.ErrorCodes.BrowserSessionNotInitialized);
            if(_disposedValue)
                throw new ObjectDisposedException(nameof(BrowserSession), "Browser session has been disposed");

            bool newSessionCreated = false;
            if (_currentSessionDetals == null) 
            {
                var (session, newSession) = await BrowserManager.GetSession(_currentUserProfile.Username);
                newSessionCreated = newSession;
                _currentSessionDetals = session;
                _currentPage = null;
            }

            if(_currentPage == null)
            {
                _currentPage = _currentSessionDetals.BrowserContext.Pages.FirstOrDefault() ?? await _currentSessionDetals.BrowserContext.NewPageAsync();
                if (newSessionCreated)
                {
                    var initalUrl = _appSettingsProvider.GetRequiredAppSettingsValue("Browser:InitialUrl");
                    await _currentPage.GotoAsync(initalUrl);

                    var password = _secretProvider.GetSecret(_currentUserProfile.Profile);
                    var mfaKey = _currentUserProfile.MFA ? _secretProvider.GetSecret($"{_currentUserProfile.Profile}MFA") : null;

                    await _actionProcessor.ExecuteActionAsync(_currentPage, _actionFactory.GetMicrosoftAccountLoginAction(_currentUserProfile.Username, password, mfaKey));
                   
                }
            }

            return _currentPage;
        }

        private void ChangeUserContext(UserProfile userProfile)
        {
            if (string.IsNullOrWhiteSpace(userProfile.Username))
                throw new TestExecutionException(Constants.ErrorCodes.UserProfileWithoutUsername, "No username provided");

            if (!string.IsNullOrEmpty(_currentUserProfile?.Username) && _currentSessionDetals != null)
                BrowserManager.CompleteSession(_currentUserProfile.Username, _currentSessionDetals);

            _currentUserProfile = userProfile;
            _currentSessionDetals = null;
            _currentPage = null;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _userContextManager.OnUserContextChanged -= ChangeUserContext;
                    
                    if (!string.IsNullOrEmpty(_currentUserProfile?.Username) && _currentSessionDetals != null)
                        BrowserManager.CompleteSession(_currentUserProfile.Username, _currentSessionDetals);
                    _currentPage = null;
                }

                _disposedValue = true;
            }
        }

        [BeforeScenario(Order = 3000)]
        public void Load()
        {
            // Ensure the constructor registers itself with the UserContextManager
        }

        public void SetSetting<T>(string key, T value)
            where T : notnull
        {
            if (_currentSessionDetals == null)
                throw new TestExecutionException(Constants.ErrorCodes.BrowserSessionNotInitialized);

            _currentSessionDetals.SessionData[key] = value;
        }

        public T? GetSetting<T>(string key)
        {
            if(_currentSessionDetals == null)
                throw new TestExecutionException(Constants.ErrorCodes.BrowserSessionNotInitialized);

            if(_currentSessionDetals.SessionData.TryGetValue(key, out var value))
                return (T?)value;
            return default;
        }
    }
}
