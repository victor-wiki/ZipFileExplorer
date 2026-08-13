namespace ZipFileExplorer.Model.OpenXml
{
    public class OpenXmlSchemaInfo
    {
        public string TargetNamespace { get; set; }
        public List<OpenXmlTypeInfo> Types { get; set; }
        public List<OpenXmlEnumInfo> Enums { get; set; }
    }
}
