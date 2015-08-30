//----------------------------------------------------------------------------------
// <copyright file="ContentType.cs" company="Logikfabrik">
//     The MIT License (MIT)
//
//     Copyright (c) 2015 anton(at)logikfabrik.se
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
//----------------------------------------------------------------------------------

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using Extensions;

    /// <summary>
    /// Base type for content types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ContentType<T> where T : ContentTypeAttribute
    {
        /// <summary>
        /// The type of this content type.
        /// </summary>
        private readonly Type type;

        /// <summary>
        /// The ID for this content type.
        /// </summary>
        private readonly Guid? id;

        /// <summary>
        /// The name of this content type.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The alias of this content type.
        /// </summary>
        private readonly string alias;

        /// <summary>
        /// The description of this content type.
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Whether or not content of this content type can be created at the root of the content tree.
        /// </summary>
        private readonly bool allowedAsRoot;

        /// <summary>
        /// The icon for this content type.
        /// </summary>
        private readonly string icon;

        /// <summary>
        /// The thumbnail for this content type.
        /// </summary>
        private readonly string thumbnail;
        private readonly IEnumerable<Type> allowedChildNodeTypes;
        private readonly IEnumerable<ContentTypeProperty> properties;

        protected ContentType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            this.type = type;
            this.alias = GetAlias(type);
            this.name = GetName(type);
            this.properties = GetProperties(type);

            var attribute = this.GetAttribute();

            this.id = GetId(attribute);
            this.icon = GetIcon(attribute);
            this.thumbnail = GetThumbnail(attribute);
            this.description = GetDescription(attribute);
            this.allowedAsRoot = GetAllowedAsRoot(attribute);
            this.allowedChildNodeTypes = GetAllowedChildNodeTypes(attribute);
        }
        
        /// <summary>
        /// Gets the type of this content type.
        /// </summary>
        public Type Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets the ID for this content type.
        /// </summary>
        public Guid? Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets the name of this content type.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the alias of this content type.
        /// </summary>
        public string Alias
        {
            get { return this.alias; }
        }

        /// <summary>
        /// Gets the properties of this content type.
        /// </summary>
        public IEnumerable<ContentTypeProperty> Properties
        {
            get { return this.properties; }
        }

        /// <summary>
        /// Gets the description of this content type.
        /// </summary>
        public string Description
        {
            get { return this.description; }
        }

        /// <summary>
        /// Gets a value indicating whether or not content of this content type can be created at the root of the content tree.
        /// </summary>
        public bool AllowedAsRoot
        {
            get { return this.allowedAsRoot; }
        }

        /// <summary>
        /// Gets the icon for this content type.
        /// </summary>
        public string Icon
        {
            get { return this.icon; }
        }

        /// <summary>
        /// Gets the thumbnail for this content type.
        /// </summary>
        public string Thumbnail
        {
            get { return this.thumbnail; }
        }

        /// <summary>
        /// Gets the allowed child nodes types of this content type.
        /// </summary>
        public IEnumerable<Type> AllowedChildNodeTypes
        {
            get { return this.allowedChildNodeTypes; }
        }

        protected T GetAttribute()
        {
            return this.type.GetCustomAttribute<T>();
        }

        /// <summary>
        /// Gets the content type alias from the given type.
        /// </summary>
        /// <param name="type">The underlying type.</param>
        /// <returns>A content type alias.</returns>
        private static string GetAlias(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
                
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
            {
                throw new ArgumentNullException("type");
            }
            
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
            {
                throw new ArgumentNullException("attribute");
            }
                
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
            {
                throw new ArgumentNullException("attribute");
            }
                
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
            {
                throw new ArgumentNullException("attribute");
            }
                
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
            {
                throw new ArgumentNullException("attribute");
            }
                
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
            {
                throw new ArgumentNullException("attribute");
            }
                
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
            {
                throw new ArgumentNullException("attribute");
            }
            
            return attribute.AllowedChildNodeTypes == null 
                ? new Type[] { } 
                : attribute.AllowedChildNodeTypes.Where(t => t.GetCustomAttribute<T>() != null);
        }

        /// <summary>
        /// Gets the content type properties from the given type.
        /// </summary>
        /// <param name="type">The underlying type.</param>
        /// <returns>Content type properties.</returns>
        private static IEnumerable<ContentTypeProperty> GetProperties(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
                
            return from property in type.GetProperties()
                   let attribute = property.GetCustomAttribute<ScaffoldColumnAttribute>()
                   where (attribute == null || attribute.Scaffold) && property.CanRead && property.CanWrite
                   select new ContentTypeProperty(property);
        }
    }
}