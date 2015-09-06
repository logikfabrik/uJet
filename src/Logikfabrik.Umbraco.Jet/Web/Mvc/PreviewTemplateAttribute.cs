// <copyright file="PreviewTemplateAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;

    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]
    public class PreviewTemplateAttribute : Attribute
    {
        public const string TemplateName = "Preview";
    }
}
