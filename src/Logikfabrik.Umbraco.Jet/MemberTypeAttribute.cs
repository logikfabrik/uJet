// <copyright file="MemberTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="MemberTypeAttribute" /> class.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]
    public class MemberTypeAttribute : IdAttribute
    {
    }
}
