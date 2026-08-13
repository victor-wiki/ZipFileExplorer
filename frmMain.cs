using WeifenLuo.WinFormsUI.Docking;
using ZipFileExplorer.Controls;
using ZipFileExplorer.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ZipFileExplorer
{
    public partial class frmMain : Form
    {
        private frmExplorer explorerForm = new frmExplorer();
        private frmDetails detailsForm = new frmDetails();

        public frmMain()
        {
            InitializeComponent();

            AutoScaleMode = AutoScaleMode.Dpi;

            this.InitControls();
        }

        private void InitControls()
        {
            this.dockPanelMain.Theme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();

            this.explorerForm.HideOnClose = true;
            this.explorerForm.OnShowContent += this.explorerForm_OnShowContent;

            this.dockPanelMain.ActiveDocumentChanged += DockPanelMain_ActiveDocumentChanged; ;
        }

        private void DockPanelMain_ActiveDocumentChanged(object? sender, EventArgs e)
        {
            this.CloseSearchForm();
        }

        private void explorerForm_OnShowContent(ZipFileInfo fileInfo, bool refresh)
        {
            this.CloseSearchForm();

            frmContent contentForm = this.FindContentForm(fileInfo.Path);

            if (contentForm != null)
            {
                contentForm.Show(this.dockPanelMain);

                if (refresh)
                {
                    contentForm.ShowContent(fileInfo);
                }
            }
            else
            {
                contentForm = new frmContent();

                var documents = this.dockPanelMain.Documents;

                contentForm.Tag = fileInfo;
                contentForm.Text = Path.GetFileName(fileInfo.Path);
                contentForm.ToolTipText = fileInfo.Path;

                contentForm.Show(this.dockPanelMain, DockState.Document);

                if (documents != null && documents.Count() > 1)
                {
                    contentForm.DockTo(this.dockPanelMain.ActiveDocumentPane, DockStyle.Fill, 0);
                }

                contentForm.OnShowDetails += this.ShowDetails;

                contentForm.ShowContent(fileInfo);
            }
        }

        private frmContent FindContentForm(string filePath)
        {
            foreach (IDockContent content in this.dockPanelMain.Documents)
            {
                frmContent form = content as frmContent;

                ZipFileInfo fileInfo = form.Tag as ZipFileInfo;

                if (fileInfo.Path == filePath)
                {
                    return content as frmContent;
                }
            }

            return null;
        }

        private void CloseSearchForm()
        {
            List<frmContent> forms = this.dockPanelMain.Documents.Where(item => item is frmContent).Select(item => item as frmContent).ToList();

            foreach (var form in forms)
            {
                var xmlViewer = form.Controls.Cast<Control>().FirstOrDefault(item => item is UC_XmlViewer) as UC_XmlViewer;

                if (xmlViewer != null)
                {
                    if (xmlViewer.SearchForm != null)
                    {
                        xmlViewer.SearchForm.Close();
                    }
                }
            }
        }

        private void tsmiOpenFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.FileName = "";

            DialogResult result = this.openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                string filePath = this.openFileDialog1.FileName;

                this.Text = filePath;

                this.explorerForm.LoadTree(filePath);

                List<frmContent> forms = this.dockPanelMain.Documents.Select(item => item as frmContent).ToList();

                forms.ForEach(item => item.Close());

                if (this.detailsForm != null && this.detailsForm.IsDisposed == false)
                {
                    this.detailsForm.Close();
                }

                this.explorerForm.Show(this.dockPanelMain, DockState.DockLeft);
            }
        }

        private async void ShowDetails(string content)
        {
            if (this.detailsForm == null || this.detailsForm.IsDisposed)
            {
                this.detailsForm = new frmDetails();
            }

            await detailsForm.SetContent(content);

            detailsForm.Show(this.dockPanelMain, DockState.DockRight);
        }
    }
}
