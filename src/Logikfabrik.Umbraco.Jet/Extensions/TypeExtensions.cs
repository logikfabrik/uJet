// <copyright file="TypeExtensions.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
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
        /// <typeparam name="T">The <see cref="TypeModelAttribute" /> type.</typeparam>
        /// <param name="modelType">The model type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a model type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsModelType<T>(this Type modelType)
            where T : TypeModelAttribute
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
        /// Determines whether the type is a document type model type, annotated using the <see cref="DocumentTypeAttribute" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a document type model type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDocumentType(this Type modelType)
        {
            return IsModelType<DocumentTypeAttribute>(modelType);
        }

        /// <summary>
        /// Determines whether the type is a data type model type, annotated using the <see cref="DataTypeAttribute" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a data type model type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDataType(this Type modelType)
        {
            return IsModelType<DataTypeAttribute>(modelType);
        }

        /// <summary>
        /// Determines whether the type is a media type model type, annotated using the <see cref="MediaTypeAttribute" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a media type model type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMediaType(this Type modelType)
        {
            return IsModelType<MediaTypeAttribute>(modelType);
        }

        /// <summary>
        /// Determines whether the type is a member type model type, annotated using the <see cref="MemberTypeAttribute" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a member type model type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMemberType(this Type modelType)
        {
            return IsModelType<MemberTypeAttribute>(modelType);
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
            if (modelType == null)
            {
                return false;
            }

            if (modelType.IsAbstract)
            {
                return false;
            }

            return modelType.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}