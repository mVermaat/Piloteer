namespace Piloteer.PowerPlatform.Metadata
{
    public class UnparsedAttribute
    {
        public UnparsedAttribute(string property, string value)
        {
            Property = property;
            Value = value;
        }

        public string Property { get; set; }
        public string Value { get; set; }
    }
}
