using Piloteer.UnitTests.Mocks;
using NSubstitute;
using Reqnroll.BoDi;

namespace Piloteer.UnitTests.AppSettings
{
    public class AppSettingsTests
    {
        private IObjectContainer _containerMock;

        public AppSettingsTests()
        {
            _containerMock = Substitute.For<IObjectContainer>();
        }

        [Fact]
        public void ValidateArrayCanBeRetrievedFromAppSettingsViaRequired()
        {
            var provider = new AppSettingsProvider(new ReqnrollOutputHelperMock(), new AppSettingsExtender(_containerMock));
            provider.LoadAppSettings();

            var profiles = provider.GetRequiredAppSettingsArray<UserProfile>("UserProfiles");

            Assert.NotNull(profiles);
            Assert.Equal(profiles.Length, 2);
            Assert.Equal(profiles[0].Profile, "Field Service Engineer");
            Assert.Equal(profiles[0].Username, "User1");
        }

        [Fact]
        public void ValidateArrayCanBeRetrievedFromAppSettingsViaOptional()
        {
            var provider = new AppSettingsProvider(new ReqnrollOutputHelperMock(), new AppSettingsExtender(_containerMock));
            provider.LoadAppSettings();

            var profiles = provider.GetOptionalAppSettingsArray<UserProfile>("UserProfiles");

            Assert.NotNull(profiles);
            Assert.Equal(profiles.Length, 2);
            Assert.Equal(profiles[0].Profile, "Field Service Engineer");
            Assert.Equal(profiles[0].Username, "User1");
        }

        [Fact]
        public void AppSettingsProviderRequiredThrowsExceptionWhenConfigIsntLoaded()
        {
            var provider = new AppSettingsProvider(new ReqnrollOutputHelperMock(), new AppSettingsExtender(_containerMock));
            try
            {
                provider.GetRequiredAppSettingsValue("TestKey");
            }
            catch (TestExecutionException ex)
            {
                Assert.Equal(2, ex.ErrorCode);
            }
        }

        [Fact]
        public void AppSettingsProviderOptionalThrowsExceptionWhenConfigIsntLoaded()
        {
            var provider = new AppSettingsProvider(new ReqnrollOutputHelperMock(), new AppSettingsExtender(_containerMock));
            try
            {
                provider.GetOptionalAppSettingsValue("TestKey");
            }
            catch (TestExecutionException ex)
            {
                Assert.Equal(2, ex.ErrorCode);
            }
        }
    }
}
