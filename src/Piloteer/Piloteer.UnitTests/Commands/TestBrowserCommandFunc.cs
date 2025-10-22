using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Piloteer.Commands;

namespace Piloteer.UnitTests.Commands
{
    internal class TestBrowserCommandFunc : BrowserOnlyCommandFunc<bool>
    {
        public TestBrowserCommandFunc(IAppSettingsProvider appSettingsProvider)
            : base(appSettingsProvider)
        {
        }


        protected override Task<bool> ExecuteBrowserAsync()
        {
            return Task.FromResult(true);
        }
    }
}
