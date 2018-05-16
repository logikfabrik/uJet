// <copyright file="IContentService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="IContentService" /> interface.
    /// </summary>
    public interface IContentService
    {
        /// <summary>
        /// Gets a model for the content with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The content model type.</typeparam>
        /// <param name="id">The content identifier.</param>
        /// <returns>A model for the content with the specified identifier.</returns>
        T GetContent<T>(int id)
            where T : class, new();

        /// <summary>
        /// Gets a model for the specified content.
        /// </summary>
        /// <typeparam name="T">The content model type.</typeparam>
        /// <param name="content">The content.</param>
        /// <returns>A model for the specified content.</returns>
        T GetContent<T>(IPublishedContent content)
            where T : class, new();

        /// <summary>
        /// Gets a model for the content with the specified identifier.
        /// </summary>
        /// <param name="id">The content identifier.</param>
        /// <param name="contentModelType">The content model type.</param>
        /// <returns>A model for the content with the specified identifier.</returns>
        object GetContent(int id, Type contentModelType);

        /// <summary>
        /// Gets a model for the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="contentModelType">The content model type.</param>
        /// <returns>A model for the specified content.</returns>
        object GetContent(IPublishedContent content, Type contentModelType);
    }
}