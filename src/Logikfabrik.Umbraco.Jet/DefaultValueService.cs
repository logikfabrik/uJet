﻿// <copyright file="DefaultValueService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using EnsureThat;
    using global::Umbraco.Core.Models;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="DefaultValueService" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class DefaultValueService : IDefaultValueService
    {
        private readonly IContentTypeModelFinder<DocumentType, DocumentTypeAttribute, IContentType> _documentTypeModelFinder;
        private readonly IContentTypeModelFinder<MediaType, MediaTypeAttribute, IMediaType> _mediaTypeModelFinder;
        private readonly IContentTypeModelFinder<MemberType, MemberTypeAttribute, IMemberType> _memberTypeModelFinder;
        private readonly IModelService _modelService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueService" /> class.
        /// </summary>
        /// <param name="documentTypeModelFinder">The document type model finder.</param>
        /// <param name="mediaTypeModelFinder">The media type model finder.</param>
        /// <param name="memberTypeModelFinder">The member type model finder.</param>
        /// <param name="modelService">The model service.</param>
        [UsedImplicitly]
        public DefaultValueService(
            IContentTypeModelFinder<DocumentType, DocumentTypeAttribute, IContentType> documentTypeModelFinder,
            IContentTypeModelFinder<MediaType, MediaTypeAttribute, IMediaType> mediaTypeModelFinder,
            IContentTypeModelFinder<MemberType, MemberTypeAttribute, IMemberType> memberTypeModelFinder,
            IModelService modelService)
        {
            Ensure.That(documentTypeModelFinder).IsNotNull();
            Ensure.That(mediaTypeModelFinder).IsNotNull();
            Ensure.That(memberTypeModelFinder).IsNotNull();
            Ensure.That(modelService).IsNotNull();

            _documentTypeModelFinder = documentTypeModelFinder;
            _mediaTypeModelFinder = mediaTypeModelFinder;
            _memberTypeModelFinder = memberTypeModelFinder;
            _modelService = modelService;
        }

        /// <inheritdoc />
        public void SetDefaultValues(IEnumerable<IContent> content)
        {
            Ensure.That(content).IsNotNull();

            foreach (var c in content)
            {
                SetDefaultValues(c);
            }
        }

        /// <inheritdoc />
        public void SetDefaultValues(IEnumerable<IMedia> content)
        {
            Ensure.That(content).IsNotNull();

            foreach (var c in content)
            {
                SetDefaultValues(c);
            }
        }

        /// <inheritdoc />
        public void SetDefaultValues(IEnumerable<IMember> content)
        {
            Ensure.That(content).IsNotNull();

            foreach (var c in content)
            {
                SetDefaultValues(c);
            }
        }

        private static void SetDefaultValues<T>(IContentBase content, ContentTypeModel<T> model)
            where T : ContentTypeModelTypeAttribute
        {
            foreach (var property in model.Properties)
            {
                SetDefaultValue(content, property);
            }
        }

        private static void SetDefaultValue(IContentBase content, PropertyTypeModel model)
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

        private void SetDefaultValues(IContent content)
        {
            var model = _documentTypeModelFinder.Find(content.ContentType, _modelService.DocumentTypes.ToArray()).SingleOrDefault();

            if (model == null)
            {
                return;
            }

            SetDefaultValues(content, model);
        }

        private void SetDefaultValues(IMedia content)
        {
            var model = _mediaTypeModelFinder.Find(content.ContentType, _modelService.MediaTypes.ToArray()).SingleOrDefault();

            if (model == null)
            {
                return;
            }

            SetDefaultValues(content, model);
        }

        private void SetDefaultValues(IMember content)
        {
            var model = _memberTypeModelFinder.Find(content.ContentType, _modelService.MemberTypes.ToArray()).SingleOrDefault();

            if (model == null)
            {
                return;
            }

            SetDefaultValues(content, model);
        }
    }
}