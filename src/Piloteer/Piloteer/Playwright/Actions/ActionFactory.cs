using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloteer.Playwright.Actions
{
    public class ActionFactory : IActionFactory
    {
        public virtual MicrosoftAccountLoginAction GetMicrosoftAccountLoginAction(string username, string password, string? mfaKey)
            => new MicrosoftAccountLoginAction(username, password, mfaKey);
    }
}
