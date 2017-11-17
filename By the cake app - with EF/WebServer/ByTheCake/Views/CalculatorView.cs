namespace WebServer.ByTheCake.Views
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Server.Contracts;

    public class CalculatorView : IView
    {
        private const string Path = @"ByTheCake/Resources/calculator.html";
        private const string ResultPlaceholder = "{{{result}}}";

        private Dictionary<string, string> data;
        private string replacement;

        public CalculatorView(Dictionary<string, string> data = null)
        {
            this.data = data;
        }

        public string View()
        {
            string html = File.ReadAllText(Path);

            if (data == null)
            {
                replacement = string.Empty;
            }
            else
            {
                replacement = this.FormatReplacement(data);
            }

            return html.Replace(ResultPlaceholder, replacement);
        }

        private string FormatReplacement(Dictionary<string, string> result)
        {
            StringBuilder replacement = new StringBuilder();

            foreach (var item in result)
            {
                replacement.AppendLine($"<div>{item.Key}: {item.Value}</div>");
            }

            return replacement.ToString();
        }
    }
}
