using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ZipFileExplorer.Controls
{
    public partial class UC_XmlViewer : UserControl
    {
        public Form SearchForm => this.fctb?.findForm;

        public UC_XmlViewer()
        {
            InitializeComponent();
        }

        public void ShowContent(string content)
        {
            XDocument doc = XDocument.Parse(content);
            string formattedXml = doc.ToString();

            this.fctb.Text = formattedXml;
        }     

        private Control FindParentByType(Control control, Type type)
        {
            if (control != null)
            {
                if (control.GetType() == type || (type.Name == nameof(Form) && control is Form))
                {
                    return control;
                }
                else
                {
                    return FindParentByType(control.Parent, type);
                }
            }

            return null;
        }

        private async void fctb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                await Task.Delay(100);

                this.SearchForm.StartPosition = FormStartPosition.Manual;

                Control topLevelControl = this.TopLevelControl;
                Control parent = this.FindParentByType(this.Parent, typeof(Form));

                int x = topLevelControl.Location.X + topLevelControl.Width - this.SearchForm.Width;
                int y = parent.Top + 55;

                this.SearchForm.Location = new System.Drawing.Point(x, y);
            }
        }
    }
}
