// <copyright file="ContentService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using System.Reflection;
    using Converters;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="ContentService" /> class.
    /// </summary>
    public abstract class ContentService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentService" /> class.
        /// </summary>
        /// <param name="umbracoHelperWrapper">The Umbraco helper wrapper.</param>
        /// <param name="typeService">The type service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="umbracoHelperWrapper" /> or <paramref name="typeService" /> are <c>null</c>.</exception>
        protected ContentService(IUmbracoHelperWrapper umbracoHelperWrapper, ITypeService typeService)
        {
            if (umbracoHelperWrapper == null)
            {
                throw new ArgumentNullException(nameof(umbracoHelperWrapper));
            }

            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            UmbracoHelper = umbracoHelperWrapper;
            TypeService = typeService;
        }

        /// <summary>
        /// Gets the type service.
        /// </summary>
        /// <value>
        /// The type service.
        /// </value>
        protected ITypeService TypeService { get; }

        /// <summary>
        /// Gets the Umbraco helper.
        /// </summary>
        /// <value>
        /// The Umbraco helper.
        /// </value>
        protected IUmbracoHelperWrapper UmbracoHelper { get; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="contentType">The type of content.</param>
        /// <returns>The content.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> or <paramref name="contentType" /> are <c>null</c>.</exception>
        protected object GetContent(IPublishedContent content, Type contentType)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            var model = Activator.CreateInstance(contentType);

            MapByConvention(content, model);

            foreach (var property in content.Properties)
            {
                MapProperty(model, property.PropertyTypeAlias, property.Value);
            }

            return model;
        }

        /// <summary>
        /// Maps content by convention.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="model">The model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> or <paramref name="model" /> are <c>null</c>.</exception>
        private static void MapByConvention(IPublishedContent content, object model)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            MapProperty(model, GetPropertyName(() => content.Id), content.Id);
            MapProperty(model, GetPropertyName(() => content.Url), content.Url);
            MapProperty(model, GetPropertyName(() => content.Name), content.Name);
            MapProperty(model, GetPropertyName(() => content.CreateDate), content.CreateDate);
            MapProperty(model, GetPropertyName(() => content.UpdateDate), content.UpdateDate);
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>The property name.</returns>
        private static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }

        /// <summary>
        /// Gets a property UI hint.
        /// </summary>
        /// <param name="property">The property to get UI hint for.</param>
        /// <returns>A property UI hint.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="property" /> is <c>null</c>.</exception>
        private static string GetPropertyUIHint(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var attribute = property.GetCustomAttribute<UIHintAttribute>();

            var uiHint = attribute?.UIHint;

            return uiHint;
        }

        /// <summary>
        /// Maps the property.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="model" /> is <c>null</c>.</exception>
        private static void MapProperty(object model, string name, object value)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var p = model.GetType()
                .GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (p == null || !p.CanWrite)
            {
                return;
            }

            if (value == null && Nullable.GetUnderlyingType(p.PropertyType) != null)
            {
                p.SetValue(model, null);
            }
            else if (value != null)
            {
                if (p.PropertyType.IsInstanceOfType(value))
                {
                    p.SetValue(model, value);
                }
                else
                {
                    var uiHint = GetPropertyUIHint(p);
                    var converter = PropertyValueConverters.GetConverter(uiHint, value.GetType(), p.PropertyType);

                    if (converter != null)
                    {
                        p.SetValue(model, converter.Convert(value, p.PropertyType));
                    }
                }
            }
        }
    }
}
