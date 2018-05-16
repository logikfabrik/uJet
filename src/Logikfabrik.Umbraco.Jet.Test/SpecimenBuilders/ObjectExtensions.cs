// <copyright file="ObjectExtensions.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    public static class ObjectExtensions
    {
        public static object GetPropertyValue(this object obj, string name)
        {
            var type = obj.GetType();

            // ReSharper disable once PossibleNullReferenceException
            return type.GetProperty(name).GetValue(obj);
        }
    }
}
