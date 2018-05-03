// <copyright file="AliasAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using EnsureThat;

    /// <summary>
    /// The <see cref="AliasAttribute" /> class.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property)]

    // ReSharper disable once InheritdocConsiderUsage
    public class AliasAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AliasAttribute" /> class.
        /// </summary>
        /// <param name="alias">The alias.</param>
        // ReSharper disable once InheritdocConsiderUsage
        // ReSharper disable once UnusedMember.Global
        public AliasAttribute(string alias)
        {
            EnsureArg.IsNotNullOrWhiteSpace(alias);

            Alias = alias;
        }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias { get; }
    }
}
