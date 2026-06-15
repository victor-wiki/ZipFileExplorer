using ICSharpCode.SharpZipLib.Core;
using SharpCompress.Archives;
using System.Data;
using System.Text;
using ZipFileExplorer.Model;

namespace ZipFileExplorer
{
    public delegate void ShowContentHandler(ZipFileInfo fileInfo, bool refresh);

    public partial class frmExplorer : frmDockWindowBase
    {
        public ShowContentHandler OnShowContent;

        private string filePath;
        private List<ZipEntryInfo> entryInfos = new List<ZipEntryInfo>();
        private string currentEntryDirectory = null;

        public frmExplorer()
        {
            InitializeComponent();
        }

        public void LoadTree(string filePath)
        {
            this.filePath = filePath;
            this.entryInfos.Clear();
            this.tvExplorer.Nodes.Clear();

            using (var zipfile = ArchiveFactory.OpenArchive(this.filePath, this.GetZipOptions()))
            {
                foreach (var entry in zipfile.Entries)
                {
                    string key = entry.Key;
                    string name = Path.GetFileName(key.TrimEnd('/'));
                    string path = entry.Key.TrimEnd('/');

                    ZipEntryInfo entryInfo = new ZipEntryInfo()
                    {
                        IsFile = !entry.IsDirectory,
                        Key = key,
                        Name = name,
                        Path = path
                    };

                    this.entryInfos.Add(entryInfo);
                }

                List<string> rootFolderNames = new List<string>();

                foreach (var entry in this.entryInfos)
                {
                    int index = entry.Path.IndexOf("/");

                    if (index > 0)
                    {
                        string rootFolder = entry.Path.Substring(0, index);

                        if (!rootFolderNames.Contains(rootFolder))
                        {
                            rootFolderNames.Add(rootFolder);
                        }
                    }
                }

                rootFolderNames.Sort();

                foreach (var rootFolderName in rootFolderNames)
                {
                    TreeNode node = new TreeNode(rootFolderName);
                    node.ImageIndex = 1;
                    node.Tag = rootFolderName;

                    this.tvExplorer.Nodes.Add(node);

                    this.AddChildNodes(node, rootFolderName);
                }

                var rootFiles = this.entryInfos.Where(item => item.IsFile && item.Path.IndexOf("/") == -1).OrderBy(item => item.Name);

                foreach (var file in rootFiles)
                {
                    TreeNode node = new TreeNode(file.Name);
                    node.ImageIndex = 2;
                    node.Tag = file;

                    this.tvExplorer.Nodes.Add(node);
                }
            }
        }

        private void AddChildNodes(TreeNode parentNode, string parentPath)
        {
            string flag = parentPath + "/";

            var childEntries = this.entryInfos.Where(item => item.Path.StartsWith(flag)).OrderByDescending(item => Path.GetDirectoryName(item.Path))
                .ThenBy(item => item.Name);

            foreach (var entry in childEntries)
            {
                string path = entry.Path;

                string subPath = entry.Path.Substring(flag.Length);

                int index = subPath.IndexOf("/");

                if (index == -1)
                {
                    if(entry.IsFile)
                    {
                        TreeNode node = new TreeNode(entry.Name);
                        node.ImageIndex = 2;
                        node.Tag = entry;
                        parentNode.Nodes.Add(node);
                    }                   
                }
                else if (index > 0)
                {
                    string folderName = subPath.Substring(0, index);

                    string p = path.Substring(0, flag.Length + index);

                    bool existing = false;

                    foreach (TreeNode n in parentNode.Nodes)
                    {
                        if (n.Tag?.ToString() == p)
                        {
                            existing = true;
                            break;
                        }
                    }

                    if (!existing)
                    {
                        TreeNode node = new TreeNode(folderName);
                        node.ImageIndex = 1;
                        node.Tag = p;
                        parentNode.Nodes.Add(node);

                        this.AddChildNodes(node, p);
                    }
                }
            }
        }

        private SharpCompress.Readers.ReaderOptions GetZipOptions()
        {
            var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;

            SharpCompress.Readers.ReaderOptions options = new SharpCompress.Readers.ReaderOptions();

            Encoding defaultEncoding = Encoding.UTF8;

            if (cultureInfo.Name == "zh-CN")
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                defaultEncoding = Encoding.GetEncoding("gbk");
            }

            options.ArchiveEncoding.Default = defaultEncoding;

            return options;
        }

        private void tvExplorer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.ShowContent(false);
        }

        private async void ShowContent(bool refresh)
        {
            var node = this.tvExplorer.SelectedNode;

            if (node == null)
            {
                return;
            }

            var entryInfo = node.Tag as ZipEntryInfo;

            if (entryInfo != null)
            {
                node.SelectedImageIndex = 2;

                using (var zipfile = ArchiveFactory.OpenArchive(this.filePath, this.GetZipOptions()))
                {
                    var entry = zipfile.Entries.FirstOrDefault(item => item.Key == entryInfo.Key);

                    using (Stream zipStream = await entry.OpenEntryStreamAsync())
                    {
                        MemoryStream ms = this.ConvertToMemoryStream(zipStream);

                        bool isPlainText = FileHelper.IsPlainTextFile(ms, false);

                        if (isPlainText)
                        {
                            using (StreamReader reader = new StreamReader(ms))
                            {
                                string content = reader.ReadToEnd();

                                if (this.OnShowContent != null)
                                {
                                    this.OnShowContent(new ZipFileInfo() { Path = entryInfo.Path, Text = content }, refresh);
                                }
                            }
                        }
                        else
                        {
                            if (this.OnShowContent != null)
                            {
                                this.OnShowContent(new ZipFileInfo() { Path = entryInfo.Path, Stream = ms }, refresh);
                            }
                        }
                    }
                }
            }
            else
            {
                node.SelectedImageIndex = 1;
            }
        }

        private MemoryStream ConvertToMemoryStream(Stream stream)
        {
            MemoryStream memoryStream = new MemoryStream();

            byte[] buffer = new byte[4096];

            StreamUtils.Copy(stream, memoryStream, buffer);

            memoryStream.Position = 0;

            return memoryStream;
        }

        private void tsmiRefresh_Click(object sender, EventArgs e)
        {
            this.ShowContent(true);
        }

        private void tvExplorer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.tvExplorer.SelectedNode = e.Node;

                this.SetMenuItemVisible(e.Node);

                this.contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void SetMenuItemVisible(TreeNode node)
        {
            ZipEntryInfo entryInfo = node.Tag as ZipEntryInfo;

            this.tsmiRefresh.Visible = entryInfo != null;
        }
    }
}
