// <copyright file="TypeService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using EnsureThat;
    using Extensions;
    using Logging;

    /// <summary>
    /// The <see cref="TypeService" /> class. Scans assemblies for types annotated using the <see cref="DocumentTypeAttribute" />, <see cref="MediaTypeAttribute" />, <see cref="MemberTypeAttribute" />, and <see cref="DataTypeAttribute" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class TypeService : ITypeService
    {
        private readonly ILogService _logService;
        private readonly Lazy<IEnumerable<Type>> _documentTypes;
        private readonly Lazy<IEnumerable<Type>> _dataTypes;
        private readonly Lazy<IEnumerable<Type>> _mediaTypes;
        private readonly Lazy<IEnumerable<Type>> _memberTypes;
        private readonly Lazy<Type[]> _types;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeService" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="assemblyLoader">The assembly loader.</param>
        public TypeService(ILogService logService, IAssemblyLoader assemblyLoader)
        {
            Ensure.That(logService).IsNotNull();
            Ensure.That(assemblyLoader).IsNotNull();

            _logService = logService;
            _types = new Lazy<Type[]>(() => GetTypes(assemblyLoader.GetAssemblies()));
            _documentTypes = new Lazy<IEnumerable<Type>>(() => GetTypesByAttribute(TypeExtensions.IsModelType<DocumentTypeAttribute>));
            _dataTypes = new Lazy<IEnumerable<Type>>(() => GetTypesByAttribute(TypeExtensions.IsModelType<DataTypeAttribute>));
            _mediaTypes = new Lazy<IEnumerable<Type>>(() => GetTypesByAttribute(TypeExtensions.IsModelType<MediaTypeAttribute>));
            _memberTypes = new Lazy<IEnumerable<Type>>(() => GetTypesByAttribute(TypeExtensions.IsModelType<MemberTypeAttribute>));
        }

        /// <inheritdoc />
        public IEnumerable<Type> DocumentTypes => _documentTypes.Value;

        /// <inheritdoc />
        public IEnumerable<Type> DataTypes => _dataTypes.Value;

        /// <inheritdoc />
        public IEnumerable<Type> MediaTypes => _mediaTypes.Value;

        /// <inheritdoc />
        public IEnumerable<Type> MemberTypes => _memberTypes.Value;

        private IEnumerable<Type> GetTypesByAttribute(Func<Type, bool> predicate)
        {
            _logService.Log<TypeService>(new LogEntry(LogEntryType.Debug, "Getting types by attribute."));

            var watch = Stopwatch.StartNew();

            var types = _types.Value.Where(predicate).ToArray();

            watch.Stop();

            _logService.Log<TypeService>(new LogEntry(LogEntryType.Debug, $"Got {types.Length} types by attribute in {watch.ElapsedMilliseconds} ms."));

            return Array.AsReadOnly(types);
        }

        private Type[] GetTypes(IEnumerable<Assembly> assemblies)
        {
            _logService.Log<TypeService>(new LogEntry(LogEntryType.Debug, "Getting types from assemblies."));

            var watch = Stopwatch.StartNew();

            var types = new List<Type>();

            foreach (var assembly in assemblies)
            {
                types.AddRange(GetTypes(assembly));
            }

            watch.Stop();

            _logService.Log<TypeService>(new LogEntry(LogEntryType.Debug, $"Got {types.Count} types in {watch.ElapsedMilliseconds} ms from assemblies."));

            return types.ToArray();
        }

        private IEnumerable<Type> GetTypes(Assembly assembly)
        {
            try
            {
                _logService.Log<TypeService>(new LogEntry(LogEntryType.Debug, $"Getting types from assembly {assembly.FullName}."));

                var watch = Stopwatch.StartNew();

                var types = assembly.GetTypes();

                watch.Stop();

                _logService.Log<TypeService>(new LogEntry(LogEntryType.Debug, $"Got {types.Length} types from assembly {assembly.FullName} in {watch.ElapsedMilliseconds} ms."));

                return types;
            }
            catch (ReflectionTypeLoadException ex)
            {
                _logService.Log<TypeService>(new LogEntry(LogEntryType.Error, $"An exception was thrown when getting types from assembly {assembly.FullName}.", ex));

                return new Type[] { };
            }
            catch (Exception ex)
            {
                _logService.Log<TypeService>(new LogEntry(LogEntryType.Error, $"An exception was thrown when getting types from assembly {assembly.FullName}.", ex));

                throw;
            }
        }
    }
}