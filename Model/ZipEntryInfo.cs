using ICSharpCode.SharpZipLib.Zip;
using SharpCompress.Archives;

namespace ZipFileExplorer
{
    public class ZipEntryInfo
    {
        public bool IsFile { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }       

    }
}
