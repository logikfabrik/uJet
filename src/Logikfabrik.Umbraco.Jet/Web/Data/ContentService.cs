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

using Logikfabrik.Umbraco.Jet.Web.Data.Converters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using Umbraco.Core.Models;

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    public abstract class ContentService
    {
        private readonly ITypeService _typeService;
        private readonly IUmbracoHelperWrapper _umbracoHelperWrapper;

        protected ITypeService TypeService { get { return _typeService; } }

        protected IUmbracoHelperWrapper UmbracoHelper { get { return _umbracoHelperWrapper; } }

        protected ContentService(IUmbracoHelperWrapper umbracoHelperWrapper, ITypeService typeService)
        {
            if (umbracoHelperWrapper == null)
                throw new ArgumentNullException("umbracoHelperWrapper");

            if (typeService == null)
                throw new ArgumentNullException("typeService");
            
            _umbracoHelperWrapper = umbracoHelperWrapper;
            _typeService = typeService;
        }

        protected object GetContent(IPublishedContent content, Type contentType)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            if (contentType == null)
                throw new ArgumentNullException("contentType");

            var model = Activator.CreateInstance(contentType);

            MapByConvention(content, model);

            foreach (var property in content.Properties)
                MapProperty(model, property.PropertyTypeAlias, property.Value);

            return model;
        }

        private void MapByConvention(IPublishedContent content, object model)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            if (model == null)
                throw new ArgumentNullException("model");

            MapProperty(model, GetPropertyName(() => content.Id), content.Id);
            MapProperty(model, GetPropertyName(() => content.Url), content.Url);
            MapProperty(model, GetPropertyName(() => content.Name), content.Name);
            MapProperty(model, GetPropertyName(() => content.CreateDate), content.CreateDate);
            MapProperty(model, GetPropertyName(() => content.UpdateDate), content.UpdateDate);
        }

        private static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }

        private static string GetUIHint(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var attribute = property.GetCustomAttribute<UIHintAttribute>();

            var uiHint = attribute == null ? null : attribute.UIHint;

            return uiHint;
        }

        private static void MapProperty(object model, string name, object value)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            var p = model.GetType().GetProperty(name, BindingFlags.IgnoreCase |
                                                      BindingFlags.Public |
                                                      BindingFlags.Instance);

            if (p == null || !p.CanWrite)
                return;

            if (value == null && Nullable.GetUnderlyingType(p.PropertyType) != null)
                p.SetValue(model, null);

            else if (value != null)
            {
                if (p.PropertyType.IsInstanceOfType(value))
                    p.SetValue(model, value);
                else
                {
                    var uiHint = GetUIHint(p);
                    var converter = PropertyValueConverters.GetConverter(uiHint, value.GetType(), p.PropertyType);

                    if (converter != null)
                        p.SetValue(model, converter.Convert(value, p.PropertyType));
                }
            }
        }
    }
}
