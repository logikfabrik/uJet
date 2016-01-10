// <copyright file="MemberType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="MemberType" /> class. The model type for member types.
    /// </summary>
    public class MemberType : ContentTypeModel<MemberTypeAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberType" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        public MemberType(Type modelType)
            : base(modelType)
        {
        }
    }
}