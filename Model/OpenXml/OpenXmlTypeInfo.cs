namespace ZipFileExplorer.Model.OpenXml
{
    public class OpenXmlTypeInfo
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Namespace { get; set; }
        public string BaseClass { get; set; }
        public string Summary { get; set; }
        public string CompositeType { get; set; }
        public List<OpenXmlAttributeInfo> Attributes { get; set; }
        public List<OpenXmlPropertyInfo> Children { get; set; }
        public OpenXmlParticleInfo Particle { get; set; }
    }
}
