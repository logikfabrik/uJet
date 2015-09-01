//----------------------------------------------------------------------------------
// <copyright file="TemplateService.cs" company="Logikfabrik">
//     The MIT License (MIT)
//
//     Copyright (c) 2015 anton(at)logikfabrik.se
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
//----------------------------------------------------------------------------------

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;
    using global::Umbraco.Core.Models;

    public class TemplateService : ITemplateService
    {
        /// <summary>
        /// The current template service instance.
        /// </summary>
        private static ITemplateService instance;

        /// <summary>
        /// Template paths.
        /// </summary>
        private readonly IEnumerable<string> templatePaths;

        private TemplateService()
        {
            this.templatePaths = GetTemplates();
        }

        public static ITemplateService Instance
        {
            get { return instance ?? (instance = new TemplateService()); }
        }

        /// <summary>
        /// Gets the paths for the templates in the views folder.
        /// </summary>
        public IEnumerable<string> TemplatePaths
        {
            get { return this.templatePaths; }
        }
        
        /// <summary>
        /// Gets the content of a template.
        /// </summary>
        /// <param name="templatePath">The template path.</param>
        /// <returns>The content of the template.</returns>
        public string GetContent(string templatePath)
        {
            if (string.IsNullOrWhiteSpace(templatePath))
            {
                throw new ArgumentException("Template path cannot be null or white space.", "templatePath");
            }

            using (var reader = new StreamReader(templatePath))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Gets a template.
        /// </summary>
        /// <param name="templatePath">The template path.</param>
        /// <returns>A template.</returns>
        public ITemplate GetTemplate(string templatePath)
        {
            if (string.IsNullOrWhiteSpace(templatePath))
            {
                throw new ArgumentException("Template path cannot be null or white space.", "templatePath");
            }

            var name = Path.GetFileNameWithoutExtension(templatePath);

            return new Template(templatePath, name, name.Alias()) { Content = this.GetContent(templatePath) };
        }

        /// <summary>
        /// Gets the templates in the views folder.
        /// </summary>
        /// <returns>Templates (paths).</returns>
        private static IEnumerable<string> GetTemplates()
        {
            /*
             * When developing exclusively using conventional ASP.NET MVC view mapping, registering all views as 
             * templates might cause conflicts. This function will only return templates stored in the root folder 
             * for that reason.
             * 
             * Developers might use the MVC features of uJet in combination with route hijacking. When doing so 
             * all views/templates are stored in the root folder (Umbraco default behaviour).
             */
            var paths = Directory.GetFiles(GetViewsPath(), "*.cshtml", SearchOption.TopDirectoryOnly);

            /*
             * Exclude all files with a file names that starts with an underscore. We do not want to 
             * add templates for _layout.cshtml, _viewstart.cshtml.
             */
            Func<string, string> getPath = path => Path.GetFileName(path) ?? string.Empty;

            return paths.Where(path => !getPath(path).StartsWith("_"));
        }

        /// <summary>
        /// Gets the path to the views folder.
        /// </summary>
        /// <returns>The path to the views folder.</returns>
        private static string GetViewsPath()
        {
            return string.Concat(AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, "Views");
        }
    }
}
