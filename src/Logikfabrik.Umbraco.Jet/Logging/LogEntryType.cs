// <copyright file="LogEntryType.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Logging
{
    /// <summary>
    /// The <see cref="LogEntryType" /> enumerable.
    /// </summary>
    public enum LogEntryType
    {
        /// <summary>
        /// Type for debug entry.
        /// </summary>
        Debug,

        /// <summary>
        /// Type for information entry.
        /// </summary>
        Information,

        /// <summary>
        /// Type for warning entry.
        /// </summary>
        Warning,

        /// <summary>
        /// Type for error entry.
        /// </summary>
        Error
    }
}
