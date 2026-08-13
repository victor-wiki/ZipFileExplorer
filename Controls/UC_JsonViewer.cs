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

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(this.GetStyleNode("Lib/codemirror/codemirror.css").Replace(".cm-string {color: #ff0000;}", ".cm-string {color: #A31515;}"));
            sb.AppendLine(this.GetStyleNode("Lib/codemirror/addon/fold/foldgutter.css"));
            sb.AppendLine(this.GetScriptNode("Lib/codemirror/codemirror.js")); 
            sb.AppendLine(this.GetScriptNode("Lib/codemirror/addon/fold/foldcode.js"));          
            sb.AppendLine(this.GetScriptNode("Lib/codemirror/addon/fold/foldgutter.js"));
            sb.AppendLine(this.GetScriptNode("Lib/codemirror/addon/fold/brace-fold.js"));
            sb.AppendLine(this.GetScriptNode("Lib/codemirror/addon/fold/comment-fold.js"));
            sb.AppendLine(this.GetScriptNode("Lib/codemirror/mode/javascript.js"));

            sb.AppendLine(html);         

            html = sb.ToString().Replace("##Content##", Uri.EscapeDataString(content));

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
