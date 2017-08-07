// <copyright file="ILogService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Logging
{
    /// <summary>
    /// The <see cref="ILogService" /> interface.
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Logs the specified entry.
        /// </summary>
        /// <typeparam name="T">The logging type.</typeparam>
        /// <param name="entry">The entry.</param>
        void Log<T>(LogEntry entry);
    }
}
