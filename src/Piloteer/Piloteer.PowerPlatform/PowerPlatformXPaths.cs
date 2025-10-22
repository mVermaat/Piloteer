using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Piloteer.Playwright;
using Reqnroll;

namespace Piloteer.PowerPlatform
{
    
    [Binding]
    public class PowerPlatformXPaths
    {
        [BeforeTestRun(Order = 1000)]
        public static void AddErrors()
        {
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.CommandBarOverflowMenu, "//ul[@data-id='OverflowFlyout']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.CopilotPane, "//div[@id='Microsoft.Copilot.Pane']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.CopilotPaneCloseButton, "//button[@data-testid='close-button']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.DuplicateDetectionGridRows, "//div[contains(@class,'data-selectable')]");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.DuplicateDetectionSaveButton, "//button[@data-id='ignore_save']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.DuplicateDetectionWindow, "//div[@data-id='ManageDuplicates']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityColumnOptionSetInput, "//button[@data-id='{0}.fieldControl-option-set-select']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.FluentItemContainer, "//div[@id='__fluentPortalMountNode']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityColumnDateTimeDateInput, "//div[@data-id='{0}.fieldControl._datecontrol-date-container']//input");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityColumnDateTimeTimeInput, "//div[@data-id='{0}.fieldControl._timecontrol-datetime-container']//input");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityColumnOptionSetValueItem, "//div[text()='{0}']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityColumnLockedIcon, "//div[@data-id='{0}-locked-icon']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityColumnTextInput, "//input");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityColumnTextArea, "//textarea");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityFieldContainer, "//div[@data-id='{0}-FieldSectionItemContainer']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityFormAssistHideButton, "//button[@data-id='FormFillBar_Hide_Button']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityFormAssistShowButton, "//button[@data-id='FormFillBar_Show_Button']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityFormCommandBar, "//ul[@data-lp-id='commandbar-Form:{0}']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityFormCommandBarButton, "//button[starts-with(@title,'{0}') and span/span/text() = '{0}']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityFormCommandBarOverflowButton, "//button[@data-lp-id='Form:{0}-OverflowButton']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityFormCompositeControls, "//div[starts-with(@data-control-name,'{0}_compositionLinkControl_')]");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityFormId, "//div[@id='navigationcontextprovider']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityFormRoot, "//div[@data-id='editFormRoot']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityFormSaveStatus, "//span[@data-id='header_saveStatus']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityFormTab, "//li[@data-id='tablist-{0}']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.EntityQuickCreateDialogRoot, "//section[@data-id='quickCreateRoot']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.ErrorDialogRoot, "//div[@data-id='errorDialogdialog']");
            XPathProvider.UpsertXPath(PowerPlatformConstants.XPaths.ErrorDialogText, "//span[@data-id='errorDialog_subtitle']");
        }
    }
}
