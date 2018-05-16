// <copyright file="ModelPropertyClassData.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ModelPropertyClassData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { typeof(string) };
            yield return new object[] { typeof(int) };
            yield return new object[] { typeof(int?) };
            yield return new object[] { typeof(decimal) };
            yield return new object[] { typeof(decimal?) };
            yield return new object[] { typeof(float) };
            yield return new object[] { typeof(float?) };
            yield return new object[] { typeof(DateTime) };
            yield return new object[] { typeof(DateTime?) };
            yield return new object[] { typeof(bool) };
            yield return new object[] { typeof(bool?) };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
