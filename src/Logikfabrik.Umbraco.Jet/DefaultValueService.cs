﻿// <copyright file="DefaultValueService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Data;
    using global::Umbraco.Core.Models;
    using Logging;

    /// <summary>
    /// The <see cref="DefaultValueService" /> class.
    /// </summary>
    public class DefaultValueService
    {
        private readonly ITypeResolver _typeResolver;
        private readonly ContentTypeModelFinder<DocumentType, DocumentTypeAttribute, IContentType> _documentTypeModelFinder;
        private readonly ContentTypeModelFinder<MediaType, MediaTypeAttribute, IMediaType> _mediaTypeModelFinder;
        private readonly ContentTypeModelFinder<MemberType, MemberTypeAttribute, IMemberType> _memberTypeModelFinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueService" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        public DefaultValueService(ILogService logService, ITypeResolver typeResolver, ITypeRepository typeRepository)
        {
            if (logService == null)
            {
                throw new ArgumentNullException(nameof(logService));
            }

            if (typeRepository == null)
            {
                throw new ArgumentNullException(nameof(typeRepository));
            }

            _typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));

            _documentTypeModelFinder = new ContentTypeModelFinder<DocumentType, DocumentTypeAttribute, IContentType>(logService, typeRepository);
            _mediaTypeModelFinder = new ContentTypeModelFinder<MediaType, MediaTypeAttribute, IMediaType>(logService, typeRepository);
            _memberTypeModelFinder = new ContentTypeModelFinder<MemberType, MemberTypeAttribute, IMemberType>(logService, typeRepository);
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
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
        public void SetDefaultValues(IContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var model = _documentTypeModelFinder.Find(content.ContentType, _typeResolver.DocumentTypes.ToArray()).SingleOrDefault();

            if (model == null)
            {
                return;
            }

            SetDefaultValues(content, model);
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
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
        public void SetDefaultValues(IMedia content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var model = _mediaTypeModelFinder.Find(content.ContentType, _typeResolver.MediaTypes.ToArray()).SingleOrDefault();

            if (model == null)
            {
                return;
            }

            SetDefaultValues(content, model);
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
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
        public void SetDefaultValues(IMember content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var model = _memberTypeModelFinder.Find(content.ContentType, _typeResolver.MemberTypes.ToArray()).SingleOrDefault();

            if (model == null)
            {
                return;
            }

            SetDefaultValues(content, model);
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <typeparam name="T">The model type.</typeparam>
        /// <param name="content">The content to set default values for.</param>
        /// <param name="model">The model.</param>
        private static void SetDefaultValues<T>(IContentBase content, ContentTypeModel<T> model)
            where T : ContentTypeModelAttribute
        {
            foreach (var property in model.Properties)
            {
                SetDefaultValue(content, property);
            }
        }

        /// <summary>
        /// Sets the default value.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        /// <param name="model">The model.</param>
        private static void SetDefaultValue(IContentBase content, PropertyType model)
        {
            if (!model.HasDefaultValue)
            {
                return;
            }

            var value = content.GetValue(model.Alias);

            if (value != null)
            {
                return;
            }

            if (!CanSetDefaultValue(model.Type, model.DefaultValue))
            {
                return;
            }

            content.SetValue(model.Alias, model.DefaultValue);
        }

        private static bool CanSetDefaultValue(Type propertyType, object defaultValue)
        {
            return defaultValue == null
                ? IsNullableType(propertyType)
                : IsCastableTo(defaultValue.GetType(), propertyType);
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        private static bool IsCastableTo(Type fromType, Type toType)
        {
            if (toType.IsAssignableFrom(fromType))
            {
                return true;
            }

            var methods = fromType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(
                    m => m.ReturnType == toType &&
                         (m.Name == "op_Implicit" ||
                          m.Name == "op_Explicit"));

            return methods.Any();
        }
    }
}