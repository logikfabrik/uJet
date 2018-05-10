// <copyright file="IContentTypeModelFinder{TModel,TModelAttribute,TContentType}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using global::Umbraco.Core.Models;

    public interface IContentTypeModelFinder<TModel, TModelAttribute, in TContentType> : ITypeModelFinder<TModel, TModelAttribute>
        where TModel : ContentTypeModel<TModelAttribute>
        where TModelAttribute : ContentTypeModelTypeAttribute
        where TContentType : IContentTypeBase
    {
        /// <summary>
        /// Finds the models matching the specified content type.
        /// </summary>
        /// <param name="contentTypeNeedle">The content type to find the models for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <returns>The models found.</returns>
        TModel[] Find(TContentType contentTypeNeedle, TModel[] modelsHaystack);
    }
}