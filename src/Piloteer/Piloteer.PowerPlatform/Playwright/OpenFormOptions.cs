using System.Text;
using Microsoft.Xrm.Sdk;
using System.Web;

namespace Piloteer.PowerPlatform.Playwright
{
    public class OpenFormOptions
    {
        private readonly string _baseUrl;

        public EntityReference? Parent { get; set; }
        public string EntityName { get; }
        public Guid? EntityId { get; }
        public Guid? FormId { get; set; }

        public OpenFormOptions(IAppSettingsProvider appSettingsProvider, EntityReference recordToOpen)
            : this(appSettingsProvider, recordToOpen.LogicalName)
        {
            EntityId = recordToOpen.Id;
        }

        public OpenFormOptions(IAppSettingsProvider appSettingsProvider, Entity recordToOpen)
            : this(appSettingsProvider, recordToOpen.LogicalName)
        {
            EntityId = recordToOpen.Id;
        }

        public OpenFormOptions(IAppSettingsProvider appSettingsProvider, string entityName)
        {
            EntityName = entityName;
            _baseUrl = appSettingsProvider.GetRequiredAppSettingsValue("PowerPlatform:Url");
            if (_baseUrl.EndsWith('/'))
                _baseUrl = _baseUrl.Substring(0, _baseUrl.Length - 1);
        }

        public string GetUrl(Guid? appId)
        {
            StringBuilder builder = new($"{_baseUrl}/main.aspx?etn={EntityName}&pagetype=entityrecord&flags=testmode=true");

            if (EntityId.HasValue)
                builder.Append($"&id=%7B{EntityId:D}%7D");
            if (appId.HasValue)
                builder.Append($"&appid={appId:D}");

            StringBuilder extraQueryParameters = new StringBuilder();
            if (Parent != null)
            {
                AppendQueryParameter(extraQueryParameters, "parentrecordid", Parent.Id);
                AppendQueryParameter(extraQueryParameters, "parentrecordname", Parent.Name);
                AppendQueryParameter(extraQueryParameters, "parentrecordtype", Parent.LogicalName);
            }
            if (FormId != null)
            {
                AppendQueryParameter(extraQueryParameters, "formid", FormId);
            }
            if (extraQueryParameters.Length > 0)
            {
                builder.Append("&extraqs=");
                builder.Append(HttpUtility.UrlEncode(extraQueryParameters.ToString()));
            }
            return builder.ToString();
        }

        private static void AppendQueryParameter(StringBuilder builder, string key, object value)
        {
            if (builder.Length > 0)
            {
                builder.Append('&');
            }

            builder.Append(key);
            builder.Append('=');
            builder.Append(value);
        }
    }
}
