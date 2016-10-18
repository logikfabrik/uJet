// <copyright file="LoggingExtensions.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Extensions
{
    using System;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.ObjectResolution;

    /// <summary>
    /// The <see cref="LoggingExtensions" /> class. Logging extension methods which wraps the standard Umbraco logging system.
    /// </summary>
    internal static class LoggingExtensions
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(() =>
        {
            if (!ResolverBase<LoggerResolver>.HasCurrent || !ResolverBase<LoggerResolver>.Current.HasValue)
            {
                return null;
            }

            return ResolverBase<LoggerResolver>.Current.Logger;
        });

        /// <summary>
        /// Writes the specified debug message to the log.
        /// </summary>
        /// <typeparam name="T">The sender type.</typeparam>
        /// <param name="sender">The message sender.</param>
        /// <param name="message">The message to write.</param>
        public static void Debug<T>(this T sender, string message)
        {
            Logger.Value?.Debug<T>(message);
        }

        /// <summary>
        /// Writes the specified message to the log.
        /// </summary>
        /// <typeparam name="T">The sender type.</typeparam>
        /// <param name="sender">The message sender.</param>
        /// <param name="message">The message to write.</param>
        public static void Info<T>(this T sender, string message)
        {
            Logger.Value?.Info<T>(message);
        }

        /// <summary>
        /// Writes the specified warning message to the log.
        /// </summary>
        /// <typeparam name="T">The sender type.</typeparam>
        /// <param name="sender">The message sender.</param>
        /// <param name="message">The message to write.</param>
        public static void Warn<T>(this T sender, string message)
        {
            Logger.Value?.Warn<T>(message);
        }

        /// <summary>
        /// Writes the specified error message to the log.
        /// </summary>
        /// <typeparam name="T">The sender type.</typeparam>
        /// <param name="sender">The message sender.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="ex">The exception to which this message refers, if any.</param>
        public static void Error<T>(this T sender, string message, Exception ex = null)
        {
            Logger.Value?.Error<T>(message, ex ?? new Exception());
        }
    }
}