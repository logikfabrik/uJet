// <copyright file="DocumentService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using EnsureThat;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="DocumentService" /> class. Service for mapping instances of <see cref="IPublishedContent" /> to document models.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class DocumentService : ContentService<DocumentTypeAttribute>
    {
        private readonly IUmbracoHelperWrapper _umbracoHelperWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService" /> class.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InheritdocConsiderUsage
        public DocumentService()
            : this(new UmbracoHelperWrapper())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService" /> class.
        /// </summary>
        /// <param name="umbracoHelperWrapper">The Umbraco helper wrapper.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public DocumentService(IUmbracoHelperWrapper umbracoHelperWrapper)
        {
            EnsureArg.IsNotNull(umbracoHelperWrapper);

            _umbracoHelperWrapper = umbracoHelperWrapper;
        }

        /// <inheritdoc />
        protected override IPublishedContent GetContent(int id)
        {
            return _umbracoHelperWrapper.TypedDocument(id);
        }

        /// <inheritdoc />
        protected override void MapByConvention(IPublishedContent content, object model)
        {
            base.MapByConvention(content, model);

            MapProperty(model, GetPropertyName(() => content.DocumentTypeId), content.DocumentTypeId);
            MapProperty(model, GetPropertyName(() => content.DocumentTypeAlias), content.DocumentTypeAlias);
        }
    }
}