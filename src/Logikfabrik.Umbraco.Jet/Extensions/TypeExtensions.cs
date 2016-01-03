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
        /// Determines whether the type is a content model type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a content model type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsContentType<T>(this Type type)
            where T : ContentTypeAttribute
        {
            return IsValidAndHasAttribute<T>(type);
        }

        /// <summary>
        /// Determines whether the type is a document model type, annotated using the <see cref="DocumentTypeAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a document model type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDocumentType(this Type type)
        {
            return IsValidAndHasAttribute<DocumentTypeAttribute>(type);
        }

        /// <summary>
        /// Determines whether the type is a data model type, annotated using the <see cref="DataTypeAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a data model type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDataType(this Type type)
        {
            return IsValidAndHasAttribute<DataTypeAttribute>(type);
        }

        /// <summary>
        /// Determines whether the type is a media model type, annotated using the <see cref="MediaTypeAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a media model type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMediaType(this Type type)
        {
            return IsValidAndHasAttribute<MediaTypeAttribute>(type);
        }

        /// <summary>
        /// Determines whether the type is a member model type, annotated using the <see cref="MemberTypeAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a member model type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMemberType(this Type type)
        {
            return IsValidAndHasAttribute<MemberTypeAttribute>(type);
        }

        /// <summary>
        /// Determines whether the type is valid and has an attribute of the specified type.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type is valid and has an attribute of the specified type; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValidAndHasAttribute<T>(Type type)
            where T : Attribute
        {
            if (type == null)
            {
                return false;
            }

            try
            {
                return type.GetCustomAttribute<T>(false) != null && IsValidType(type);
            }
            catch (TypeLoadException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the type is a valid.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type is valid; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValidType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (type.IsAbstract)
            {
                return false;
            }

            return type.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}