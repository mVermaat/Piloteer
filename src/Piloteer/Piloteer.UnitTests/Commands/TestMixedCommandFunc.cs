using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Piloteer.Commands;

namespace Piloteer.UnitTests.Commands
{

    internal class TestMixedCommandFunc : MixedCommandFunc<bool>
    {
        public TestMixedCommandFunc(IAppSettingsProvider appSettingsProvider)
            : base(appSettingsProvider)
        {
        }

        protected override Task<bool> ExecuteApiAsync()
        {
            return Task.FromResult(true);
        }

        protected override Task<bool> ExecuteBrowserAsync()
        {
            return Task.FromResult(false);
        }
    }
}
