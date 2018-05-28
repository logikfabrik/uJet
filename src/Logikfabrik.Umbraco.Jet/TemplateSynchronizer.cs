// <copyright file="TemplateSynchronizer.cs" company="Logikfabrik">
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
    using global::Umbraco.Core.Services;
    using JetBrains.Annotations;

    /// <summary>
    /// Synchronizer for templates (Razor views).
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class TemplateSynchronizer : ISynchronizer
    {
        private readonly IFileService _fileService;
        private readonly ITemplateService _templateService;

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

        private static ITemplate GetMasterTemplate(ITemplate template, IEnumerable<ITemplate> templates)
        {
            var layout = template.GetLayout();

            /*
             * Templates created using the back office might not have a conventional Umbraco alias in regards
             * to case. We must therefore not take case into account when getting master templates.
             */
            return layout == null ? null : templates.FirstOrDefault(t => t.Alias.Equals(layout, StringComparison.InvariantCultureIgnoreCase));
        }

        private static IEnumerable<ITemplate> GetTemplatesToUpdate(IEnumerable<ITemplate> templates)
        {
            /*
             * It's possible a template is changed through VS or the file system, changing the layout assignment.
             * This has nothing to do with whether the template is new or not; if the layout is changed Umbraco
             * needs to be synchronized.
             */
            var t = templates.ToArray();

            foreach (var template in t)
            {
                var masterTemplate = GetMasterTemplate(template, t);

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

        private IEnumerable<string> GetPathsForTemplatesToAdd()
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

        private IEnumerable<ITemplate> GetTemplatesToAdd(IEnumerable<string> templatePaths)
        {
            var templates = templatePaths.Select(_templateService.GetTemplate);

            return templates;
        }

        private void AddNewTemplates()
        {
            var templatesToAdd = GetTemplatesToAdd(GetPathsForTemplatesToAdd()).ToArray();

            if (!templatesToAdd.Any())
            {
                return;
            }

            _fileService.SaveTemplate(templatesToAdd);
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