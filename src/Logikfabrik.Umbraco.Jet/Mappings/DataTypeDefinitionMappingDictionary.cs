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

    public class DataTypeDefinitionMappingDictionary : IDictionary<Type, IDataTypeDefinitionMapping>
    {
        private readonly Dictionary<Type, IDataTypeDefinitionMapping> innerDictionary = new Dictionary<Type, IDataTypeDefinitionMapping>();
        private IDefaultDataTypeDefinitionMapping defaultMapping;

        public IDefaultDataTypeDefinitionMapping DefaultMapping
        {
            get { return this.defaultMapping ?? (this.defaultMapping = new DefaultDataTypeDefinitionMapping()); }
            set { this.defaultMapping = value; }
        }
        
        public bool IsReadOnly
        {
            get { return false; }
        }

        public int Count
        {
            get { return this.innerDictionary.Count; }
        }

        public ICollection<Type> Keys
        {
            get { return this.innerDictionary.Keys; }
        }

        public ICollection<IDataTypeDefinitionMapping> Values
        {
            get { return this.innerDictionary.Values; }
        }

        public IDataTypeDefinitionMapping this[Type key]
        {
            get { return this.innerDictionary[key]; }
            set { this.innerDictionary[key] = value; }
        }

        public IEnumerator<KeyValuePair<Type, IDataTypeDefinitionMapping>> GetEnumerator()
        {
            return this.innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(KeyValuePair<Type, IDataTypeDefinitionMapping> item)
        {
            this.innerDictionary.Add(item.Key, item.Value);
        }

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

        public void Add(Type key, IDataTypeDefinitionMapping value)
        {
            this.innerDictionary.Add(key, value);
        }

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