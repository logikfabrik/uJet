// <copyright file="TemplateService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="TemplateService" /> class.
    /// </summary>
    public class TemplateService : ITemplateService
    {
        private static ITemplateService instance;

        /// <summary>
        /// Prevents a default instance of the <see cref="TemplateService" /> class from being created.
        /// </summary>
        private TemplateService()
        {
            TemplatePaths = GetTemplates();
        }

        /// <summary>
        /// Gets an singleton instance of the template service.
        /// </summary>
        public static ITemplateService Instance => instance ?? (instance = new TemplateService());

        /// <summary>
        /// Gets the template paths for templates in the view folder.
        /// </summary>
        /// <value>
        /// The template paths.
        /// </value>
        public IEnumerable<string> TemplatePaths { get; }

        /// <summary>
        /// Gets the template content.
        /// </summary>
        /// <param name="templatePath">The template path.</param>
        /// <returns>The content of the template.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="templatePath" /> is <c>null</c> or white space.</exception>
        public string GetContent(string templatePath)
        {
            if (string.IsNullOrWhiteSpace(templatePath))
            {
                throw new ArgumentException("Template path cannot be null or white space.", nameof(templatePath));
            }

            using (var reader = new StreamReader(templatePath))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <param name="templatePath">The template path.</param>
        /// <returns>The template.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="templatePath" /> is <c>null</c> or white space.</exception>
        public ITemplate GetTemplate(string templatePath)
        {
            if (string.IsNullOrWhiteSpace(templatePath))
            {
                throw new ArgumentException("Template path cannot be null or white space.", nameof(templatePath));
            }

            var name = Path.GetFileNameWithoutExtension(templatePath);

            return new Template(name, name.Alias()) { Content = GetContent(templatePath) };
        }

        /// <summary>
        /// Gets the templates.
        /// </summary>
        /// <returns>The templates.</returns>
        private static IEnumerable<string> GetTemplates()
        {
            /*
             * When developing exclusively using conventional ASP.NET MVC view mapping, registering all views as
             * templates might cause conflicts. This function will only return views stored in the root folder
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

            return paths.Where(path => !getPath(path).StartsWith("_", StringComparison.Ordinal));
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
