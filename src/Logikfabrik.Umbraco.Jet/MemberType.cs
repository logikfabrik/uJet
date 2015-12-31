// <copyright file="MemberType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using Extensions;

    /// <summary>
    /// The <see cref="MemberType" /> class. The model type for member model types.
    /// </summary>
    public class MemberType : BaseType<MemberTypeAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberType" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="type" /> is not a member model type.</exception>
        public MemberType(Type type)
            : base(type)
        {
            if (!type.IsMemberType())
            {
                throw new ArgumentException($"Type {type} is not a member model type.", nameof(type));
            }
        }
    }
}
