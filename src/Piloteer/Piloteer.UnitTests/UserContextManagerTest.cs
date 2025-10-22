using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Piloteer.UnitTests.Mocks;
using NSubstitute;

namespace Piloteer.UnitTests
{
    public class UserContextManagerTest
    {
        [Theory]
        [InlineData("Salesperson")]
        [InlineData("salesperson")]
        [InlineData("SALESPERSON")]
        public void ValidateEventRaised(string profile)
        {
            // Arrange
            var appSettingsProviderMock = Substitute.For<IAppSettingsProvider>();
            appSettingsProviderMock.GetRequiredAppSettingsArray<UserProfile>("UserProfiles").Returns([new() { Profile = "Salesperson", Username = "user1" }]);
            var userContextManager = new UserContextManager(appSettingsProviderMock, new ReqnrollOutputHelperMock());

            string? raisedProfile = null;
            string? raisedUsername = null;
            userContextManager.OnUserContextChanged += (userProfile) =>
            {
                raisedProfile = userProfile.Profile;
                raisedUsername = userProfile.Username;
            };

            // Act
            userContextManager.ChangeUserContext(profile);

            // Assert
            Assert.Equal("user1", raisedUsername);
            Assert.Equal("Salesperson", raisedProfile);
        }

        [Fact]
        public void ValidateEventNotRaisedForUnknownProfile()
        {
            // Arrange
            var appSettingsProviderMock = Substitute.For<IAppSettingsProvider>();
            var userContextManager = new UserContextManager(appSettingsProviderMock, new ReqnrollOutputHelperMock());
            
            userContextManager.OnUserContextChanged += (userProfile) => Assert.Fail("OnUserContextChanged shouldn't get raised with an unknown profile");
           
            // Act & Assert
            Assert.Throws<TestExecutionException>(() => userContextManager.ChangeUserContext("UnknownProfile"));
        }

        [Fact]
        public void ValidateEventNotRaisedForProfileWithoutUsername()
        {
            // Arrange
            var appSettingsProviderMock = Substitute.For<IAppSettingsProvider>();
            appSettingsProviderMock.GetRequiredAppSettingsArray<UserProfile>("UserProfiles").Returns([new() { Profile = "Salesperson" }]);
            var userContextManager = new UserContextManager(appSettingsProviderMock, new ReqnrollOutputHelperMock());

            userContextManager.OnUserContextChanged += (userProfile) => Assert.Fail("OnUserContextChanged shouldn't get raised with a profile without username");

            // Act & Assert
            Assert.Throws<TestExecutionException>(() => userContextManager.ChangeUserContext("Salesperson"));
        }

        [Fact]
        public void ValidateNoExceptionIfThrownIfNoEventHandlerRegistered()
        {
            // Arrange
            var appSettingsProviderMock = Substitute.For<IAppSettingsProvider>();
            appSettingsProviderMock.GetRequiredAppSettingsArray<UserProfile>("UserProfiles").Returns([new() { Profile = "Salesperson", Username = "user1" }]);
            var userContextManager = new UserContextManager(appSettingsProviderMock, new ReqnrollOutputHelperMock());
            
            // Act & Assert
            userContextManager.ChangeUserContext("Salesperson");
        }
    }
}
