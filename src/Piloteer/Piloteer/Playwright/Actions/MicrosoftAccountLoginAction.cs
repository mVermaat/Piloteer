using Microsoft.Playwright;
using OtpNet;

namespace Piloteer.Playwright.Actions
{
    public class MicrosoftAccountLoginAction : IPlaywrightAction
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string? _mfaKey;

        public MicrosoftAccountLoginAction(string username, string password, string? mfaKey)
        {
            _username = username;
            _password = password;
            _mfaKey = mfaKey;
        }

        public async Task<ActionResult> ExecuteAsync(IPage page)
        {

            // username
            var usernameLocator = XPathProvider.GetLocator(page, Constants.XPaths.MicrosoftLoginUsername);
            var nextLocator = XPathProvider.GetLocator(page, Constants.XPaths.MicrosoftLoginNext);
            await usernameLocator.FillAsync(_username);
            await nextLocator.ClickAsync();

            // password - may support certificate auth at some point
            var passwordLocator = XPathProvider.GetLocator(page, Constants.XPaths.MicrosoftLoginPassword);
            await passwordLocator.FillAsync(_password);
            await nextLocator.ClickAsync();

            // MFA
            if(!string.IsNullOrEmpty(_mfaKey))
            {
                var mfaLocator = XPathProvider.GetLocator(page, Constants.XPaths.MicrosoftLoginTOTPInput);
                var mfaVerify = XPathProvider.GetLocator(page, Constants.XPaths.MicrosoftLoginTOTPVerify);
                await mfaLocator.FillAsync(GenerateOneTimeCode(_mfaKey));
                await mfaVerify.ClickAsync();
            }

            // stay signed in
            await nextLocator.ClickAsync();

            return ActionResult.Success();
        }

        private static string GenerateOneTimeCode(string key)
        { 
            byte[] base32Bytes = Base32Encoding.ToBytes(key);

            var totp = new Totp(base32Bytes);
            return totp.ComputeTotp(); 
        }
    }
}
