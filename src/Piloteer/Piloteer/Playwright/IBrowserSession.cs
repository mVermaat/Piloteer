using Microsoft.Playwright;

namespace Piloteer.Playwright
{
    public interface IBrowserSession : IDisposable
    {
        Task<IPage> GetCurrentPage();
        public void SetSetting<T>(string key, T value) where T : notnull;
        public T? GetSetting<T>(string key);

        bool IsInitialized { get; }
    }
}