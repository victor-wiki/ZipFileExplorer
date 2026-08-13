namespace ZipFileExplorer.Model.OpenXml
{
    public class OpenXmlParticleInfo
    {
        public string Kind { get; set; }
        public List<OpenXmlParticleItemInfo> Items { get; set; }
    }

    public class OpenXmlParticleItemInfo
    {
        public string Name { get; set; }
        public List<OpenXmlOccurInfo> Occurs { get; set; }
    }

    public class OpenXmlOccurInfo
    {
        public int Max { get; set; }
    }
}
