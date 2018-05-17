// <copyright file="IContentTypeFinder{TModel,TModelTypeAttribute,TContentType}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="IContentTypeFinder{TModel, TModelTypeAttribute, TContentType}" /> interface.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelTypeAttribute">The model type attribute type.</typeparam>
    /// <typeparam name="TContentType">The content type.</typeparam>
    public interface IContentTypeFinder<in TModel, TModelTypeAttribute, TContentType>
        where TModel : ContentTypeModel<TModelTypeAttribute>
        where TModelTypeAttribute : ContentTypeModelTypeAttribute
        where TContentType : class, IContentTypeBase
    {
        /// <summary>
        /// Finds the models with a model type matching any of the specified model types, and then the content types matching any of those models.
        /// </summary>
        /// <param name="modelTypeNeedles">The model types to find the content types for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <param name="contentTypesHaystack">The haystack of content types.</param>
        /// <returns>The content types found.</returns>
        TContentType[] Find(Type[] modelTypeNeedles, TModel[] modelsHaystack, TContentType[] contentTypesHaystack);

        /// <summary>
        /// Finds the models with a model type matching the specified model type, and then the content types matching any of those models.
        /// </summary>
        /// <param name="modelTypeNeedle">The model type to find the content types for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <param name="contentTypesHaystack">The haystack of content types.</param>
        /// <returns>The content types found.</returns>
        TContentType[] Find(Type modelTypeNeedle, TModel[] modelsHaystack, TContentType[] contentTypesHaystack);

        /// <summary>
        /// Finds all models with a model type matching any of the specified model types, and then the content types matching any of those models.
        /// </summary>
        /// <param name="modelTypeNeedles">The model types to find the content types for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <param name="contentTypesHaystack">The haystack of content types.</param>
        /// <returns>The content types found.</returns>
        TContentType[] FindAll(Type[] modelTypeNeedles, TModel[] modelsHaystack, TContentType[] contentTypesHaystack);

        /// <summary>
        /// Finds all models with a model type matching the specified model type, and then the content types matching any of those models.
        /// </summary>
        /// <param name="modelTypeNeedle">The model type to find the content types for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <param name="contentTypesHaystack">The haystack of content types.</param>
        /// <returns>The content types found.</returns>
        TContentType[] FindAll(Type modelTypeNeedle, TModel[] modelsHaystack, TContentType[] contentTypesHaystack);

        /// <summary>
        /// Finds the content types matching any of the specified models.
        /// </summary>
        /// <param name="modelNeedles">The models to find the content types for.</param>
        /// <param name="contentTypesHaystack">The haystack of content types.</param>
        /// <returns>The content types found.</returns>
        TContentType[] Find(TModel[] modelNeedles, TContentType[] contentTypesHaystack);

        /// <summary>
        /// Finds the content types matching the specified model.
        /// </summary>
        /// <param name="modelNeedle">The model to find the content types for.</param>
        /// <param name="contentTypesHaystack">The haystack of content types.</param>
        /// <returns>The content types found.</returns>
        /// <remarks>A content type is a match if a model needle's ID can be mapped to the ID of the content type, or if a model needle and the content type has the same alias.</remarks>
        TContentType[] Find(TModel modelNeedle, TContentType[] contentTypesHaystack);
    }
}