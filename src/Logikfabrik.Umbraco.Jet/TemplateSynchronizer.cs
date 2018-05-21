// <copyright file="TemplateSynchronizer.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using EnsureThat;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="TemplateSynchronizer" /> class. Adds and updates templates (Razor views).
    /// </summary>
    public class TemplateSynchronizer : ISynchronizer
    {
        private readonly IFileService _fileService;
        private readonly ITemplateService _templateService;
        private readonly Regex _layoutRegex = new Regex("(?s:(?<=@{.*Layout\\s*=\\s*\").*(?=.cshtml\";.+}))");

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateSynchronizer" /> class.
        /// </summary>
        /// <param name="fileService">The file service.</param>
        /// <param name="templateService">The template service.</param>
        [UsedImplicitly]
        public TemplateSynchronizer(
            IFileService fileService,
            ITemplateService templateService)
        {
            Ensure.That(fileService).IsNotNull();
            Ensure.That(templateService).IsNotNull();

            _fileService = fileService;
            _templateService = templateService;
        }

        /// <inheritdoc />
        public void Run()
        {
            AddNewTemplates();
            UpdateTemplates();
        }

        /// <summary>
        /// Gets the template layout (master) for the specified template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns>The template layout, without extension (.cshtml).</returns>
        internal string GetLayout(ITemplate template)
        {
            if (string.IsNullOrWhiteSpace(template.Content))
            {
                return null;
            }

            var match = _layoutRegex.Match(template.Content);

            return !match.Success ? null : match.Value;
        }

        /// <summary>
        /// Gets the templates to add.
        /// </summary>
        /// <returns>The templates to add.</returns>
        internal IEnumerable<string> GetTemplatesToAdd()
        {
            var templates = _fileService.GetTemplates().Select(template => template.Alias);
            var templatePaths = _templateService.TemplatePaths;

            /*
             * Templates created using the back office might not have a conventional Umbraco alias in regards
             * to case. We must therefore not take case into account when getting templates to add.
             */
            return from templatePath in templatePaths
                   let alias = Path.GetFileNameWithoutExtension(templatePath)
                   where !templates.Contains(alias, StringComparer.InvariantCultureIgnoreCase)
                   select templatePath;
        }

        /// <summary>
        /// Gets the templates to add.
        /// </summary>
        /// <param name="templatePaths">The template paths.</param>
        /// <returns>The templates to add.</returns>
        internal IEnumerable<ITemplate> GetTemplatesToAdd(IEnumerable<string> templatePaths)
        {
            var templates = templatePaths.Select(_templateService.GetTemplate);

            return templates;
        }

        /// <summary>
        /// Adds new templates.
        /// </summary>
        private void AddNewTemplates()
        {
            var templatesToAdd = GetTemplatesToAdd(GetTemplatesToAdd()).ToArray();

            if (!templatesToAdd.Any())
            {
                return;
            }

            _fileService.SaveTemplate(templatesToAdd);
        }

        private ITemplate GetMasterTemplate(IEnumerable<ITemplate> templates, ITemplate template)
        {
            var layout = GetLayout(template);

            /*
             * Templates created using the back office might not have a conventional Umbraco alias in regards
             * to case. We must therefore not take case into account when getting master templates.
             */
            return layout == null ? null : templates.FirstOrDefault(t => t.Alias.Equals(layout, StringComparison.InvariantCultureIgnoreCase));
        }

        private IEnumerable<ITemplate> GetTemplatesToUpdate(IEnumerable<ITemplate> templates)
        {
            /*
             * It's possible a template is changed through VS or the file system, changing the layout assignment.
             * This has nothing to do with whether the template is new or not; if the layout is changed Umbraco
             * needs to be synchronized.
             */
            var t = templates.ToArray();

            foreach (var template in t)
            {
                var masterTemplate = GetMasterTemplate(t, template);

                if (masterTemplate == null)
                {
                    // No layout/master has been set, or a matching master template does not exist.
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(template.MasterTemplateAlias) && template.MasterTemplateAlias.Equals(masterTemplate.Alias, StringComparison.InvariantCultureIgnoreCase))
                {
                    // The current master template matches the layout/master. There's no need to update the template.
                    continue;
                }

                template.SetMasterTemplate(masterTemplate);

                yield return template;
            }
        }

        private void UpdateTemplates()
        {
            var templates = _fileService.GetTemplates();

            if (templates == null)
            {
                return;
            }

            var templatesToUpdate = GetTemplatesToUpdate(templates).ToArray();

            if (!templatesToUpdate.Any())
            {
                return;
            }

            _fileService.SaveTemplate(templatesToUpdate);
        }
    }
}