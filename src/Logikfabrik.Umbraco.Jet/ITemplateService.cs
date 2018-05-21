﻿// <copyright file="ITemplateService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="ITemplateService" /> interface.
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// Gets the template paths for templates in the views folder.
        /// </summary>
        /// <value>
        /// The template paths.
        /// </value>
        IEnumerable<string> TemplatePaths { get; }

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <param name="templatePath">The template path.</param>
        /// <returns>The template.</returns>
        ITemplate GetTemplate(string templatePath);
    }
}