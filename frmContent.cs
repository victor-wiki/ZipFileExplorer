using ImageMagick;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;
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
            await this.webView.EnsureCoreWebView2Async();

            string extension = Path.GetExtension(fileInfo.Path).ToLower();

            try
            {
                if (FileHelper.IsXmlFile(extension))
                {
                    string html = File.ReadAllText("Template/XmlContent.html");

                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(this.GetStyleNode("Lib/codemirror/codemirror.css"));
                    sb.AppendLine(this.GetStyleNode("Lib/codemirror/addon/fold/foldgutter.css"));
                    sb.AppendLine(this.GetScriptNode("Lib/codemirror/codemirror.js"));
                    sb.AppendLine(this.GetScriptNode("Lib/codemirror/mode/xml.js"));
                    sb.AppendLine(this.GetScriptNode("Lib/codemirror/addon/fold/foldcode.js"));
                    sb.AppendLine(this.GetScriptNode("Lib/codemirror/addon/fold/foldgutter.js"));
                    sb.AppendLine(this.GetScriptNode("Lib/codemirror/addon/fold/xml-fold.js"));

                    sb.AppendLine(html);

                    var doc = XDocument.Parse(fileInfo.Text);
                    string formattedContent = doc.ToString();

                    html = sb.ToString().Replace("##Content##", Uri.EscapeDataString(formattedContent));

                    this.webView.NavigateToString(html);
                }
                else if (extension == ".svg" || extension == ".html" || extension == ".htm")
                {
                    this.webView.NavigateToString(fileInfo.Text);
                }
                else if (fileInfo.Text != null)
                {
                    string pre = $"<pre>{fileInfo.Text}</pre>";

                    this.webView.NavigateToString(pre);
                }
                else if (fileInfo.Stream != null)
                {
                    string fileType = extension.Substring(1);

                    if (FileHelper.IsImageFile(extension))
                    {
                        fileInfo.Stream.Seek(0, SeekOrigin.Begin);

                        using (var image = new MagickImage(fileInfo.Stream))
                        {
                            image.Format = fileType == "png"? MagickFormat.Png : MagickFormat.Jpg;

                            string base64String = FileHelper.GetBase64StringFromByteArray(image.ToByteArray(), "image", fileType);

                            string img = $"<img src='{base64String}'/>";

                            await this.webView.Invoke(async () =>
                            {
                                this.webView.Source = new Uri("about:blank");

                                string encodedHtml = JsonConvert.SerializeObject(img);
                                string script = "window.document.write(" + encodedHtml + ")";

                                await this.webView.EnsureCoreWebView2Async();
                                await this.webView.ExecuteScriptAsync(script);
                            });
                        }
                    }
                    else if(FileHelper.IsAudioFile(extension))
                    {
                        this.CreateAndPlayMedia(fileInfo.Stream, "audio", fileType);
                    }
                    else if(FileHelper.IsVideoFile(extension))
                    {
                        this.CreateAndPlayMedia(fileInfo.Stream, "video", fileType);
                    }                           
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void CreateAndPlayMedia(MemoryStream stream, string type, string fileType)
        {
            string url = FileHelper.GetBase64StringFromByteArray(stream.ToArray(), type, fileType);

            await this.webView.CoreWebView2.ExecuteScriptAsync($"var {type} = document.createElement('{type}'); {type}.src = '{url}'; {type}.style='width:100%;height:100%'; {type}.controls=true; document.body.appendChild({type}); {type}.play();");
        }

        private string GetStyleNode(string path)
        {
            return $"<style>{this.GetFileContent(path)}</style>";
        }

        private string GetScriptNode(string path)
        {
            return $"<script>{this.GetFileContent(path)}</script>";
        }

        private string GetFileContent(string filePath)
        {
            return File.ReadAllText(filePath);
        }        
    }
}
