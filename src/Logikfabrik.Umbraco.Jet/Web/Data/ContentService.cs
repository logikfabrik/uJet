// <copyright file="ContentService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using System.Linq.Expressions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="ContentService" /> class.
    /// </summary>
    public abstract class ContentService
    {
        private readonly ContentMapper _contentMapper = new ContentMapper();

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
        /// Gets the mapped content.
        /// </summary>
        /// <param name="content">The content to map.</param>
        /// <param name="modelType">The model type to map to.</param>
        /// <returns>The mapped content.</returns>
        protected object GetMappedContent(IPublishedContent content, Type modelType)
        {
            var model = Activator.CreateInstance(modelType);

            MapByConvention(content, model);

            foreach (var property in content.Properties)
            {
                MapProperty(model, property.PropertyTypeAlias, property.Value);
            }

            return model;
        }

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
    }
}