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
    /// The <see cref="ContentType{T}" /> class.
    /// </summary>
    /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
    public abstract class ContentType<T> where T : ContentTypeAttribute
    {
        /// <summary>
        /// The type.
        /// </summary>
        private readonly Type type;

        /// <summary>
        /// The identifier.
        /// </summary>
        private readonly Guid? id;

        /// <summary>
        /// The name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The alias.
        /// </summary>
        private readonly string alias;

        /// <summary>
        /// The description.
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Whether content of this type can be created at the root of the content tree.
        /// </summary>
        private readonly bool allowedAsRoot;

        /// <summary>
        /// The icon.
        /// </summary>
        private readonly string icon;

        /// <summary>
        /// The thumbnail.
        /// </summary>
        private readonly string thumbnail;

        /// <summary>
        /// The allowed child node types.
        /// </summary>
        private readonly IEnumerable<Type> allowedChildNodeTypes;

        /// <summary>
        /// The properties.
        /// </summary>
        private readonly IEnumerable<ContentTypeProperty> properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentType{T}" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="ArgumentNullException">Thrown if type is null.</exception>
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
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid? Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias
        {
            get { return this.alias; }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public IEnumerable<ContentTypeProperty> Properties
        {
            get { return this.properties; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return this.description; }
        }

        /// <summary>
        /// Gets a value indicating whether content of this type can be created at the root of the content tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if allowed as root; otherwise, <c>false</c>.
        /// </value>
        public bool AllowedAsRoot
        {
            get { return this.allowedAsRoot; }
        }

        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public string Icon
        {
            get { return this.icon; }
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <value>
        /// The thumbnail.
        /// </value>
        public string Thumbnail
        {
            get { return this.thumbnail; }
        }

        /// <summary>
        /// Gets the allowed child node types.
        /// </summary>
        /// <value>
        /// The allowed child node types.
        /// </value>
        public IEnumerable<Type> AllowedChildNodeTypes
        {
            get { return this.allowedChildNodeTypes; }
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <returns>The attribute.</returns>
        protected T GetAttribute()
        {
            return this.type.GetCustomAttribute<T>();
        }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The alias.</returns>
        /// <exception cref="ArgumentNullException">Thrown if type is null.</exception>
        private static string GetAlias(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.Name.Alias();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The name.</returns>
        /// <exception cref="ArgumentNullException">Thrown if type is null.</exception>
        private static string GetName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.GetCustomAttribute<T>().Name;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown if attribute is null.</exception>
        private static Guid? GetId(IdAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            return attribute.Id;
        }

        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The icon.</returns>
        /// <exception cref="ArgumentNullException">Thrown if attribute is null.</exception>
        private static string GetIcon(ContentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            return attribute.Icon;
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The thumbnail.</returns>
        /// <exception cref="ArgumentNullException">Thrown if attribute is null.</exception>
        private static string GetThumbnail(ContentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            return attribute.Thumbnail;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The description.</returns>
        /// <exception cref="ArgumentNullException">Throw if attribute is null.</exception>
        private static string GetDescription(ContentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            return attribute.Description;
        }

        /// <summary>
        /// Gets a value indicating whether content of this type can be created at the root of the content tree.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>
        ///   <c>true</c> if allowed as root; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if attribute is null.</exception>
        private static bool GetAllowedAsRoot(ContentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            return attribute.AllowedAsRoot;
        }

        /// <summary>
        /// Gets the allowed child node types.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The allowed child node types.</returns>
        /// <exception cref="ArgumentNullException">Thrown if attribute is null.</exception>
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
        /// Gets the properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The properties.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the type is null.</exception>
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