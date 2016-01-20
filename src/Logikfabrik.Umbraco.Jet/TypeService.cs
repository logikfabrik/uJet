﻿// <copyright file="TypeService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Configuration;
    using Extensions;

    /// <summary>
    /// The <see cref="TypeService" /> class. Scans assemblies for types annotated using the <see cref="DocumentTypeAttribute" />, <see cref="MediaTypeAttribute" />, <see cref="MemberTypeAttribute" />, and <see cref="DataTypeAttribute" />.
    /// </summary>
    public class TypeService : ITypeService
    {
        private static ITypeService instance;

        private readonly Lazy<ReadOnlyCollection<Type>> _documentTypes;
        private readonly Lazy<ReadOnlyCollection<Type>> _dataTypes;
        private readonly Lazy<ReadOnlyCollection<Type>> _mediaTypes;
        private readonly Lazy<ReadOnlyCollection<Type>> _memberTypes;
        private readonly Lazy<Assembly[]> _assemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeService" /> class.
        /// </summary>
        /// <param name="getAssemblies">Function to get assemblies to scan.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="getAssemblies" /> is <c>null</c>.</exception>
        internal TypeService(Func<Assembly[]> getAssemblies)
        {
            if (getAssemblies == null)
            {
                throw new ArgumentNullException(nameof(getAssemblies));
            }

            _assemblies = new Lazy<Assembly[]>(getAssemblies);
            _documentTypes = new Lazy<ReadOnlyCollection<Type>>(() => GetTypesByAttribute(TypeExtensions.IsModelType<DocumentTypeAttribute>));
            _dataTypes = new Lazy<ReadOnlyCollection<Type>>(() => GetTypesByAttribute(TypeExtensions.IsModelType<DataTypeAttribute>));
            _mediaTypes = new Lazy<ReadOnlyCollection<Type>>(() => GetTypesByAttribute(TypeExtensions.IsModelType<MediaTypeAttribute>));
            _memberTypes = new Lazy<ReadOnlyCollection<Type>>(() => GetTypesByAttribute(TypeExtensions.IsModelType<MemberTypeAttribute>));
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
        /// Gets the document type model types within the current application domain.
        /// </summary>
        /// <value>
        /// The document type model types.
        /// </value>
        public ReadOnlyCollection<Type> DocumentTypes => _documentTypes.Value;

        /// <summary>
        /// Gets the data type model types within the current application domain.
        /// </summary>
        /// <value>
        /// The data type model types.
        /// </value>
        public ReadOnlyCollection<Type> DataTypes => _dataTypes.Value;

        /// <summary>
        /// Gets the media type model types within the current application domain.
        /// </summary>
        /// <value>
        /// The media type model types.
        /// </value>
        public ReadOnlyCollection<Type> MediaTypes => _mediaTypes.Value;

        /// <summary>
        /// Gets the member type model types within the current application domain.
        /// </summary>
        /// <value>
        /// The member type model types.
        /// </value>
        public ReadOnlyCollection<Type> MemberTypes => _memberTypes.Value;

        /// <summary>
        /// Gets the assemblies to be scanned for model types, within the current application domain.
        /// </summary>
        /// <returns>The assemblies to be scanned.</returns>
        private static Assembly[] GetAssemblies()
        {
            var assemblyNames = JetConfigurationManager.Assemblies;

            return !assemblyNames.Any()
                ? AppDomain.CurrentDomain.GetAssemblies()
                : AppDomain.CurrentDomain.GetAssemblies().Where(a => assemblyNames.Contains(a.GetName().Name, StringComparer.InvariantCultureIgnoreCase)).ToArray();
        }

        /// <summary>
        /// Gets types by attribute predicate.
        /// </summary>
        /// <param name="predicate">A predicate for type scanning.</param>
        /// <returns>Types matching the given predicate.</returns>
        private ReadOnlyCollection<Type> GetTypesByAttribute(Func<Type, bool> predicate)
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

            return Array.AsReadOnly(types.ToArray());
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