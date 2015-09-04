//----------------------------------------------------------------------------------
// <copyright file="IContentTypeRepository.cs" company="Logikfabrik">
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

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;

    /// <summary>
    /// The <see cref="IContentTypeRepository" /> interface.
    /// </summary>
    public interface IContentTypeRepository
    {
        /// <summary>
        /// Gets the content type identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The content type identifier</returns>
        int? GetContentTypeId(Guid id);

        /// <summary>
        /// Gets the property type identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The property type identifier.</returns>
        int? GetPropertyTypeId(Guid id);

        /// <summary>
        /// Sets the content type identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="contentTypeId">The content type identifier.</param>
        void SetContentTypeId(Guid id, int contentTypeId);

        /// <summary>
        /// Sets the property type identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="propertyTypeId">The property type identifier.</param>
        void SetPropertyTypeId(Guid id, int propertyTypeId);
    }
}
