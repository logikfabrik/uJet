// <copyright file="SynchronizationMode.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Configuration
{
    using System;

    /// <summary>
    /// The <see cref="SynchronizationMode" /> enumeration.
    /// </summary>
    [Flags]
    public enum SynchronizationMode
    {
        /// <summary>
        /// Do not scan or synchronize any model types.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        None = 0,

        /// <summary>
        /// Scan and synchronize model types annotated using the <see cref="DocumentTypeAttribute" /> class.
        /// </summary>
        DocumentTypes = 1,

        /// <summary>
        /// Scan and synchronize model types annotated using the <see cref="MediaTypeAttribute" /> class.
        /// </summary>
        MediaTypes = 2,

        /// <summary>
        /// Scan and synchronize model types annotated using the <see cref="DataTypeAttribute" /> class.
        /// </summary>
        DataTypes = 4,

        /// <summary>
        /// Scan and synchronize model types annotated using the <see cref="MemberTypeAttribute" /> class.
        /// </summary>
        MemberTypes = 8
    }
}
