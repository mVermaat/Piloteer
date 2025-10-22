using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Xrm.Sdk.Metadata;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Playwright.Actions;
using Piloteer.PowerPlatform.Playwright.Fields;
using Reqnroll.Assist;

namespace Piloteer.PowerPlatform.Models
{
    public class FormMetadata
    {
        private readonly EntityMetadata _entityMetadata;
        private readonly SystemForm _form;
        private Dictionary<string, List<Playwright.Fields.FormControl>> _parsedForm;

        public FormMetadata(EntityMetadata entityMetadata, SystemForm form)
        {
            _entityMetadata = entityMetadata;
            _parsedForm = [];
            _form = form;
        }

        public async Task ParseForm(IDataverseService service, IAppSettingsProvider appSettingsProvider, IPage page, IPowerPlatformActionFactory actionFactory, IActionProcessor actionProcessor)
        {
            _parsedForm = await FormXmlParser.ParseForm(service, appSettingsProvider, page, actionFactory, actionProcessor, _form, _entityMetadata);
        }

        public Playwright.Fields.FormControl? GetControl(string attributeName)
        {
            if (!_parsedForm.TryGetValue(attributeName, out var controls) || controls.Count == 0)
                return null;

            return controls.First();
        }
    }
}
