// <copyright file="MemberTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="MemberTypeAttribute" /> class. Attribute for model type annotation.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]

    // ReSharper disable once InheritdocConsiderUsage
    public class MemberTypeAttribute : ContentTypeModelAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public MemberTypeAttribute(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public MemberTypeAttribute(string id, string name)
            : base(id, name)
        {
        }
    }
}
