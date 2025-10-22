using Microsoft.Xrm.Sdk;
using Piloteer.PowerPlatform.Models;

namespace Piloteer.PowerPlatform
{
    public static class ExtensionMethods
    {
        public static EntityReference ToEntityReference(this Entity entity, string primaryFieldAttribute)
        {
            return new EntityReference(entity.LogicalName, entity.Id) { Name = entity.GetAttributeValue<string>(primaryFieldAttribute) };
        }

        /// <summary>
        /// Gets the label text for a label in the specified language.
        /// </summary>
        /// <param name="label">Label to check.</param>
        /// <param name="lcid">Language code</param>
        /// <param name="logicalName">LogicalName belonging to the label in case of an error, so proper details can be given.</param>
        /// <returns></returns>
        /// <exception cref="TestExecutionException"></exception>
        public static string? GetLabelInLanguage(this Label label, int lcid, string logicalName)
        {
            if (label == null)
                return null;

            var result = label.LocalizedLabels.Where(l => l.LanguageCode == lcid).FirstOrDefault()?.Label;

            if (label.UserLocalizedLabel != null && !string.IsNullOrEmpty(label.UserLocalizedLabel.Label) && string.IsNullOrEmpty(result))
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.MissingTranslation, logicalName);

            return result;
        }

        public static string? GetLabelInLanguage(this FormLabel[] labels, int languageCode, int fallbackLanguage)
          => labels.FirstOrDefault(l => l.LanguageCode == languageCode && !string.IsNullOrEmpty(l.Label))?.Label
          ?? labels.FirstOrDefault(l => l.LanguageCode == fallbackLanguage && !string.IsNullOrEmpty(l.Label))?.Label;
    }
}
