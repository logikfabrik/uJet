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

using Logikfabrik.Umbraco.Jet.Configuration;
using Logikfabrik.Umbraco.Jet.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Logikfabrik.Umbraco.Jet
{
    /// <summary>
    /// Service for types.
    /// </summary>
    public class TypeService : ITypeService
    {
        private static ITypeService _instance;
        private readonly Lazy<IEnumerable<Type>> _documentTypes;
        private readonly Lazy<IEnumerable<Type>> _dataTypes;
        private readonly Lazy<IEnumerable<Type>> _mediaTypes;
        private readonly Lazy<IEnumerable<Assembly>> _assemblies;

        private TypeService()
        {
            _assemblies = new Lazy<IEnumerable<Assembly>>(GetAssemblies);
            _documentTypes = new Lazy<IEnumerable<Type>>(GetDocumentTypes);
            _dataTypes = new Lazy<IEnumerable<Type>>(GetDataTypes);
            _mediaTypes = new Lazy<IEnumerable<Type>>(GetMediaTypes);
        }

        /// <summary>
        /// Gets the document types within the current application domain.
        /// </summary>
        public IEnumerable<Type> DocumentTypes { get { return _documentTypes.Value; } }

        /// <summary>
        /// Gets the data types within the current application domain.
        /// </summary>
        public IEnumerable<Type> DataTypes { get { return _dataTypes.Value; } }

        /// <summary>
        /// Gets the media types within the current application domain.
        /// </summary>
        public IEnumerable<Type> MediaTypes { get { return _mediaTypes.Value; } }

        public static ITypeService Instance
        {
            get { return _instance ?? (_instance = new TypeService()); }
        }

        /// <summary>
        /// Gets the document types within the current application domain.
        /// </summary>
        /// <returns>Document types.</returns>
        private IEnumerable<Type> GetDocumentTypes()
        {
            return GetTypesByAttribute(TypeExtensions.IsDocumentType);
        }

        /// <summary>
        /// Gets the data types within the current application domain.
        /// </summary>
        /// <returns>Data types.</returns>
        private IEnumerable<Type> GetDataTypes()
        {
            return GetTypesByAttribute(TypeExtensions.IsDataType);
        }

        /// <summary>
        /// Gets the media types within the current application domain.
        /// </summary>
        /// <returns>Media types.</returns>
        private IEnumerable<Type> GetMediaTypes()
        {
            return GetTypesByAttribute(TypeExtensions.IsMediaType);
        }

        private IEnumerable<Type> GetTypesByAttribute(Func<Type, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            Func<Assembly, IEnumerable<Type>> getTypes = a => a.GetTypes().Where(predicate);

            var types = new List<Type>();

            foreach (var assembly in _assemblies.Value)
            {
                types.AddRange(getTypes(assembly));
            }

            return types;
        }

        /// <summary>
        /// Gets the assemblies to be probed for content types, within the current application domain.
        /// </summary>
        /// <returns>Assemblies.</returns>
        private static IEnumerable<Assembly> GetAssemblies()
        {
            var assemblies = JetConfigurationManager.Assemblies;

            return !assemblies.Any()
                ? AppDomain.CurrentDomain.GetAssemblies()
                : AppDomain.CurrentDomain.GetAssemblies().Where(a => assemblies.Contains(a.GetName().Name));
        }
    }
}
