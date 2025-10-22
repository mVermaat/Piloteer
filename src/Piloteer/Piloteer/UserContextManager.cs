using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reqnroll;

namespace Piloteer
{
    [Binding]
    public class UserContextManager : IUserContextManager
    {
        private readonly IAppSettingsProvider _appSettingsProvider;
        private readonly IReqnrollOutputHelper _logger;

        public event UserContextChangedEventHandler? OnUserContextChanged;

        public delegate void UserContextChangedEventHandler(UserProfile userProfile);

        public UserContextManager(IAppSettingsProvider appSettingsProvider, IReqnrollOutputHelper logger)
        {
            _appSettingsProvider = appSettingsProvider;
            _logger = logger;
        }

        [Given("a logged in ([^\\s]+)")]
        public void ChangeUserContext(string profile)
        {
            var userProfile = _appSettingsProvider.GetRequiredAppSettingsArray<UserProfile>("UserProfiles")?
                .FirstOrDefault(x => string.Equals(x.Profile, profile, StringComparison.OrdinalIgnoreCase));

            if (userProfile == null)
                throw new TestExecutionException(Constants.ErrorCodes.UserProfileNotFound, profile);
            else if (string.IsNullOrWhiteSpace(userProfile.Username))
                throw new TestExecutionException(Constants.ErrorCodes.UserProfileWithoutUsername, profile);

            var username = userProfile.Username;
            _logger.WriteLine($"Changing user context to profile '{profile}'. Resulting in change to user {username}");
            OnUserContextChanged?.Invoke(userProfile);            
        }
    }
}
