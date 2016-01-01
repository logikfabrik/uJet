// <copyright file="DefaultValueService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="DefaultValueService" /> class.
    /// </summary>
    public class DefaultValueService
    {
        private readonly ITypeResolver _typeResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueService" /> class.
        /// </summary>
        public DefaultValueService()
            : this(TypeResolver.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueService" /> class.
        /// </summary>
        /// <param name="typeResolver">The type resolver.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeResolver" /> is <c>null</c>.</exception>
        public DefaultValueService(ITypeResolver typeResolver)
        {
            if (typeResolver == null)
            {
                throw new ArgumentNullException(nameof(typeResolver));
            }

            _typeResolver = typeResolver;
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

            var typeModel = _typeResolver.ResolveTypeModel(content.ContentType);

            if (typeModel == null)
            {
                return;
            }

            SetDefaultValues(content, typeModel);
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

            var typeModel = _typeResolver.ResolveTypeModel(content.ContentType);

            if (typeModel == null)
            {
                return;
            }

            SetDefaultValues(content, typeModel);
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

            var typeModel = _typeResolver.ResolveTypeModel(content.ContentType);

            if (typeModel == null)
            {
                return;
            }

            SetDefaultValues(content, typeModel);
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <typeparam name="T">The type model type.</typeparam>
        /// <param name="content">The content to set default values for.</param>
        /// <param name="typeModel">The type model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" />, or <paramref name="typeModel" /> are <c>null</c>.</exception>
        private static void SetDefaultValues<T>(IContentBase content, BaseType<T> typeModel)
            where T : BaseTypeAttribute
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (typeModel == null)
            {
                throw new ArgumentNullException(nameof(typeModel));
            }

            foreach (var property in typeModel.Properties)
            {
                SetDefaultValue(content, property);
            }
        }

        /// <summary>
        /// Sets the default value.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        /// <param name="propertyTypeModel">The property type model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" />, or <paramref name="propertyTypeModel" /> are <c>null</c>.</exception>
        private static void SetDefaultValue(IContentBase content, TypeProperty propertyTypeModel)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (propertyTypeModel == null)
            {
                throw new ArgumentNullException(nameof(propertyTypeModel));
            }

            if (!propertyTypeModel.HasDefaultValue)
            {
                return;
            }

            var value = content.GetValue(propertyTypeModel.Alias);

            if (value != null)
            {
                return;
            }

            if (!CanSetDefaultValue(propertyTypeModel.Type, propertyTypeModel.DefaultValue))
            {
                return;
            }

            content.SetValue(propertyTypeModel.Alias, propertyTypeModel.DefaultValue);
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