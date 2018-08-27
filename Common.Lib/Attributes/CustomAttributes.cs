namespace Common.Lib.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class PdfFieldName : System.Attribute
    {
        public string Name;

        public PdfFieldName(string name)
        {
            Name = name;
        }
    }
}
