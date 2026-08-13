namespace ZipFileExplorer.Model.OpenXml
{
    public class OpenXmlValidatorInfo
    {
        public string Name { get; set; }
        public bool IsInitialVersion { get; set; }
        public List<OpenXmlValidatorArgumentInfo> Arguments { get; set; }
    }

    public class OpenXmlValidatorArgumentInfo
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
