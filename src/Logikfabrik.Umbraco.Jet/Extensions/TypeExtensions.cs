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
        /// Determines whether the type is a document type, annotated using the <see cref="DocumentTypeAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a document type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDocumentType(this Type type)
        {
            try
            {
                if (!IsValidType(type))
                {
                    return false;
                }

                return type.GetCustomAttribute<DocumentTypeAttribute>() != null;
            }
            catch (TypeLoadException)
            {
                return false;
            }
            catch (ReflectionTypeLoadException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the type is a data type, annotated using the <see cref="DataTypeAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a data type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDataType(this Type type)
        {
            try
            {
                if (!IsValidType(type))
                {
                    return false;
                }

                return type.GetCustomAttribute<DataTypeAttribute>() != null;
            }
            catch (TypeLoadException)
            {
                return false;
            }
            catch (ReflectionTypeLoadException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the type is a media type, annotated using the <see cref="MediaTypeAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a media type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMediaType(this Type type)
        {
            try
            {
                if (!IsValidType(type))
                {
                    return false;
                }

                return type.GetCustomAttribute<MediaTypeAttribute>() != null;
            }
            catch (TypeLoadException)
            {
                return false;
            }
            catch (ReflectionTypeLoadException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the type is a member type, annotated using the <see cref="MemberTypeAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type is a member type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMemberType(this Type type)
        {
            try
            {
                if (!IsValidType(type))
                {
                    return false;
                }

                return type.GetCustomAttribute<MemberTypeAttribute>() != null;
            }
            catch (TypeLoadException)
            {
                return false;
            }
            catch (ReflectionTypeLoadException)
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
