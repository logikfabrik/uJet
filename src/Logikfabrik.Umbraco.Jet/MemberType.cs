// <copyright file="MemberType.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="MemberType" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class MemberType : ContentTypeModel<MemberTypeAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberType" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public MemberType(Type modelType)
            : base(modelType)
        {
        }
    }
}