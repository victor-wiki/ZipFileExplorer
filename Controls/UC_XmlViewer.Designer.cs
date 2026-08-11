using FastColoredTextBoxNS;
using System.Resources;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_XmlViewer));
            fctb = new FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)fctb).BeginInit();
            SuspendLayout();
            // 
            // fctb
            // 
            fctb.AutoCompleteBracketsList = new char[]
    {
    '(',
    ')',
    '{',
    '}',
    '[',
    ']',
    '"',
    '"',
    '\'',
    '\''
    };
            fctb.AutoIndentCharsPatterns = "";
            fctb.AutoIndentExistingLines = false;
            fctb.AutoScrollMinSize = new Size(35, 14);
            fctb.BackBrush = null;
            fctb.BorderStyle = BorderStyle.FixedSingle;
            fctb.CharHeight = 14;
            fctb.CharWidth = 8;
            fctb.CommentPrefix = null;
            fctb.Cursor = Cursors.IBeam;
            fctb.DefaultMarkerSize = 8;
            fctb.DelayedEventsInterval = 200;
            fctb.DelayedTextChangedInterval = 500;
            fctb.DisabledColor = Color.FromArgb(100, 180, 180, 180);
            fctb.Dock = DockStyle.Fill;
            //fctb.Hotkeys = resources.GetString("fctb.Hotkeys");
            fctb.ImeMode = ImeMode.Off;
            fctb.IsReplaceMode = false;
            fctb.Language = Language.XML;
            fctb.LeftBracket = '<';
            fctb.LeftBracket2 = '(';
            fctb.Location = new Point(0, 0);
            fctb.Name = "fctb";
            fctb.Paddings = new Padding(0);
            fctb.ReservedCountOfLineNumberChars = 2;
            fctb.RightBracket = '>';
            fctb.RightBracket2 = ')';
            fctb.SelectionColor = Color.FromArgb(100, 255, 255, 0);
            fctb.ServiceColors = (ServiceColors)resources.GetObject("fctb.ServiceColors");
            fctb.Size = new Size(667, 385);
            fctb.TabIndex = 3;
            fctb.Zoom = 100;
            fctb.KeyDown += fctb_KeyDown;
            // 
            // UC_XmlViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(fctb);
            Name = "UC_XmlViewer";
            Size = new Size(667, 385);
            ((System.ComponentModel.ISupportInitialize)fctb).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private FastColoredTextBox fctb;
    }
}
