// <copyright file="DocumentType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Models
{
    [DocumentType(
        "85384e6c-9001-4c02-8b0e-eb76f1edabc7",
        "DocumentType",
        Description = "Description",
        AllowedAsRoot = true,
        AllowedChildNodeTypes = new[] { typeof(IDocumentTypeB), typeof(ContentType) },
        CompositionNodeTypes = new[] { typeof(CompositionDocumentTypeA), typeof(CompositionDocumentTypeB) },
        DefaultTemplate = "DefaultTemplate",
        Templates = new string[] { })]
    public class DocumentType : ContentType
    {
    }
}