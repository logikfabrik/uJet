// <copyright file="ContentService{TModelAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using System.Linq.Expressions;
    using EnsureThat;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="ContentService{TModelAttribute}" /> class.
    /// </summary>
    /// <typeparam name="TModelAttribute">The model attribute type.</typeparam>
    public abstract class ContentService<TModelAttribute>
        where TModelAttribute : ContentTypeModelAttribute
    {
        private readonly ContentMapper _contentMapper = new ContentMapper();

        /// <summary>
        /// Gets a model for the content with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The content model type.</typeparam>
        /// <param name="id">The content identifier.</param>
        /// <returns>A model for the content with the specified identifier.</returns>
        public T GetContent<T>(int id)
            where T : class, new()
        {
            EnsureArg.IsTrue(typeof(T).IsModelType<TModelAttribute>(), nameof(T));

            return GetMappedContent<T>(GetContent(id));
        }

        /// <summary>
        /// Gets a model for the specified content.
        /// </summary>
        /// <typeparam name="T">The content model type.</typeparam>
        /// <param name="content">The content.</param>
        /// <returns>A model for the specified content.</returns>
        public T GetContent<T>(IPublishedContent content)
            where T : class, new()
        {
            EnsureArg.IsTrue(typeof(T).IsModelType<TModelAttribute>(), nameof(T));
            EnsureArg.IsNotNull(content);

            return GetMappedContent<T>(content);
        }

        /// <summary>
        /// Gets a model for the content with the specified identifier.
        /// </summary>
        /// <param name="id">The content identifier.</param>
        /// <param name="contentModelType">The content model type.</param>
        /// <returns>A model for the content with the specified identifier.</returns>
        public object GetContent(int id, Type contentModelType)
        {
            EnsureArg.IsNotNull(contentModelType);
            EnsureArg.IsTrue(contentModelType.IsModelType<TModelAttribute>(), nameof(contentModelType));

            return GetMappedContent(GetContent(id), contentModelType);
        }

        /// <summary>
        /// Gets a model for the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="contentModelType">The content model type.</param>
        /// <returns>A model for the specified content.</returns>
        public object GetContent(IPublishedContent content, Type contentModelType)
        {
            EnsureArg.IsNotNull(content);
            EnsureArg.IsNotNull(contentModelType);
            EnsureArg.IsTrue(contentModelType.IsModelType<TModelAttribute>(), nameof(contentModelType));

            return GetMappedContent(content, contentModelType);
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>The property name.</returns>
        protected static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }

        /// <summary>
        /// Gets the content with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The content with the specified identifier.</returns>
        protected abstract IPublishedContent GetContent(int id);

        /// <summary>
        /// Maps the property.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The property value.</param>
        protected void MapProperty(object model, string propertyName, object propertyValue)
        {
            _contentMapper.MapProperty(model, propertyName, propertyValue);
        }

        /// <summary>
        /// Maps content by convention.
        /// </summary>
        /// <param name="content">The content to map.</param>
        /// <param name="model">The model to map to.</param>
        protected virtual void MapByConvention(IPublishedContent content, object model)
        {
            MapProperty(model, GetPropertyName(() => content.Id), content.Id);
            MapProperty(model, GetPropertyName(() => content.Url), content.Url);
            MapProperty(model, GetPropertyName(() => content.Name), content.Name);

            MapProperty(model, GetPropertyName(() => content.CreateDate), content.CreateDate);
            MapProperty(model, GetPropertyName(() => content.UpdateDate), content.UpdateDate);

            MapProperty(model, GetPropertyName(() => content.CreatorId), content.CreatorId);
            MapProperty(model, GetPropertyName(() => content.CreatorName), content.CreatorName);

            MapProperty(model, GetPropertyName(() => content.WriterId), content.WriterId);
            MapProperty(model, GetPropertyName(() => content.WriterName), content.WriterName);
        }

        private T GetMappedContent<T>(IPublishedContent content)
            where T : class, new()
        {
            var model = new T();

            MapByConvention(content, model);

            foreach (var property in content.Properties)
            {
                MapProperty(model, property.PropertyTypeAlias, property.Value);
            }

            return model;
        }

        private object GetMappedContent(IPublishedContent content, Type contentModelType)
        {
            var model = Activator.CreateInstance(contentModelType);

            MapByConvention(content, model);

            if (content.Properties == null)
            {
                return model;
            }

            foreach (var property in content.Properties)
            {
                MapProperty(model, property.PropertyTypeAlias, property.Value);
            }

            return model;
        }
    }
}