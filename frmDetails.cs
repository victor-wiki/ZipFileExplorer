using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ZipFileExplorer
{
    public partial class frmDetails : DockContent
    {
        public frmDetails()
        {
            InitializeComponent();
        }

        public async Task SetContent(string content)
        {
            await this.uC_JsonViewer1.ShowContent(content);
        }
    }
}
