// <copyright file="TypeService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Configuration;
    using Extensions;

    /// <summary>
    /// The <see cref="TypeService" /> class. Scans assemblies for types annotated using the <see cref="DocumentTypeAttribute" />, <see cref="MediaTypeAttribute" />, <see cref="DataTypeAttribute" />, and <see cref="MemberTypeAttribute" />.
    /// </summary>
    public class TypeService : ITypeService
    {
        private static ITypeService instance;
        private readonly Lazy<IEnumerable<Type>> _documentTypes;
        private readonly Lazy<IEnumerable<Type>> _dataTypes;
        private readonly Lazy<IEnumerable<Type>> _mediaTypes;
        private readonly Lazy<IEnumerable<Type>> _memberTypes;
        private readonly Lazy<IEnumerable<Assembly>> _assemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeService" /> class.
        /// </summary>
        /// <param name="getAssemblies">Function to get assemblies to scan.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="getAssemblies" /> is <c>null</c>.</exception>
        internal TypeService(Func<IEnumerable<Assembly>> getAssemblies)
        {
            if (getAssemblies == null)
            {
                throw new ArgumentNullException(nameof(getAssemblies));
            }

            _assemblies = new Lazy<IEnumerable<Assembly>>(getAssemblies);
            _documentTypes = new Lazy<IEnumerable<Type>>(GetDocumentTypes);
            _dataTypes = new Lazy<IEnumerable<Type>>(GetDataTypes);
            _mediaTypes = new Lazy<IEnumerable<Type>>(GetMediaTypes);
            _memberTypes = new Lazy<IEnumerable<Type>>(GetMemberTypes);
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="TypeService" /> class from being created.
        /// </summary>
        private TypeService()
            : this(GetAssemblies)
        {
        }

        /// <summary>
        /// Gets a singleton instance of the type service.
        /// </summary>
        public static ITypeService Instance => instance ?? (instance = new TypeService());

        /// <summary>
        /// Gets the document model types within the current application domain.
        /// </summary>
        /// <value>
        /// The document model types.
        /// </value>
        public IEnumerable<Type> DocumentTypes => _documentTypes.Value;

        /// <summary>
        /// Gets the data model types within the current application domain.
        /// </summary>
        /// <value>
        /// The data model types.
        /// </value>
        public IEnumerable<Type> DataTypes => _dataTypes.Value;

        /// <summary>
        /// Gets the media model types within the current application domain.
        /// </summary>
        /// <value>
        /// The media model types.
        /// </value>
        public IEnumerable<Type> MediaTypes => _mediaTypes.Value;

        /// <summary>
        /// Gets the member model types within the current application domain.
        /// </summary>
        /// <value>
        /// The member model types.
        /// </value>
        public IEnumerable<Type> MemberTypes => _memberTypes.Value;

        /// <summary>
        /// Gets the assemblies to be scanned for model types, within the current application domain.
        /// </summary>
        /// <returns>The assemblies to be scanned.</returns>
        private static IEnumerable<Assembly> GetAssemblies()
        {
            var assemblyNames = JetConfigurationManager.Assemblies;

            return !assemblyNames.Any()
                ? AppDomain.CurrentDomain.GetAssemblies()
                : AppDomain.CurrentDomain.GetAssemblies().Where(a => assemblyNames.Contains(a.GetName().Name, StringComparer.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Gets the document model types within the current application domain.
        /// </summary>
        /// <returns>The document model types.</returns>
        private IEnumerable<Type> GetDocumentTypes()
        {
            return GetTypesByAttribute(TypeExtensions.IsDocumentType);
        }

        /// <summary>
        /// Gets the data model types within the current application domain.
        /// </summary>
        /// <returns>The data model types.</returns>
        private IEnumerable<Type> GetDataTypes()
        {
            return GetTypesByAttribute(TypeExtensions.IsDataType);
        }

        /// <summary>
        /// Gets the media model types within the current application domain.
        /// </summary>
        /// <returns>The media model types.</returns>
        private IEnumerable<Type> GetMediaTypes()
        {
            return GetTypesByAttribute(TypeExtensions.IsMediaType);
        }

        /// <summary>
        /// Gets the member model types within the current application domain.
        /// </summary>
        /// <returns>The member model types.</returns>
        private IEnumerable<Type> GetMemberTypes()
        {
            return GetTypesByAttribute(TypeExtensions.IsMemberType);
        }

        /// <summary>
        /// Gets types by attribute predicate.
        /// </summary>
        /// <param name="predicate">A predicate for type scanning.</param>
        /// <returns>Types matching the given predicate.</returns>
        private IEnumerable<Type> GetTypesByAttribute(Func<Type, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var types = new List<Type>();

            foreach (var assembly in _assemblies.Value)
            {
                types.AddRange(GetTypes(assembly).Where(predicate));
            }

            return types;
        }

        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <param name="assembly">The assembly to get types from.</param>
        /// <returns>The types.</returns>
        private IEnumerable<Type> GetTypes(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException)
            {
                return new Type[] { };
            }
        }
    }
}