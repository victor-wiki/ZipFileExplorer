using Microsoft.Web.WebView2.Core;
using System.Reflection;
using System.Text;

namespace ZipFileExplorer.Controls
{
    public partial class UC_JsonViewer : UserControl
    {
        public UC_JsonViewer()
        {
            InitializeComponent();
        }

        public async Task ShowContent(string content)
        {
            await this.webView.EnsureCoreWebView2Async();

            string html = File.ReadAllText("Template/JsonContent.html");

            string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string codemirrorFolder = Path.Combine(location, "Lib/codemirror");

            this.webView.CoreWebView2.SetVirtualHostNameToFolderMapping("codemirror.local", codemirrorFolder, CoreWebView2HostResourceAccessKind.Allow);

            html = html.Replace("##Content##", Uri.EscapeDataString(content));

            this.webView.NavigateToString(html);
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
