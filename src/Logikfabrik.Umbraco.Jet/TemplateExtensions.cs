// <copyright file="TemplateExtensions.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Text.RegularExpressions;
    using EnsureThat;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// Extension methods for templates.
    /// </summary>
    public static class TemplateExtensions
    {
        private static readonly Regex LayoutRegex = new Regex("(?s:(?<=@{.*Layout\\s*=\\s*\").*(?=.cshtml\";.*}))");

        /// <summary>
        /// Gets the template layout (master) of the specified template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns>The template layout, without extension (.cshtml); or <c>null</c> if no template layout.</returns>
        public static string GetLayout(this ITemplate template)
        {
            Ensure.That(template).IsNotNull();

            if (string.IsNullOrWhiteSpace(template.Content))
            {
                return null;
            }

            var match = LayoutRegex.Match(template.Content);

            return !match.Success ? null : match.Value;
        }
    }
}
