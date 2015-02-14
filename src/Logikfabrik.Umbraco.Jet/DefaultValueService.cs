// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Logikfabrik.Umbraco.Jet.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Core.Models;

namespace Logikfabrik.Umbraco.Jet
{
    public class DefaultValueService
    {
        private readonly ITypeService _typeService;

        public DefaultValueService() : this(TypeService.Instance) { }

        public DefaultValueService(ITypeService typeService)
        {
            if (typeService == null)
                throw new ArgumentNullException("typeService");

            _typeService = typeService;
        }

        public void SetDefaultValues(IEnumerable<IContent> contents)
        {
            if (contents == null)
                throw new ArgumentNullException("contents");

            foreach (var content in contents)
                SetDefaultValues(content);
        }

        public void SetDefaultValues(IContent content)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            var type =
                _typeService.DocumentTypes.FirstOrDefault(t => t.Name.Alias().Equals(content.ContentType.Alias));

            if (type == null)
                return;

            SetDefaultValues(content, new DocumentType(type));
        }

        public void SetDefaultValues(IEnumerable<IMedia> contents)
        {
            if (contents == null)
                throw new ArgumentNullException("contents");

            foreach (var content in contents)
                SetDefaultValues(content);
        }

        public void SetDefaultValues(IMedia content)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            var type =
                _typeService.MediaTypes.FirstOrDefault(t => t.Name.Alias().Equals(content.ContentType.Alias));

            if (type == null)
                return;

            SetDefaultValues(content, new MediaType(type));
        }

        private static void SetDefaultValues<T>(IContentBase content, ContentType<T> contentType)
            where T : ContentTypeAttribute
        {
            if (content == null)
                throw new ArgumentNullException("content");

            if (contentType == null)
                throw new ArgumentNullException("contentType");

            foreach (var property in contentType.Properties)
                SetDefaultValue(content, property);
        }

        private static void SetDefaultValue(IContentBase content, ContentTypeProperty contentTypeProperty)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            if (contentTypeProperty == null)
                throw new ArgumentNullException("contentTypeProperty");

            if (!contentTypeProperty.HasDefaultValue)
                return;

            var value = content.GetValue(contentTypeProperty.Alias);

            if (value != null)
                return;

            if (!CanSetDefaultValue(contentTypeProperty.Type, contentTypeProperty.DefaultValue))
                return;

            content.SetValue(contentTypeProperty.Alias, contentTypeProperty.DefaultValue);
        }

        private static bool CanSetDefaultValue(Type propertyType, object defaultValue)
        {
            if (propertyType == null)
                throw new ArgumentNullException("propertyType");

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
                throw new ArgumentNullException("from");

            if (to == null)
                throw new ArgumentNullException("to");

            if (to.IsAssignableFrom(from))
                return true;

            var methods = from.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(
                    m => m.ReturnType == to &&
                         (m.Name == "op_Implicit" ||
                          m.Name == "op_Explicit")
                );

            return methods.Any();
        }
    }
}