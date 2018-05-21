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
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="TemplateService" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class TemplateService : ITemplateService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateService"/> class.
        /// </summary>
        /// <param name="appDomain">The application domain.</param>
        public TemplateService(AppDomain appDomain)
        {
            Ensure.That(appDomain).IsNotNull();

            TemplatePaths = GetTemplatePaths(appDomain);
        }

        /// <inheritdoc />
        public IEnumerable<string> TemplatePaths { get; }

        /// <inheritdoc />
        public ITemplate GetTemplate(string templatePath)
        {
            Ensure.That(templatePath).IsNotNullOrWhiteSpace();

            var name = Path.GetFileNameWithoutExtension(templatePath);

            return new Template(name, name.Alias()) { Content = GetContent(templatePath) };
        }

        private static IEnumerable<string> GetTemplatePaths(AppDomain appDomain)
        {
            /*
             * When developing exclusively using conventional ASP.NET MVC view mapping, registering all views as
             * templates might cause conflicts. This function will only return views stored in the root folder
             * for that reason. Views in sub-folders will not be returned; they will be considered as
             * ASP.NET MVC views views and not Umbraco templates.
             *
             * Developers might use the MVC features of uJet in combination with route hijacking. When doing so
             * all views/templates are stored in the root folder (Umbraco default behaviour).
             */
            var paths = Directory.GetFiles(GetViewsPath(appDomain), "*.cshtml", SearchOption.TopDirectoryOnly);

            /*
             * Exclude all files with a file names that starts with an underscore. We do not want to
             * add templates for e.g. _layout.cshtml, _viewstart.cshtml.
             */
            string GetPath(string path) => Path.GetFileName(path) ?? string.Empty;

            return paths.Where(path => !GetPath(path).StartsWith("_", StringComparison.Ordinal));
        }

        private static string GetViewsPath(AppDomain appDomain)
        {
            return string.Concat(appDomain.BaseDirectory, Path.DirectorySeparatorChar, "Views");
        }

        private static string GetContent(string templatePath)
        {
            using (var reader = new StreamReader(templatePath))
            {
                return reader.ReadToEnd();
            }
        }
    }
}