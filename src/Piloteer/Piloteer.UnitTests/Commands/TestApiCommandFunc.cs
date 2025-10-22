using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Piloteer.Commands;

namespace Piloteer.UnitTests.Commands
{

    internal class TestApiCommandFunc : ApiOnlyCommandFunc<bool>
    {
        protected override Task<bool> ExecuteApiAsync()
        {
            return Task.FromResult(true);
        }
    }
}
