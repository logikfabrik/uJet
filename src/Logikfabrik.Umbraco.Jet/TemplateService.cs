// <copyright file="TemplateService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using EnsureThat;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="TemplateService" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class TemplateService : ITemplateService
    {
        private static ITemplateService _instance;

        public TemplateService()
        {
            TemplatePaths = GetTemplatePaths();
        }

        /// <inheritdoc />
        public IEnumerable<string> TemplatePaths { get; }

        /// <inheritdoc />
        public string GetContent(string templatePath)
        {
            Ensure.That(templatePath).IsNotNullOrWhiteSpace();

            using (var reader = new StreamReader(templatePath))
            {
                return reader.ReadToEnd();
            }
        }

        /// <inheritdoc />
        public ITemplate GetTemplate(string templatePath)
        {
            Ensure.That(templatePath).IsNotNullOrWhiteSpace();

            var name = Path.GetFileNameWithoutExtension(templatePath);

            return new Template(name, name.Alias()) { Content = GetContent(templatePath) };
        }

        /// <summary>
        /// Gets the template paths for templates in the views folder.
        /// </summary>
        /// <returns>The template paths.</returns>
        private static IEnumerable<string> GetTemplatePaths()
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
            string GetPath(string path) => Path.GetFileName(path) ?? string.Empty;

            return paths.Where(path => !GetPath(path).StartsWith("_", StringComparison.Ordinal));
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