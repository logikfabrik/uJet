//----------------------------------------------------------------------------------
// <copyright file="PropertyValueConverterDictionary.cs" company="Logikfabrik">
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

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class PropertyValueConverterDictionary : IDictionary<Type, IEnumerable<IPropertyValueConverter>>
    {
        private readonly Dictionary<Type, IEnumerable<IPropertyValueConverter>> innerDictionary = new Dictionary<Type, IEnumerable<IPropertyValueConverter>>();

        public ICollection<Type> Keys
        {
            get { return this.innerDictionary.Keys; }
        }

        public ICollection<IEnumerable<IPropertyValueConverter>> Values
        {
            get { return this.innerDictionary.Values; }
        }

        public int Count
        {
            get { return this.innerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerable<IPropertyValueConverter> this[Type key]
        {
            get { return this.innerDictionary[key]; }
            set { this.innerDictionary[key] = value; }
        }

        public IEnumerator<KeyValuePair<Type, IEnumerable<IPropertyValueConverter>>> GetEnumerator()
        {
            return this.innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(KeyValuePair<Type, IEnumerable<IPropertyValueConverter>> item)
        {
            this.innerDictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.innerDictionary.Clear();
        }

        public bool Contains(KeyValuePair<Type, IEnumerable<IPropertyValueConverter>> item)
        {
            return this.innerDictionary.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<Type, IEnumerable<IPropertyValueConverter>>[] array, int index)
        {
            var a = this.innerDictionary.ToArray();

            a.CopyTo(array, index);
        }

        public bool Remove(KeyValuePair<Type, IEnumerable<IPropertyValueConverter>> item)
        {
            return this.innerDictionary.Remove(item.Key);
        }
        
        public bool ContainsKey(Type key)
        {
            return this.innerDictionary.ContainsKey(key);
        }

        public void Add(Type key, IEnumerable<IPropertyValueConverter> value)
        {
            this.innerDictionary.Add(key, value);
        }

        public bool Remove(Type key)
        {
            return this.innerDictionary.Remove(key);
        }

        public bool TryGetValue(Type key, out IEnumerable<IPropertyValueConverter> value)
        {
            return this.innerDictionary.TryGetValue(key, out value);
        }
    }
}
