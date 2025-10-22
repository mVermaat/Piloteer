using System.Globalization;
using Microsoft.Xrm.Sdk;

namespace Piloteer.PowerPlatform.Models
{
    public class UserSettings
    {
        private Entity _settingsEntity;

        public TimeZoneInfo TimeZoneInfo { get; }

        public NumberFormatInfo NumberFormat { get; }
        public int UILanguageId => _settingsEntity.GetAttributeValue<int>("uilanguageid");
        public Guid UserId => _settingsEntity.GetAttributeValue<Guid>("systemuserid");

        public string DateFormat => _settingsEntity.GetAttributeValue<string>("dateformatstring")
          .Replace("/", _settingsEntity.GetAttributeValue<string>("dateseparator"));

        public string TimeFormat => _settingsEntity.GetAttributeValue<string>("timeformatstring")
            .Replace(":", _settingsEntity.GetAttributeValue<string>("timeseparator"));

        public UserSettings(Entity settingsEntity, TimeZoneInfo timeZoneInfo)
        {
            _settingsEntity = settingsEntity;
            TimeZoneInfo = timeZoneInfo;
            NumberFormat= GetNumberFormatInfo();
        }

        private NumberFormatInfo GetNumberFormatInfo()
        {
            var nfi = (NumberFormatInfo)NumberFormatInfo.InvariantInfo.Clone();
            nfi.NumberDecimalSeparator = _settingsEntity.GetAttributeValue<string>("decimalsymbol");
            return nfi;
        }
    }
}
