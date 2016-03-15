// <copyright file="LoggingExtensions.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Extensions
{
    using System;
    using global::Umbraco.Core.Logging;

    /// <summary>
    /// The <see cref="LoggingExtensions" /> class. Logging extension methods which wraps the standard Umbraco logging system.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Writes the specified debug message to the log.
        /// </summary>
        /// <typeparam name="T">The sender type.</typeparam>
        /// <param name="sender">The message sender.</param>
        /// <param name="message">The message to write.</param>
        public static void Debug<T>(this T sender, string message)
        {
            LogHelper.Debug<T>(message);
        }

        /// <summary>
        /// Writes the specified message to the log.
        /// </summary>
        /// <typeparam name="T">The sender type.</typeparam>
        /// <param name="sender">The message sender.</param>
        /// <param name="message">The message to write.</param>
        public static void Info<T>(this T sender, string message)
        {
            LogHelper.Info<T>(message);
        }

        /// <summary>
        /// Writes the specified warning message to the log.
        /// </summary>
        /// <typeparam name="T">The sender type.</typeparam>
        /// <param name="sender">The message sender.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="ex">The exception to which this message refers, if any.</param>
        public static void Warn<T>(this T sender, string message, Exception ex = null)
        {
            LogHelper.Warn<T>($"{message} > {ex?.Message}{Environment.NewLine}\t{ex}");
        }

        /// <summary>
        /// Writes the specified error message to the log.
        /// </summary>
        /// <typeparam name="T">The sender type.</typeparam>
        /// <param name="sender">The message sender.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="ex">The exception to which this message refers, if any.</param>
        public static void Fatal<T>(this T sender, string message, Exception ex = null)
        {
            LogHelper.Error<T>(message, ex ?? new Exception(message));
        }
    }
}
