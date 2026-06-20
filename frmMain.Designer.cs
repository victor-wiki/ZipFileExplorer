namespace ZipFileExplorer
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            menuStrip1 = new MenuStrip();
            tsmiOpenFile = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            dockPanelMain = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { tsmiOpenFile });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1039, 25);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // tsmiOpenFile
            // 
            tsmiOpenFile.Name = "tsmiOpenFile";
            tsmiOpenFile.Size = new Size(75, 21);
            tsmiOpenFile.Text = "Open File";
            tsmiOpenFile.Click += tsmiOpenFile_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.Filter = "ZIP file|*.zip|PPT file|*.pptx|Word file|*.docx|Excel file|*.xlsx|All files|*.*";
            // 
            // dockPanelMain
            // 
            dockPanelMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dockPanelMain.Location = new Point(0, 28);
            dockPanelMain.Name = "dockPanelMain";
            dockPanelMain.Size = new Size(1039, 541);
            dockPanelMain.TabIndex = 13;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1039, 570);
            Controls.Add(dockPanelMain);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Zip File Explorer";
            WindowState = FormWindowState.Maximized;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem tsmiOpenFile;
        private OpenFileDialog openFileDialog1;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanelMain;
    }
}
