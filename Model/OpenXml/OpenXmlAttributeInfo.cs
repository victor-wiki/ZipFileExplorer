namespace ZipFileExplorer.Model.OpenXml
{
    public class OpenXmlAttributeInfo
    {
        public string QName { get; set; }
        public string PropertyName { get; set; }
        public string Type { get; set; }
        public string PropertyComments { get; set; }
        public List<OpenXmlValidatorInfo> Validators { get; set; }
    }
}
