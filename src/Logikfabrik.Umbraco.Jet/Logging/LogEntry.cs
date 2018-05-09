// <copyright file="LogEntry.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Logging
{
    using System;
    using EnsureThat;

    /// <summary>
    /// The <see cref="LogEntry" /> class.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public LogEntry(LogEntryType type, string message)
            : this(type, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public LogEntry(LogEntryType type, string message, Exception exception)
        {
            Ensure.That(message).IsNotNullOrWhiteSpace();

            Type = type;
            Message = message;
            Exception = exception;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public LogEntryType Type { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; }
    }
}
