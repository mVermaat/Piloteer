using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Metadata;

namespace Piloteer.PowerPlatform.Playwright.Fields
{
    public class FormControl
    {
        public FormControl(AttributeMetadata metadata, string controlName, string attributeName, string tabName)
        {
            Metadata = metadata;
            ControlName = controlName;
            AttributeName = attributeName;
            TabName = tabName;
        }

        public AttributeMetadata Metadata { get; }
        
        public string AttributeName { get; }
        public string ControlName { get; }
        public string TabName { get; }
    }
}
