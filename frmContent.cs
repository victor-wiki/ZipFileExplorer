using ImageMagick;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;
using ZipFileExplorer.Controls;
using ZipFileExplorer.Model;

namespace ZipFileExplorer
{
    public partial class frmContent : frmDockWindowBase
    {
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

                    uC_XmlViewer.ShowContent(fileInfo.Text);
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

       
    }
}
