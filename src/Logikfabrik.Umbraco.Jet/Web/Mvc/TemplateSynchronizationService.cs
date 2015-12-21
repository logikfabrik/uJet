// <copyright file="TemplateSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="TemplateSynchronizationService" /> class.
    /// </summary>
    public class TemplateSynchronizationService : ISynchronizationService
    {
        /// <summary>
        /// The file service.
        /// </summary>
        private readonly IFileService _fileService;

        /// <summary>
        /// The template service.
        /// </summary>
        private readonly ITemplateService _templateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateSynchronizationService" /> class.
        /// </summary>
        public TemplateSynchronizationService()
            : this(
                ApplicationContext.Current.Services.FileService,
                TemplateService.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateSynchronizationService" /> class.
        /// </summary>
        /// <param name="fileService">The file service.</param>
        /// <param name="templateService">The template service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fileService" />, or <paramref name="templateService" /> are <c>null</c>.</exception>
        public TemplateSynchronizationService(
            IFileService fileService,
            ITemplateService templateService)
        {
            if (fileService == null)
            {
                throw new ArgumentNullException(nameof(fileService));
            }

            if (templateService == null)
            {
                throw new ArgumentNullException(nameof(templateService));
            }

            _fileService = fileService;
            _templateService = templateService;
        }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public void Synchronize()
        {
            var templatesToAdd = GetTemplatesToAdd(GetTemplatesToAdd()).ToArray();

            if (!templatesToAdd.Any())
            {
                return;
            }

            AddTemplates(templatesToAdd);
        }

        /// <summary>
        /// Gets the templates to add.
        /// </summary>
        /// <returns>The templates to add.</returns>
        internal IEnumerable<string> GetTemplatesToAdd()
        {
            var templates = _fileService.GetTemplates().Where(template => template != null).Select(template => template.Alias);
            var templatePaths = _templateService.TemplatePaths;

            /* Templates created using the back office might not have a conventional Umbraco alias in regards to case. We must
             * therefore not take case into account when getting templates to add.
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="templatePaths" /> is <c>null</c>.</exception>
        internal IEnumerable<ITemplate> GetTemplatesToAdd(IEnumerable<string> templatePaths)
        {
            if (templatePaths == null)
            {
                throw new ArgumentNullException(nameof(templatePaths));
            }

            var templates = templatePaths.Select(_templateService.GetTemplate);

            return templates;
        }

        /// <summary>
        /// Adds the templates.
        /// </summary>
        /// <param name="templates">The templates.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="templates" /> is <c>null</c>.</exception>
        private void AddTemplates(IEnumerable<ITemplate> templates)
        {
            if (templates == null)
            {
                throw new ArgumentNullException(nameof(templates));
            }

            _fileService.SaveTemplate(templates);
        }
    }
}
