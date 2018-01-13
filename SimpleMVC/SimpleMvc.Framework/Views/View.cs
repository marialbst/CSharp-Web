﻿namespace SimpleMvc.Framework.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Contracts;

    public class View : IRenderable
    {
        public const string BaseLayoutFileName = "Layout";

        public const string ContentPlaceholder = "{{{content}}}";

        public const string HtmlExtension = ".html";

        public const string LocalErrorPath = "\\SimpleMvc.Framework\\Errors\\Error.html";

        private readonly string templateFullQuailifiedName;

        private readonly IDictionary<string, string> viewData;

        public View(string templateFullQuailifiedName, IDictionary<string, string> viewData)
        {
            this.templateFullQuailifiedName = templateFullQuailifiedName;
            this.viewData = viewData;
        }

        public string Render()
        {
            string fullHtml = this.ReadFile();

            if (this.viewData.Any())
            {
                foreach (var param in this.viewData)
                {
                    fullHtml = fullHtml.Replace($"{{{{{{{param.Key}}}}}}}",param.Value);
                }
            }

            return fullHtml;
        }

        private string ReadFile()
        {
            string layoutHtml = this.RenderLayoutHtml();

            string templateFullQualifiedNameWithExtension = this.templateFullQuailifiedName + HtmlExtension;
            
            if (!File.Exists(templateFullQualifiedNameWithExtension))
            {
                string errorPath = this.GetErrorPath();
                string errorHtml = File.ReadAllText(errorPath);

                this.viewData.Add("error","Requested view does not exist!");

                return errorHtml;
            }

            string htmlFileContent = File.ReadAllText(templateFullQualifiedNameWithExtension);
            string result = layoutHtml.Replace(ContentPlaceholder, htmlFileContent);

            return result;
        }

        private string GetErrorPath()
        {
            string appDirectoryPath = Directory.GetCurrentDirectory();
            DirectoryInfo parentDirectory = Directory.GetParent(appDirectoryPath);
            string parentDirectoryPath = parentDirectory.FullName;

            string errorPagePath = parentDirectoryPath + LocalErrorPath;

            return errorPagePath;
        }

        private string RenderLayoutHtml()
        {
            string layoutHtmlQualifiedName = string.Format(
                "{0}\\{1}{2}",
                MvcContext.Get.ViewsFolder,
                BaseLayoutFileName,
                HtmlExtension
            );

            if (!File.Exists(layoutHtmlQualifiedName))
            {
                string errorPath = this.GetErrorPath();

                string errorHtml = File.ReadAllText(errorPath);

                this.viewData.Add("error", "Layout view does not exist!");

                return errorHtml;
            }

            string layoutHtmlFileContent = File.ReadAllText(layoutHtmlQualifiedName);
            return layoutHtmlFileContent;
        }
    }
}
