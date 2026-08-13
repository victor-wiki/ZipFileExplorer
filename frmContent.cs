using ZipFileExplorer.Controls;
using ZipFileExplorer.Model;

namespace ZipFileExplorer
{
    public delegate void ShowDetails(string content);

    public partial class frmContent : frmDockWindowBase
    {
        public ShowDetails OnShowDetails;

        public frmContent()
        {
            InitializeComponent();
        }

        public async void ShowContent(ZipFileInfo fileInfo)
        {
            string extension = Path.GetExtension(fileInfo.Path).ToLower();

            try
            {
                if (FileHelper.IsXmlFile(extension))
                {
                    UC_XmlViewer uC_XmlViewer = new UC_XmlViewer();
                    uC_XmlViewer.Dock = DockStyle.Fill;
                    this.Controls.Add(uC_XmlViewer);

                    uC_XmlViewer.OnShowDetails += this.ShowDetails;

                    uC_XmlViewer.ShowContent(fileInfo);
                }
                else
                {
                    UC_WebViewer uC_WebViewer = new UC_WebViewer();
                    uC_WebViewer.Dock = DockStyle.Fill;
                    this.Controls.Add(uC_WebViewer);

                    await uC_WebViewer.ShowContent(fileInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }       

        private void ShowDetails(string content)
        {
            if(this.OnShowDetails!=null)
            {
                this.OnShowDetails(content);
            }
        }
    }
}
