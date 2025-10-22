using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xrm.Sdk.Query;
using Piloteer.Commands;

namespace Piloteer.PowerPlatform.Commands
{
    public class SetModelAppCommand : ApiOnlyCommandFunc<Guid>
    {
        private readonly IPowerPlatformTestingContext _context;
        private readonly string _modelAppName;

        public SetModelAppCommand(IPowerPlatformTestingContext context, string modelAppName)
        {
            _context = context;
            _modelAppName = modelAppName;
        }

        protected override async Task<Guid> ExecuteApiAsync()
        {
            var modelApp = (await _context.PowerPlatformConnection.Service.RetrieveMultipleAsync(new QueryExpression("appmodule")
            {
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("uniquename", ConditionOperator.Equal, _modelAppName)
                    }
                }
            })).Entities.FirstOrDefault();

            if (modelApp == null)
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.ModelAppNotFound, _modelAppName);

            _context.CurrentModelAppId = modelApp.Id;
            return modelApp.Id;
        }
    }
}
