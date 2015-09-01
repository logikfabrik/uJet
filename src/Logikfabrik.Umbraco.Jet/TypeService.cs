//----------------------------------------------------------------------------------
// <copyright file="TypeService.cs" company="Logikfabrik">
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
    using System.Linq;
    using System.Reflection;
    using Configuration;
    using Extensions;

    /// <summary>
    /// The concrete type service, responsible for probing assemblies for document types, media types, and data types.
    /// </summary>
    public class TypeService : ITypeService
    {
        /// <summary>
        /// The current instance.
        /// </summary>
        private static ITypeService instance;

        /// <summary>
        /// Enumerable of all document types.
        /// </summary>
        private readonly Lazy<IEnumerable<Type>> documentTypes;

        /// <summary>
        /// Enumerable of all data types.
        /// </summary>
        private readonly Lazy<IEnumerable<Type>> dataTypes;

        /// <summary>
        /// Enumerable of all media types.
        /// </summary>
        private readonly Lazy<IEnumerable<Type>> mediaTypes;

        /// <summary>
        /// Enumerable of all assemblies.
        /// </summary>
        private readonly Lazy<IEnumerable<Assembly>> assemblies;

        /// <summary>
        /// Prevents a default instance of the <see cref="TypeService" /> class from being created.
        /// </summary>
        private TypeService()
        {
            this.assemblies = new Lazy<IEnumerable<Assembly>>(GetAssemblies);
            this.documentTypes = new Lazy<IEnumerable<Type>>(this.GetDocumentTypes);
            this.dataTypes = new Lazy<IEnumerable<Type>>(this.GetDataTypes);
            this.mediaTypes = new Lazy<IEnumerable<Type>>(this.GetMediaTypes);
        }

        /// <summary>
        /// Gets an singleton instance of the type service.
        /// </summary>
        public static ITypeService Instance
        {
            get { return instance ?? (instance = new TypeService()); }
        }

        /// <summary>
        /// Gets the document types within the current application domain.
        /// </summary>
        public IEnumerable<Type> DocumentTypes
        {
            get { return this.documentTypes.Value; }
        }

        /// <summary>
        /// Gets the data types within the current application domain.
        /// </summary>
        public IEnumerable<Type> DataTypes
        {
            get { return this.dataTypes.Value; }
        }

        /// <summary>
        /// Gets the media types within the current application domain.
        /// </summary>
        public IEnumerable<Type> MediaTypes
        {
            get { return this.mediaTypes.Value; }
        }

        /// <summary>
        /// Gets the assemblies to be probed for content types, within the current application domain.
        /// </summary>
        /// <returns>Assemblies to be probed.</returns>
        private static IEnumerable<Assembly> GetAssemblies()
        {
            var assemblies = JetConfigurationManager.Assemblies;

            return !assemblies.Any()
                ? AppDomain.CurrentDomain.GetAssemblies()
                : AppDomain.CurrentDomain.GetAssemblies().Where(a => assemblies.Contains(a.GetName().Name));
        }

        /// <summary>
        /// Gets the document types within the current application domain.
        /// </summary>
        /// <returns>Document types.</returns>
        private IEnumerable<Type> GetDocumentTypes()
        {
            return this.GetTypesByAttribute(TypeExtensions.IsDocumentType);
        }

        /// <summary>
        /// Gets the data types within the current application domain.
        /// </summary>
        /// <returns>Data types.</returns>
        private IEnumerable<Type> GetDataTypes()
        {
            return this.GetTypesByAttribute(TypeExtensions.IsDataType);
        }

        /// <summary>
        /// Gets the media types within the current application domain.
        /// </summary>
        /// <returns>Media types.</returns>
        private IEnumerable<Type> GetMediaTypes()
        {
            return this.GetTypesByAttribute(TypeExtensions.IsMediaType);
        }

        /// <summary>
        /// Gets types by attribute predicate.
        /// </summary>
        /// <param name="predicate">A predicate function for type probing.</param>
        /// <returns>Types matching the given predicate.</returns>
        private IEnumerable<Type> GetTypesByAttribute(Func<Type, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            Func<Assembly, IEnumerable<Type>> getTypes = a => a.GetTypes().Where(predicate);

            var types = new List<Type>();

            foreach (var assembly in this.assemblies.Value)
            {
                types.AddRange(getTypes(assembly));
            }

            return types;
        }
    }
}
