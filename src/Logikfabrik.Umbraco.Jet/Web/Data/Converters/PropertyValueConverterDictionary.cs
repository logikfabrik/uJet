// <copyright file="PropertyValueConverterDictionary.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see cref="PropertyValueConverterDictionary" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class PropertyValueConverterDictionary : IDictionary<Type, IEnumerable<IPropertyValueConverter>>
    {
        private readonly Dictionary<Type, IEnumerable<IPropertyValueConverter>> _innerDictionary = new Dictionary<Type, IEnumerable<IPropertyValueConverter>>();

        /// <inheritdoc />
        public ICollection<Type> Keys => _innerDictionary.Keys;

        /// <inheritdoc />
        public ICollection<IEnumerable<IPropertyValueConverter>> Values => _innerDictionary.Values;

        /// <inheritdoc />
        public int Count => _innerDictionary.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public IEnumerable<IPropertyValueConverter> this[Type key]
        {
            get => _innerDictionary[key];
            set => _innerDictionary[key] = value;
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Type, IEnumerable<IPropertyValueConverter>>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<Type, IEnumerable<IPropertyValueConverter>> item)
        {
            _innerDictionary.Add(item.Key, item.Value);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _innerDictionary.Clear();
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<Type, IEnumerable<IPropertyValueConverter>> item)
        {
            return _innerDictionary.ContainsKey(item.Key);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<Type, IEnumerable<IPropertyValueConverter>>[] array, int arrayIndex)
        {
            var a = _innerDictionary.ToArray();

            a.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<Type, IEnumerable<IPropertyValueConverter>> item)
        {
            return _innerDictionary.Remove(item.Key);
        }

        /// <inheritdoc />
        public bool ContainsKey(Type key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        /// <inheritdoc />
        public void Add(Type key, IEnumerable<IPropertyValueConverter> value)
        {
            _innerDictionary.Add(key, value);
        }

        /// <inheritdoc />
        public bool Remove(Type key)
        {
            return _innerDictionary.Remove(key);
        }

        /// <inheritdoc />
        public bool TryGetValue(Type key, out IEnumerable<IPropertyValueConverter> value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }
    }
}
