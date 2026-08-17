using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.UserControls;
using Newtonsoft.Json;
using System.Reflection;
using System.Xml.Linq;
using ZipFileExplorer.Model;
using ZipFileExplorer.Model.OpenXml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ZipFileExplorer.Controls
{
    public partial class UC_XmlViewer : UserControl
    {
        private ZipFileInfo fileInfo;

        public Form SearchForm = null;

        public ShowDetails OnShowDetails;

        public UC_XmlViewer()
        {
            InitializeComponent();

            this.textEditor.ActiveTextAreaControl.TextArea.MouseDown += this.textEditor_MouseDown;

            Type type = this.textEditor.GetType();

            FieldInfo fieldInfo = type.GetField("_findForm", BindingFlags.NonPublic | BindingFlags.Instance);

            if (fieldInfo != null)
            {
                FindAndReplaceForm findForm = fieldInfo.GetValue(this.textEditor) as FindAndReplaceForm;

                this.SearchForm = findForm;

                if (findForm != null)
                {
                    findForm.Shown += this.FindForm_Shown;
                }
            }
        }

        public void ShowContent(ZipFileInfo fileInfo)
        {
            this.fileInfo = fileInfo;

            XDocument doc = XDocument.Parse(fileInfo.Text);
            string formattedXml = doc.ToString();

            this.textEditor.Text = formattedXml;

            this.textEditor.Document.FoldingManager.UpdateFoldings(null, null);
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

        private void tsmiShowDetails_Click(object sender, EventArgs e)
        {
            Point? point = this.contextMenuStrip1.Tag == null ? null : (Point)this.contextMenuStrip1.Tag;

            if (point == null)
            {
                return;
            }

            var caret = this.textEditor.ActiveTextAreaControl.TextArea.Caret;
            int lineIndex = caret.Line;

            if (lineIndex >= 0 && lineIndex < this.textEditor.Lines.Length)
            {
                bool isExcel = Path.GetExtension(this.fileInfo.PackageName).ToLower() == ".xlsx";

                string line = this.textEditor.Lines[lineIndex].Trim();

                string content = this.fileInfo.Text;

                XDocument doc = XDocument.Parse(content);

                XElement root = doc.Root;

                Func<string, string> getTagName = (line) =>
                {
                    int index = line.IndexOf(">");

                    string tag = null;

                    if (index > 0)
                    {
                        tag = line.Substring(0, index);
                    }
                    else
                    {
                        tag = line;
                    }

                    return tag.Trim('<', '>', '/').Split(' ')[0];
                };

                string name = getTagName(line);

                Func<string, string> getPrefix = (tagName) =>
                {
                    return tagName.Contains(":") ? tagName.Split(':')[0] : (isExcel ? "x" : "");
                };

                Func<string, string> getSchema = (line) =>
                {
                    string tagName = getTagName(line);
                    string prefix = getPrefix(tagName);

                    string schema = null;
                    string uriValue = null;

                    foreach (var attr in root.Attributes())
                    {
                        if (attr == null)
                        {
                            continue;
                        }

                        if ((prefix != "" && attr.Name.LocalName == prefix) || ((prefix == "" || (prefix == "x" || isExcel)) && !attr.Name.ToString().Contains(":")))
                        {
                            uriValue = attr.Value;

                            break;
                        }
                    }

                    if (uriValue == null && prefix != "")
                    {
                        int index = line.IndexOf(' ');

                        if (index > 0)
                        {
                            var items = line.Substring(index + 1).TrimEnd('>').Split(' ');

                            foreach (var item in items)
                            {
                                if (item.Contains($":{prefix}="))
                                {
                                    uriValue = item.Split('=')[1];

                                    break;
                                }
                            }
                        }
                    }

                    if (uriValue != null)
                    {
                        Uri uri = new Uri(uriValue);

                        schema = uri.Host + uri.AbsolutePath;
                    }

                    return schema;
                };

                string schema = getSchema(line);

                if (schema != null)
                {
                    string namespaceFilePath = "Data/OpenXml/typed/namespaces.json";

                    var namespaceInfos = JsonConvert.DeserializeObject<OpenXmlNamespaceInfo[]>(File.ReadAllText(namespaceFilePath));

                    Func<string, string> getSchemaFileName = (schema) =>
                    {
                        return schema.Replace(".", "_").Replace("/", "_") + ".json"; ;
                    };

                    Func<string, string> getSchemaFilePath = (schema) =>
                    {
                        string schemaFileName = getSchemaFileName(schema);

                        string schemaFilePath = Path.Combine("Data/OpenXml/schemas", schemaFileName);

                        return schemaFilePath;
                    };

                    Func<string, string> getFilterName = (name) =>
                    {
                        if (isExcel && !name.Contains(":"))
                        {
                            return "x:" + name;
                        }

                        return name;
                    };

                    string schemaFileName = getSchemaFileName(schema);
                    string schemaFilePath = getSchemaFilePath(schema);

                    string details = null;

                    string prefix = getPrefix(name);
                    string ns = namespaceInfos.FirstOrDefault(item => item.Prefix == prefix)?.Namespace;

                    bool foundType = false;

                    if (File.Exists(schemaFilePath))
                    {
                        OpenXmlSchemaInfo schemaInfo = JsonConvert.DeserializeObject<OpenXmlSchemaInfo>(File.ReadAllText(schemaFilePath));

                        string filterName = getFilterName(name);

                        var types = schemaInfo.Types.Where(item => item.Name.EndsWith("/" + filterName)).ToArray();

                        if (types.Length == 1)
                        {
                            foundType = true;
                            types[0].Namespace = ns;

                            details = JsonConvert.SerializeObject(types[0], Formatting.Indented);
                        }
                        else
                        {
                            string parentLine = this.GetParentLine(lineIndex);

                            if (!string.IsNullOrEmpty(parentLine))
                            {
                                var parentTagName = getTagName(parentLine);

                                string parentSchema = getSchema(parentLine);

                                if (parentSchema != null)
                                {
                                    string parentSchemaFilePath = getSchemaFilePath(parentSchema);

                                    if (File.Exists(parentSchemaFilePath))
                                    {
                                        OpenXmlSchemaInfo parentSchemaInfo = JsonConvert.DeserializeObject<OpenXmlSchemaInfo>(File.ReadAllText(parentSchemaFilePath));

                                        filterName = getFilterName(parentTagName);

                                        var parentType = parentSchemaInfo.Types.FirstOrDefault(item => item.Name.EndsWith("/" + filterName));

                                        if (parentType != null)
                                        {
                                            if (parentType.Children != null)
                                            {
                                                var type = types.FirstOrDefault(item => parentType.Children.Any(t => t.Name == item.Name));

                                                if (type != null)
                                                {
                                                    foundType = true;
                                                    type.Namespace = ns;

                                                    details = JsonConvert.SerializeObject(type, Formatting.Indented);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!foundType)
                    {
                        schemaFilePath = Path.Combine("Data/OpenXml/typed", schemaFileName);

                        if (File.Exists(schemaFilePath))
                        {
                            OpenXmlMarkupInfo[] markupInfos = JsonConvert.DeserializeObject<OpenXmlMarkupInfo[]>(File.ReadAllText(schemaFilePath));

                            string filterName = getFilterName(name);

                            var markupInfo = markupInfos.FirstOrDefault(item => item.Name.EndsWith("/" + filterName));

                            Action<OpenXmlMarkupInfo> setDetails = (markupInfo) =>
                            {
                                markupInfo.Namespace = ns;

                                details = JsonConvert.SerializeObject(markupInfo, Formatting.Indented);
                            };

                            if (markupInfo != null)
                            {
                                setDetails(markupInfo);
                            }
                            else if (isExcel)
                            {
                                schemaFilePath = Path.Combine("Data/OpenXml/typed", "schemas_openxmlformats_org_markup-compatibility_2006.json");

                                if (File.Exists(schemaFilePath))
                                {
                                    markupInfos = JsonConvert.DeserializeObject<OpenXmlMarkupInfo[]>(File.ReadAllText(schemaFilePath));
                                    markupInfo = markupInfos.FirstOrDefault(item => item.Name.EndsWith("/" + filterName));

                                    if (markupInfo != null)
                                    {
                                        setDetails(markupInfo);
                                    }
                                }
                            }
                        }
                    }

                    if (this.OnShowDetails != null)
                    {
                        this.OnShowDetails(details ?? "");
                    }
                }
            }
        }

        private string GetParentLine(int currentLineIndex)
        {
            string line = this.textEditor.Lines[currentLineIndex];

            string parentLine = this.textEditor.Lines.Select((item, index) => new { Line = item, Index = index })
                .Where(item => item.Index < currentLineIndex)
                .OrderByDescending(item => item.Index)
                .FirstOrDefault(item => (item.Line.Length - item.Line.TrimStart().Length) < (line.Length - line.TrimStart().Length)).Line?.Trim();

            return parentLine;
        }

        private void textEditor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string extension = Path.GetExtension(this.fileInfo.PackageName).ToLower();

                if (FileHelper.IsOpenXmlFileForParsing(extension))
                {
                    this.contextMenuStrip1.Tag = new Point(e.X, e.Y);

                    this.contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void FindForm_Shown(object sender, EventArgs e)
        {
            if (this.SearchForm != null && this.SearchForm.IsDisposed == false)
            {
                Control topLevelControl = this.TopLevelControl;
                Control parent = this.FindParentByType(this.Parent, typeof(Form));

                int x = topLevelControl.Location.X + topLevelControl.Width - this.SearchForm.Width - 10;
                int y = parent.Top + 55;

                this.SearchForm.Location = new System.Drawing.Point(x, y);
            }
        }
    }
}
