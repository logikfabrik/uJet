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
        /// Prevents a default instance of the <see cref="TypeService" /> class from being created.
        /// </summary>
        private TypeService()
        {
            assemblies = new Lazy<IEnumerable<Assembly>>(GetAssemblies);
            documentTypes = new Lazy<IEnumerable<Type>>(GetDocumentTypes);
            dataTypes = new Lazy<IEnumerable<Type>>(GetDataTypes);
            mediaTypes = new Lazy<IEnumerable<Type>>(GetMediaTypes);
            memberTypes = new Lazy<IEnumerable<Type>>(GetMemberTypes);
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
            var assemblies = JetConfigurationManager.Assemblies;

            return !assemblies.Any()
                ? AppDomain.CurrentDomain.GetAssemblies()
                : AppDomain.CurrentDomain.GetAssemblies().Where(a => assemblies.Contains(a.GetName().Name));
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

            Func<Assembly, IEnumerable<Type>> getTypes = a => a.GetTypes().Where(predicate);

            var types = new List<Type>();

            foreach (var assembly in assemblies.Value)
            {
                types.AddRange(getTypes(assembly));
            }

            return types;
        }
    }
}
