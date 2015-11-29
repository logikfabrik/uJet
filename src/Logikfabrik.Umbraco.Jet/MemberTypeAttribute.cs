// <copyright file="MemberTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="MemberTypeAttribute" /> class. Attribute for member type annotation.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]
    public class MemberTypeAttribute : BaseTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public MemberTypeAttribute(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public MemberTypeAttribute(string id, string name)
            : base(id, name)
        {
        }
    }
}
