﻿// The MIT License (MIT)

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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    public class DataTypeDefinitionMappingDictionary : IDictionary<Type, IDataTypeDefinitionMapping>
    {
        private IDefaultDataTypeDefinitionMapping _defaultMapping;
        private readonly Dictionary<Type, IDataTypeDefinitionMapping> _innerDictionary = new Dictionary<Type, IDataTypeDefinitionMapping>();

        public IDefaultDataTypeDefinitionMapping DefaultMapping
        {
            get { return _defaultMapping ?? (_defaultMapping = new DefaultDataTypeDefinitionMapping()); }
            set { _defaultMapping = value; }
        }

        public IEnumerator<KeyValuePair<Type, IDataTypeDefinitionMapping>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<Type, IDataTypeDefinitionMapping> item)
        {
            _innerDictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _innerDictionary.Clear();
        }

        public bool Contains(KeyValuePair<Type, IDataTypeDefinitionMapping> item)
        {
            return _innerDictionary.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<Type, IDataTypeDefinitionMapping>[] array, int index)
        {
            var a = _innerDictionary.ToArray();

            a.CopyTo(array, index);
        }

        public bool Remove(KeyValuePair<Type, IDataTypeDefinitionMapping> item)
        {
            return _innerDictionary.Remove(item.Key);
        }

        public int Count { get { return _innerDictionary.Count; } }

        public bool IsReadOnly { get { return false; } }

        public bool ContainsKey(Type key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        public void Add(Type key, IDataTypeDefinitionMapping value)
        {
            _innerDictionary.Add(key, value);
        }

        public bool Remove(Type key)
        {
            return _innerDictionary.Remove(key);
        }

        public bool TryGetValue(Type key, out IDataTypeDefinitionMapping value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        public IDataTypeDefinitionMapping this[Type key]
        {
            get { return _innerDictionary[key]; }
            set { _innerDictionary[key] = value; }
        }

        public ICollection<Type> Keys { get { return _innerDictionary.Keys; } }

        public ICollection<IDataTypeDefinitionMapping> Values { get { return _innerDictionary.Values; } }
    }
}