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
    /// The default value service. Responsible for setting default values for types.
    /// </summary>
    public class DefaultValueService
    {
        private readonly ITypeService typeService;

        public DefaultValueService()
            : this(TypeService.Instance)
        {
        }

        public DefaultValueService(ITypeService typeService)
        {
            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            this.typeService = typeService;
        }

        public void SetDefaultValues(IEnumerable<IContent> contents)
        {
            if (contents == null)
            {
                throw new ArgumentNullException(nameof(contents));
            }

            foreach (var content in contents)
            {
                SetDefaultValues(content);
            }
        }

        public void SetDefaultValues(IContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var type =
                typeService.DocumentTypes.FirstOrDefault(t => t.Name.Alias() == content.ContentType.Alias);

            if (type == null)
            {
                return;
            }

            // ReSharper disable once RedundantNameQualifier
            // ReSharper disable once ArrangeStaticMemberQualifier
            DefaultValueService.SetDefaultValues(content, new DocumentType(type));
        }

        public void SetDefaultValues(IEnumerable<IMedia> contents)
        {
            if (contents == null)
            {
                throw new ArgumentNullException(nameof(contents));
            }

            foreach (var content in contents)
            {
                SetDefaultValues(content);
            }
        }

        public void SetDefaultValues(IMedia content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var type =
                typeService.MediaTypes.FirstOrDefault(t => t.Name.Alias() == content.ContentType.Alias);

            if (type == null)
            {
                return;
            }

            // ReSharper disable once RedundantNameQualifier
            // ReSharper disable once ArrangeStaticMemberQualifier
            DefaultValueService.SetDefaultValues(content, new MediaType(type));
        }

        private static void SetDefaultValues<T>(IContentBase content, ContentType<T> contentType)
            where T : ContentTypeAttribute
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            foreach (var property in contentType.Properties)
            {
                SetDefaultValue(content, property);
            }
        }

        private static void SetDefaultValue(IContentBase content, ContentTypeProperty contentTypeProperty)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (contentTypeProperty == null)
            {
                throw new ArgumentNullException(nameof(contentTypeProperty));
            }

            if (!contentTypeProperty.HasDefaultValue)
            {
                return;
            }

            var value = content.GetValue(contentTypeProperty.Alias);

            if (value != null)
            {
                return;
            }

            if (!CanSetDefaultValue(contentTypeProperty.Type, contentTypeProperty.DefaultValue))
            {
                return;
            }

            content.SetValue(contentTypeProperty.Alias, contentTypeProperty.DefaultValue);
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
                throw new ArgumentNullException(nameof(@from));
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