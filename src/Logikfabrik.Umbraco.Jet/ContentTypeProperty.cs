﻿// The MIT License (MIT)

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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Logikfabrik.Umbraco.Jet
{
    public class ContentTypeProperty
    {
        private readonly Type _type;
        private readonly string _name;
        private readonly string _alias;
        private readonly bool _mandatory;
        private readonly int? _sortOrder;
        private readonly string _description;
        private readonly string _propertyGroup;
        private readonly string _regularExpression;
        private readonly string _uiHint;
        private readonly object _defaultValue;
        private readonly bool _hasDefaultValue;

        /// <summary>
        /// Gets the type of this content type property.
        /// </summary>
        public Type Type { get { return _type; } }

        /// <summary>
        /// Gets the name of this content type property.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets the alias of this content type property.
        /// </summary>
        public string Alias { get { return _alias; } }

        /// <summary>
        /// Gets whether or not this content type property is mandatory.
        /// </summary>
        public bool Mandatory { get { return _mandatory; } }

        /// <summary>
        /// Gets the sort order of this content type property.
        /// </summary>
        public int? SortOrder { get { return _sortOrder; } }

        /// <summary>
        /// Gets the property group of this content type property.
        /// </summary>
        public string PropertyGroup { get { return _propertyGroup; } }

        /// <summary>
        /// Gets the description of this content type property.
        /// </summary>
        public string Description { get { return _description; } }

        /// <summary>
        /// Gets the regular expression of this content type property.
        /// </summary>
        public string RegularExpression { get { return _regularExpression; } }

        /// <summary>
        /// Gets the UI hint of this content type property.
        /// </summary>
        public string UIHint { get { return _uiHint; } }

        /// <summary>
        /// Gets the default value of this content type property.
        /// </summary>
        public object DefaultValue { get { return _defaultValue; } }

        /// <summary>
        /// Gets whether or not this content type property has a default value.
        /// </summary>
        public bool HasDefaultValue { get { return _hasDefaultValue; } }

        public ContentTypeProperty(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            _type = property.PropertyType;
            _mandatory = GetIsMandatory(property);
            _alias = GetAlias(property);
            _regularExpression = GetRegularExpression(property);
            _uiHint = GetUIHint(property);
            _defaultValue = GetDefaultValue(property, out _hasDefaultValue);

            var attribute = property.GetCustomAttribute<DisplayAttribute>();

            _name = GetName(property, attribute);
            _sortOrder = GetSortOrder(property, attribute);
            _description = GetDescription(property, attribute);
            _propertyGroup = GetPropertyGroup(property, attribute);
        }

        /// <summary>
        /// Gets the content type property name from the given property.
        /// </summary>
        /// <param name="property">The underlying property.</param>
        /// <param name="attribute">The display attribute of the underlying property.</param>
        /// <returns>A content type property name.</returns>
        private static string GetName(PropertyInfo property, DisplayAttribute attribute)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            return attribute == null ? property.Name : attribute.GetName();
        }

        /// <summary>
        /// Gets the content type property alias from the given property.
        /// </summary>
        /// <param name="property">The underlying property.</param>
        /// <returns>A content type property alias.</returns>
        private static string GetAlias(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            return property.Name.Alias();
        }

        /// <summary>
        /// Gets the content type property description from the given property.
        /// </summary>
        /// <param name="property">The underlying property.</param>
        /// <param name="attribute">The display attribute of the underlying property.</param>
        /// <returns>A content type property description.</returns>
        private static string GetDescription(PropertyInfo property, DisplayAttribute attribute)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            return attribute == null ? null : attribute.GetDescription();
        }

        /// <summary>
        /// Gets whether or not the content type property is mandatory from the given property.
        /// </summary>
        /// <param name="property">The underlying property.</param>
        /// <returns>True if the property is mandatory; otherwise false.</returns>
        private static bool GetIsMandatory(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            return property.GetCustomAttribute<RequiredAttribute>() != null;
        }

        /// <summary>
        /// Gets the content type property default value from the given property.
        /// </summary>
        /// <param name="property">The underlying property.</param>
        /// <param name="hasDefaultValue">Whether or not the content type property has a default value.</param>
        /// <returns>A default value.</returns>
        private static object GetDefaultValue(PropertyInfo property, out bool hasDefaultValue)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var attribute = property.GetCustomAttribute<DefaultValueAttribute>();

            hasDefaultValue = attribute != null;

            return hasDefaultValue ? attribute.Value : null;
        }

        /// <summary>
        /// Gets the content type property sort order from the given property.
        /// </summary>
        /// <param name="property">The underlying property.</param>
        /// <param name="attribute">The display attribute of the underlying property.</param>
        /// <returns>A content type property sort order.</returns>
        private static int? GetSortOrder(PropertyInfo property, DisplayAttribute attribute)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            return attribute == null ? null : attribute.GetOrder();
        }

        /// <summary>
        /// Gets the content type property group from the given property.
        /// </summary>
        /// <param name="property">The underlying property.</param>
        /// <param name="attribute">The display attribute of the underlying property.</param>
        /// <returns>A content type property group.</returns>
        private static string GetPropertyGroup(PropertyInfo property, DisplayAttribute attribute)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            return attribute == null ? null : attribute.GetGroupName();
        }

        /// <summary>
        /// Get the content type property regular expression from the given property.
        /// </summary>
        /// <param name="property">The underlying property.</param>
        /// <returns>A content type property regular expression.</returns>
        private static string GetRegularExpression(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var attribute = property.GetCustomAttribute<RegularExpressionAttribute>();

            return attribute == null ? null : attribute.Pattern;
        }

        /// <summary>
        /// Gets the content type property UI hint from the given property.
        /// </summary>
        /// <param name="property">The underlying property.</param>
        /// <returns>A content type property UI hint.</returns>
        private static string GetUIHint(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var attribute = property.GetCustomAttribute<UIHintAttribute>();

            return attribute == null ? null : attribute.UIHint;
        }
    }
}