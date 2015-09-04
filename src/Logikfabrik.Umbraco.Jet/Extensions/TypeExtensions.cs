//----------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Logikfabrik">
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
