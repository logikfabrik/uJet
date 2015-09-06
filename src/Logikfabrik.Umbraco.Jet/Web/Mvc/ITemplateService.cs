// <copyright file="ITemplateService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;

    public interface ITemplateService
    {
        /// <summary>
        /// Gets the paths for the templates in the views folder.
        /// </summary>
        IEnumerable<string> TemplatePaths { get; }

        /// <summary>
        /// Gets the content of a template.
        /// </summary>
        /// <param name="templatePath">The template path.</param>
        /// <returns>The content of the template.</returns>
        string GetContent(string templatePath);

        /// <summary>
        /// Gets a template.
        /// </summary>
        /// <param name="templatePath">The template path.</param>
        /// <returns>A template.</returns>
        ITemplate GetTemplate(string templatePath);
    }
}
