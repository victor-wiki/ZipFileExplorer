namespace ZipFileExplorer
{
    partial class frmExplorer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExplorer));
            tvExplorer = new TreeView();
            imageList1 = new ImageList(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            tsmiRefresh = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tvExplorer
            // 
            tvExplorer.Dock = DockStyle.Fill;
            tvExplorer.HideSelection = false;
            tvExplorer.ImageIndex = 0;
            tvExplorer.ImageList = imageList1;
            tvExplorer.Location = new Point(0, 0);
            tvExplorer.Name = "tvExplorer";
            tvExplorer.SelectedImageIndex = 0;
            tvExplorer.ShowLines = false;
            tvExplorer.Size = new Size(245, 450);
            tvExplorer.TabIndex = 0;
            tvExplorer.AfterSelect += tvExplorer_AfterSelect;
            tvExplorer.NodeMouseClick += tvExplorer_NodeMouseClick;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "tree_Fake.png");
            imageList1.Images.SetKeyName(1, "tree_Folder.png");
            imageList1.Images.SetKeyName(2, "tree_File.png");
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { tsmiRefresh });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(121, 26);
            // 
            // tsmiRefresh
            // 
            tsmiRefresh.Name = "tsmiRefresh";
            tsmiRefresh.Size = new Size(180, 22);
            tsmiRefresh.Text = "Refresh";
            tsmiRefresh.Click += tsmiRefresh_Click;
            // 
            // frmExplorer
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(245, 450);
            Controls.Add(tvExplorer);
            Name = "frmExplorer";
            Text = "Explorer";
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TreeView tvExplorer;
        private ImageList imageList1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem tsmiRefresh;
    }
}