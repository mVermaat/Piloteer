namespace Piloteer.Playwright.Actions
{
    public interface IActionFactory
    {
        MicrosoftAccountLoginAction GetMicrosoftAccountLoginAction(string username, string password, string? mfaKey);
    }
}