//----------------------------------------------------------------------------------
// <copyright file="TemplateSynchronizationService.cs" company="Logikfabrik">
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
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    public class TemplateSynchronizationService : ISynchronizationService
    {
        private readonly IFileService fileService;
        private readonly ITemplateService templateService;

        public TemplateSynchronizationService()
            : this(
                ApplicationContext.Current.Services.FileService,
                TemplateService.Instance)
        {
        }

        public TemplateSynchronizationService(
            IFileService fileService,
            ITemplateService templateService)
        {
            if (fileService == null)
            {
                throw new ArgumentNullException("fileService");
            }

            if (templateService == null)
            {
                throw new ArgumentNullException("templateService");
            }

            this.fileService = fileService;
            this.templateService = templateService;
        }
        
        public void Synchronize()
        {
            var templatesToAdd = this.GetTemplatesToAdd(this.GetTemplatesToAdd()).ToArray();

            if (!templatesToAdd.Any())
            {
                return;
            }

            this.AddTemplates(templatesToAdd);
        }

        /// <summary>
        /// Gets the templates to add.
        /// </summary>
        /// <returns>Templates to add (paths).</returns>
        internal IEnumerable<string> GetTemplatesToAdd()
        {
            var templates =
                this.fileService.GetTemplates().Where(template => template != null).Select(template => template.Alias);
            var templatePaths = this.templateService.TemplatePaths;

            return from templatePath in templatePaths
                   let alias = Path.GetFileNameWithoutExtension(templatePath).Alias()
                   where !templates.Contains(alias)
                   select templatePath;
        }

        /// <summary>
        /// Gets the templates to add.
        /// </summary>
        /// <param name="templatePaths">Paths to the templates to add.</param>
        /// <returns>Templates to add (templates).</returns>
        internal IEnumerable<ITemplate> GetTemplatesToAdd(IEnumerable<string> templatePaths)
        {
            if (templatePaths == null)
            {
                throw new ArgumentNullException("templatePaths");
            }

            var templates = templatePaths.Select(this.templateService.GetTemplate);

            return templates;
        }

        private void AddTemplates(IEnumerable<ITemplate> templates)
        {
            if (templates == null)
            {
                throw new ArgumentNullException("templates");
            }

            this.fileService.SaveTemplate(templates);
        }
    }
}
