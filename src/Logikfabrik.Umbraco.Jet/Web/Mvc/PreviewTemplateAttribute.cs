// <copyright file="PreviewTemplateAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;

    /// <summary>
    /// The <see cref="PreviewTemplateAttribute" /> class.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]

    // ReSharper disable once InheritdocConsiderUsage
    public class PreviewTemplateAttribute : Attribute
    {
        /// <summary>
        /// The template name.
        /// </summary>
        public const string TemplateName = "Preview";
    }
}
