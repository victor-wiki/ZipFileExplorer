namespace ZipFileExplorer.Controls
{
    partial class UC_XmlViewer
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            textEditor = new ICSharpCode.TextEditor.TextEditorControlEx();
            contextMenuStrip1 = new ContextMenuStrip(components);
            tsmiShowDetails = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // textEditor
            // 
            textEditor.Dock = DockStyle.Fill;
            textEditor.FoldingStrategy = "XML";
            textEditor.Font = new Font("Courier New", 10F);
            textEditor.Location = new Point(0, 0);
            textEditor.Name = "textEditor";
            textEditor.ShowVRuler = false;
            textEditor.Size = new Size(667, 385);
            textEditor.SyntaxHighlighting = "XML";
            textEditor.TabIndex = 0;
            textEditor.VRulerRow = 0;          
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { tsmiShowDetails });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(151, 26);
            // 
            // tsmiShowDetails
            // 
            tsmiShowDetails.Name = "tsmiShowDetails";
            tsmiShowDetails.Size = new Size(150, 22);
            tsmiShowDetails.Text = "Show Details";
            tsmiShowDetails.Click += tsmiShowDetails_Click;
            // 
            // UC_XmlViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(textEditor);
            Name = "UC_XmlViewer";
            Size = new Size(667, 385);
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ICSharpCode.TextEditor.TextEditorControlEx textEditor;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem tsmiShowDetails;
    }
}
