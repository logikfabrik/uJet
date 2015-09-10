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
        /// <summary>
        /// The current instance.
        /// </summary>
        private static ITypeService instance;

        /// <summary>
        /// The document types.
        /// </summary>
        private readonly Lazy<IEnumerable<Type>> documentTypes;

        /// <summary>
        /// The data types.
        /// </summary>
        private readonly Lazy<IEnumerable<Type>> dataTypes;

        /// <summary>
        /// The media types.
        /// </summary>
        private readonly Lazy<IEnumerable<Type>> mediaTypes;

        /// <summary>
        /// The member types.
        /// </summary>
        private readonly Lazy<IEnumerable<Type>> memberTypes;

        /// <summary>
        /// The assemblies.
        /// </summary>
        private readonly Lazy<IEnumerable<Assembly>> assemblies;

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

            assemblies = new Lazy<IEnumerable<Assembly>>(getAssemblies);
            documentTypes = new Lazy<IEnumerable<Type>>(GetDocumentTypes);
            dataTypes = new Lazy<IEnumerable<Type>>(GetDataTypes);
            mediaTypes = new Lazy<IEnumerable<Type>>(GetMediaTypes);
            memberTypes = new Lazy<IEnumerable<Type>>(GetMemberTypes);
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="TypeService" /> class from being created.
        /// </summary>
        private TypeService() : this(GetAssemblies)
        {
        }

        /// <summary>
        /// Gets an singleton instance of the type service.
        /// </summary>
        public static ITypeService Instance => instance ?? (instance = new TypeService());

        /// <summary>
        /// Gets or sets the document types within the current application domain.
        /// </summary>
        /// <value>
        /// The document types.
        /// </value>
        public IEnumerable<Type> DocumentTypes => documentTypes.Value;

        /// <summary>
        /// Gets or sets the data types within the current application domain.
        /// </summary>
        /// <value>
        /// The data types.
        /// </value>
        public IEnumerable<Type> DataTypes => dataTypes.Value;

        /// <summary>
        /// Gets or sets the media types within the current application domain.
        /// </summary>
        /// <value>
        /// The media types.
        /// </value>
        public IEnumerable<Type> MediaTypes => mediaTypes.Value;

        /// <summary>
        /// Gets or sets the member types within the current application domain.
        /// </summary>
        /// <value>
        /// The member types.
        /// </value>
        public IEnumerable<Type> MemberTypes => memberTypes.Value;

        /// <summary>
        /// Gets the assemblies to be scanned for content types, within the current application domain.
        /// </summary>
        /// <returns>The assemblies to be scanned.</returns>
        private static IEnumerable<Assembly> GetAssemblies()
        {
            var assemblyNames = JetConfigurationManager.Assemblies;

            return !assemblyNames.Any()
                ? AppDomain.CurrentDomain.GetAssemblies()
                : AppDomain.CurrentDomain.GetAssemblies().Where(a => assemblyNames.Contains(a.GetName().Name));
        }

        /// <summary>
        /// Gets the document types within the current application domain.
        /// </summary>
        /// <returns>The document types.</returns>
        private IEnumerable<Type> GetDocumentTypes()
        {
            return GetTypesByAttribute(TypeExtensions.IsDocumentType);
        }

        /// <summary>
        /// Gets the data types within the current application domain.
        /// </summary>
        /// <returns>The data types.</returns>
        private IEnumerable<Type> GetDataTypes()
        {
            return GetTypesByAttribute(TypeExtensions.IsDataType);
        }

        /// <summary>
        /// Gets the media types within the current application domain.
        /// </summary>
        /// <returns>The media types.</returns>
        private IEnumerable<Type> GetMediaTypes()
        {
            return GetTypesByAttribute(TypeExtensions.IsMediaType);
        }

        /// <summary>
        /// Gets the member types within the current application domain.
        /// </summary>
        /// <returns>The member types.</returns>
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

            foreach (var assembly in assemblies.Value)
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

            return assembly.GetTypes();
        }
    }
}
