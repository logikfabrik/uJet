// <copyright file="ITemplateService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="ITemplateService" /> interface.
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// Gets the template paths for templates in the view folder.
        /// </summary>
        /// <value>
        /// The template paths.
        /// </value>
        IEnumerable<string> TemplatePaths { get; }

        /// <summary>
        /// Gets the template content.
        /// </summary>
        /// <param name="templatePath">The template path.</param>
        /// <returns>The content of the template.</returns>
        string GetContent(string templatePath);

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <param name="templatePath">The template path.</param>
        /// <returns>The template.</returns>
        ITemplate GetTemplate(string templatePath);
    }
}
