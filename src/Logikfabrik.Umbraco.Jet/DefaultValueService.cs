// <copyright file="DefaultValueService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="DefaultValueService" /> class.
    /// </summary>
    public class DefaultValueService
    {
        private readonly ITypeService _typeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueService" /> class.
        /// </summary>
        public DefaultValueService()
            : this(TypeService.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueService" /> class.
        /// </summary>
        /// <param name="typeService">The type service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeService" /> is <c>null</c>.</exception>
        public DefaultValueService(ITypeService typeService)
        {
            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            _typeService = typeService;
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> is <c>null</c>.</exception>
        public void SetDefaultValues(IEnumerable<IContent> content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            foreach (var c in content)
            {
                SetDefaultValues(c);
            }
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> is <c>null</c>.</exception>
        public void SetDefaultValues(IContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var type = _typeService.DocumentTypes.FirstOrDefault(t => t.Name.Alias() == content.ContentType.Alias);

            if (type == null)
            {
                return;
            }

            SetDefaultValues(content, new DocumentType(type));
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> is <c>null</c>.</exception>
        public void SetDefaultValues(IEnumerable<IMedia> content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            foreach (var c in content)
            {
                SetDefaultValues(c);
            }
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> is <c>null</c>.</exception>
        public void SetDefaultValues(IMedia content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var type = _typeService.MediaTypes.FirstOrDefault(t => t.Name.Alias() == content.ContentType.Alias);

            if (type == null)
            {
                return;
            }

            SetDefaultValues(content, new MediaType(type));
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> is <c>null</c>.</exception>
        public void SetDefaultValues(IEnumerable<IMember> content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            foreach (var c in content)
            {
                SetDefaultValues(c);
            }
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> is <c>null</c>.</exception>
        public void SetDefaultValues(IMember content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var type = _typeService.MediaTypes.FirstOrDefault(t => t.Name.Alias() == content.ContentType.Alias);

            if (type == null)
            {
                return;
            }

            SetDefaultValues(content, new MediaType(type));
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <typeparam name="T">The content type.</typeparam>
        /// <param name="content">The content to set default values for.</param>
        /// <param name="baseType">The base type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" />, or <paramref name="baseType" /> are <c>null</c>.</exception>
        private static void SetDefaultValues<T>(IContentBase content, BaseType<T> baseType)
            where T : BaseTypeAttribute
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (baseType == null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }

            foreach (var property in baseType.Properties)
            {
                SetDefaultValue(content, property);
            }
        }

        /// <summary>
        /// Sets the default value.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        /// <param name="typeProperty">The type property.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" />, or <paramref name="typeProperty" /> are <c>null</c>.</exception>
        private static void SetDefaultValue(IContentBase content, TypeProperty typeProperty)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (typeProperty == null)
            {
                throw new ArgumentNullException(nameof(typeProperty));
            }

            if (!typeProperty.HasDefaultValue)
            {
                return;
            }

            var value = content.GetValue(typeProperty.Alias);

            if (value != null)
            {
                return;
            }

            if (!CanSetDefaultValue(typeProperty.Type, typeProperty.DefaultValue))
            {
                return;
            }

            content.SetValue(typeProperty.Alias, typeProperty.DefaultValue);
        }

        private static bool CanSetDefaultValue(Type propertyType, object defaultValue)
        {
            if (propertyType == null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            return defaultValue == null
                ? IsNullableType(propertyType)
                : IsCastableTo(defaultValue.GetType(), propertyType);
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        private static bool IsCastableTo(Type from, Type to)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (to.IsAssignableFrom(from))
            {
                return true;
            }

            var methods = from.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(
                    m => m.ReturnType == to &&
                         (m.Name == "op_Implicit" ||
                          m.Name == "op_Explicit"));

            return methods.Any();
        }
    }
}