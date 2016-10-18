// <copyright file="DocumentTypeWithAlias.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Models
{
    [DocumentType(
        "870E2F78-27BA-47F9-BA5A-EF8D9CAEEBE4",
        "DocumentType",
        Description = "Description",
        AllowedAsRoot = true,
        Alias = "customAlias",
        AllowedChildNodeTypes = new[] { typeof(IDocumentTypeB), typeof(ContentType) },
        CompositionNodeTypes = new[] { typeof(CompositionDocumentTypeA), typeof(CompositionDocumentTypeB) },
        DefaultTemplate = "DefaultTemplate",
        IsContainer = true,
        Templates = new string[] { })]
    public class DocumentTypeWithAlias : ContentType
    {
    }
}