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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Logikfabrik.Umbraco.Jet
{
    public abstract class ContentType<T> where T : ContentTypeAttribute
    {
        private readonly Type _type;
        private readonly Guid? _id;
        private readonly string _name;
        private readonly string _alias;
        private readonly string _description;
        private readonly bool _allowedAsRoot;
        private readonly string _icon;
        private readonly string _thumbnail;
        private readonly IEnumerable<Type> _allowedChildNodeTypes;
        private readonly IEnumerable<ContentTypeProperty> _properties;

        /// <summary>
        /// Gets the type of this content type.
        /// </summary>
        public Type Type { get { return _type; } }

        /// <summary>
        /// Gets the ID for this content type.
        /// </summary>
        public Guid? Id { get { return _id; } }

        /// <summary>
        /// Gets the name of this content type.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets the alias of this content type.
        /// </summary>
        public string Alias { get { return _alias; } }

        /// <summary>
        /// Gets the properties of this content type.
        /// </summary>
        public IEnumerable<ContentTypeProperty> Properties { get { return _properties; } }

        /// <summary>
        /// Gets the description of this content type.
        /// </summary>
        public string Description { get { return _description; } }

        /// <summary>
        /// Gets whether or not content of this content type can be created at the root of the content tree.
        /// </summary>
        public bool AllowedAsRoot { get { return _allowedAsRoot; } }

        /// <summary>
        /// Gets the icon for this content type.
        /// </summary>
        public string Icon { get { return _icon; } }

        /// <summary>
        /// Gets the thumbnail for this content type.
        /// </summary>
        public string Thumbnail { get { return _thumbnail; } }

        /// <summary>
        /// Gets the allowed child nodes types of this content type.
        /// </summary>
        public IEnumerable<Type> AllowedChildNodeTypes { get { return _allowedChildNodeTypes; } }

        protected ContentType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            _type = type;
            _alias = GetAlias(type);
            _name = GetName(type);
            _properties = GetProperties(type);

            var attribute = GetAttribute();

            _id = GetId(attribute);
            _icon = GetIcon(attribute);
            _thumbnail = GetThumbnail(attribute);
            _description = GetDescription(attribute);
            _allowedAsRoot = GetAllowedAsRoot(attribute);
            _allowedChildNodeTypes = GetAllowedChildNodeTypes(attribute);
        }

        protected T GetAttribute()
        {
            return _type.GetCustomAttribute<T>();
        }

        /// <summary>
        /// Gets the content type alias from the given type.
        /// </summary>
        /// <param name="type">The underlying type.</param>
        /// <returns>A content type alias.</returns>
        private static string GetAlias(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return type.Name.Alias();
        }

        /// <summary>
        /// Gets the content type name from the given type.
        /// </summary>
        /// <param name="type">The underlying type.</param>
        /// <returns>A content type name.</returns>
        private static string GetName(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return type.GetCustomAttribute<T>().Name;
        }

        /// <summary>
        /// Gets the content type ID from the given type.
        /// </summary>
        /// <param name="attribute">The content type attribute of the underlying type.</param>
        /// <returns>A content type ID.</returns>
        private static Guid? GetId(IdAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            return attribute.Id;
        }

        /// <summary>
        /// Gets the content type icon from the given type.
        /// </summary>
        /// <param name="attribute">The content type attribute of the underlying type.</param>
        /// <returns>A content type icon path.</returns>
        private static string GetIcon(ContentTypeAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            return attribute.Icon;
        }

        /// <summary>
        /// Gets the content type thumbnail from the given type.
        /// </summary>
        /// <param name="attribute">The content type attribute of the underlying type.</param>
        /// <returns>A content type thumbnail path.</returns>
        private static string GetThumbnail(ContentTypeAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            return attribute.Thumbnail;
        }

        /// <summary>
        /// Gets the content type description from the given type.
        /// </summary>
        /// <param name="attribute">The content type attribute of the underlying type.</param>
        /// <returns>A content type description.</returns>
        private static string GetDescription(ContentTypeAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            return attribute.Description;
        }

        /// <summary>
        /// Gets whether or not content of this content type can be created at the root of the content tree from the given type.
        /// </summary>
        /// <param name="attribute">The content type attribute of the underlying type.</param>
        /// <returns>True if this content type can be created at the root of the content tree; otherwise false.</returns>
        private static bool GetAllowedAsRoot(ContentTypeAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            return attribute.AllowedAsRoot;
        }

        /// <summary>
        /// Gets the allowed child node types from the given type.
        /// </summary>
        /// <param name="attribute">The content type attribute of the underlying type.</param>
        /// <returns>Allowed child node types.</returns>
        private static IEnumerable<Type> GetAllowedChildNodeTypes(ContentTypeAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            if (attribute.AllowedChildNodeTypes == null)
                return new Type[] { };

            return attribute.AllowedChildNodeTypes.Where(t => t.GetCustomAttribute<T>() != null);
        }

        /// <summary>
        /// Gets the content type properties from the given type.
        /// </summary>
        /// <param name="type">The underlying type.</param>
        /// <returns>Content type properties.</returns>
        private static IEnumerable<ContentTypeProperty> GetProperties(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return from property in type.GetProperties()
                   let attribute = property.GetCustomAttribute<ScaffoldColumnAttribute>()
                   where (attribute == null || attribute.Scaffold) && property.CanRead && property.CanWrite
                   select new ContentTypeProperty(property);
        }
    }
}