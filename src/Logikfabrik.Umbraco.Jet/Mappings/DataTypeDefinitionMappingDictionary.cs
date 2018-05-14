// <copyright file="DataTypeDefinitionMappingDictionary.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see cref="DataTypeDefinitionMappingDictionary" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class DataTypeDefinitionMappingDictionary : IDictionary<Type, IDataTypeDefinitionMapping>
    {
        private readonly Dictionary<Type, IDataTypeDefinitionMapping> _innerDictionary = new Dictionary<Type, IDataTypeDefinitionMapping>();

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public int Count => _innerDictionary.Count;

        /// <inheritdoc />
        public ICollection<Type> Keys => _innerDictionary.Keys;

        /// <inheritdoc />
        public ICollection<IDataTypeDefinitionMapping> Values => _innerDictionary.Values;

        /// <inheritdoc />
        public IDataTypeDefinitionMapping this[Type key]
        {
            get => _innerDictionary[key];
            set => _innerDictionary[key] = value;
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Type, IDataTypeDefinitionMapping>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<Type, IDataTypeDefinitionMapping> item)
        {
            _innerDictionary.Add(item.Key, item.Value);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _innerDictionary.Clear();
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<Type, IDataTypeDefinitionMapping> item)
        {
            return _innerDictionary.ContainsKey(item.Key);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<Type, IDataTypeDefinitionMapping>[] array, int arrayIndex)
        {
            var a = _innerDictionary.ToArray();

            a.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<Type, IDataTypeDefinitionMapping> item)
        {
            return _innerDictionary.Remove(item.Key);
        }

        /// <inheritdoc />
        public bool ContainsKey(Type key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        /// <inheritdoc />
        public void Add(Type key, IDataTypeDefinitionMapping value)
        {
            _innerDictionary.Add(key, value);
        }

        /// <inheritdoc />
        public bool Remove(Type key)
        {
            return _innerDictionary.Remove(key);
        }

        /// <inheritdoc />
        public bool TryGetValue(Type key, out IDataTypeDefinitionMapping value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }
    }
}