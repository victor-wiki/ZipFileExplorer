using WeifenLuo.WinFormsUI.Docking;
using ZipFileExplorer.Model;

namespace ZipFileExplorer
{
    public partial class frmMain : Form
    {
        private frmExplorer explorerForm = new frmExplorer();
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
        }

        private void explorerForm_OnShowContent(ZipFileInfo fileInfo, bool refresh)
        {
            frmContent contentForm = this.FindContentForm(fileInfo.Path);

            if (contentForm != null)
            {
                contentForm.Show(this.dockPanelMain);

                if(refresh)
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

        private void tsmiOpenFile_Click(object sender, EventArgs e)
        {
            DialogResult result = this.openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                string filePath = this.openFileDialog1.FileName;

                this.Text = filePath;

                this.explorerForm.LoadTree(filePath);

                List<frmContent> forms = this.dockPanelMain.Documents.Select(item => item as frmContent).ToList();

                forms.ForEach(item=>item.Close());

                this.explorerForm.Show(this.dockPanelMain, DockState.DockLeft);               
            }
        }
    }
}
