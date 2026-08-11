using ImageMagick;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZipFileExplorer.Model;

namespace ZipFileExplorer.Controls
{
    public partial class UC_WebViewer : UserControl
    {
        public UC_WebViewer()
        {
            InitializeComponent();
        }

        public async Task ShowContent(ZipFileInfo fileInfo)
        {
            await this.webView.EnsureCoreWebView2Async();

            string extension = Path.GetExtension(fileInfo.Path).ToLower();

            if (extension == ".svg" || extension == ".html" || extension == ".htm")
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
                        image.Format = fileType == "png" ? MagickFormat.Png : MagickFormat.Jpg;

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
                else if (FileHelper.IsAudioFile(extension))
                {
                    this.CreateAndPlayMedia(fileInfo.Stream, "audio", fileType);
                }
                else if (FileHelper.IsVideoFile(extension))
                {
                    this.CreateAndPlayMedia(fileInfo.Stream, "video", fileType);
                }
            }
        }

        private async void CreateAndPlayMedia(MemoryStream stream, string type, string fileType)
        {
            string url = FileHelper.GetBase64StringFromByteArray(stream.ToArray(), type, fileType);

            await this.webView.CoreWebView2.ExecuteScriptAsync($"var {type} = document.createElement('{type}'); {type}.src = '{url}'; {type}.style='width:100%;height:100%'; {type}.controls=true; document.body.appendChild({type}); {type}.play();");
        }      
    }
}
