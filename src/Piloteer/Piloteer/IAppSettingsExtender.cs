using Microsoft.Extensions.Configuration;

namespace Piloteer
{
    public interface IAppSettingsExtender
    {
        IConfigurationBuilder Extend(IConfigurationBuilder builder);
    }
}
