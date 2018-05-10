// <copyright file="TypeExtensions.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Extensions
{
    using System;
    using System.Reflection;

    /// <summary>
    /// The <see cref="TypeExtensions" /> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the type is a model type.
        /// </summary>
        /// <typeparam name="T">The <see cref="ModelTypeAttribute" /> type.</typeparam>
        /// <param name="modelType">The model type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a model type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsModelType<T>(this Type modelType)
            where T : ModelTypeAttribute
        {
            if (modelType == null)
            {
                return false;
            }

            try
            {
                return modelType.GetCustomAttribute<T>(false) != null && IsValid(modelType);
            }
            catch (TypeLoadException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the type is valid.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <returns>
        ///   <c>true</c> if the type is valid; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValid(Type modelType)
        {
            if (modelType == null || modelType.IsAbstract)
            {
                return false;
            }

            return modelType.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}