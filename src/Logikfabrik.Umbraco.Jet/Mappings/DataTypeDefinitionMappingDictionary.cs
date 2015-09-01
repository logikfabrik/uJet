//----------------------------------------------------------------------------------
// <copyright file="DataTypeDefinitionMappingDictionary.cs" company="Logikfabrik">
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

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Dictionary for data type definition mappings.
    /// </summary>
    public class DataTypeDefinitionMappingDictionary : IDictionary<Type, IDataTypeDefinitionMapping>
    {
        /// <summary>
        /// Inner dictionary for data type definition mappings.
        /// </summary>
        private readonly Dictionary<Type, IDataTypeDefinitionMapping> innerDictionary = new Dictionary<Type, IDataTypeDefinitionMapping>();
        
        /// <summary>
        /// Default mapping.
        /// </summary>
        private IDefaultDataTypeDefinitionMapping defaultMapping;

        /// <summary>
        /// Gets or sets the default mapping.
        /// </summary>
        public IDefaultDataTypeDefinitionMapping DefaultMapping
        {
            get { return this.defaultMapping ?? (this.defaultMapping = new DefaultDataTypeDefinitionMapping()); }
            set { this.defaultMapping = value; }
        }
        
        /// <summary>
        /// Gets a value indicating whether or not the dictionary is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the number of key value pairs in the dictionary.
        /// </summary>
        public int Count
        {
            get { return this.innerDictionary.Count; }
        }

        /// <summary>
        /// Gets the mapping keys in the dictionary.
        /// </summary>
        public ICollection<Type> Keys
        {
            get { return this.innerDictionary.Keys; }
        }

        /// <summary>
        /// Gets the data type definition mappings in the dictionary.
        /// </summary>
        public ICollection<IDataTypeDefinitionMapping> Values
        {
            get { return this.innerDictionary.Values; }
        }

        public IDataTypeDefinitionMapping this[Type key]
        {
            get { return this.innerDictionary[key]; }
            set { this.innerDictionary[key] = value; }
        }

        /// <summary>
        /// Gets an enumerator that iterates through the dictionary.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public IEnumerator<KeyValuePair<Type, IDataTypeDefinitionMapping>> GetEnumerator()
        {
            return this.innerDictionary.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that iterates through the dictionary.
        /// </summary>
        /// <returns>An enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds a data type definition mapping to the dictionary.
        /// </summary>
        /// <param name="item">The data type definition mapping and type to add.</param>
        public void Add(KeyValuePair<Type, IDataTypeDefinitionMapping> item)
        {
            this.innerDictionary.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears the dictionary.
        /// </summary>
        public void Clear()
        {
            this.innerDictionary.Clear();
        }

        public bool Contains(KeyValuePair<Type, IDataTypeDefinitionMapping> item)
        {
            return this.innerDictionary.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<Type, IDataTypeDefinitionMapping>[] array, int index)
        {
            var a = this.innerDictionary.ToArray();

            a.CopyTo(array, index);
        }

        public bool Remove(KeyValuePair<Type, IDataTypeDefinitionMapping> item)
        {
            return this.innerDictionary.Remove(item.Key);
        }
        
        public bool ContainsKey(Type key)
        {
            return this.innerDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Adds a data type definition mapping to the dictionary.
        /// </summary>
        /// <param name="key">A type.</param>
        /// <param name="value">A data type definition mapping.</param>
        public void Add(Type key, IDataTypeDefinitionMapping value)
        {
            this.innerDictionary.Add(key, value);
        }

        /// <summary>
        /// Removes a data type definition mapping from the dictionary.
        /// </summary>
        /// <param name="key">A type.</param>
        /// <returns></returns>
        public bool Remove(Type key)
        {
            return this.innerDictionary.Remove(key);
        }

        public bool TryGetValue(Type key, out IDataTypeDefinitionMapping value)
        {
            return this.innerDictionary.TryGetValue(key, out value);
        }
    }
}