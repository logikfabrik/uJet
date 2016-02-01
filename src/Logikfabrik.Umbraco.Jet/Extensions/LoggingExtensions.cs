using Umbraco.Core.Logging;

namespace Logikfabrik.Umbraco.Jet.Extensions
{
    using System;

    /// <summary>
    /// Static logging extensions which wraps the standard Umbraco logging system.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="sender">The sender of the debug message.</param>
        /// <param name="msg">The debug message by itself.</param>
        /// <typeparam name="T">The type of the sender of the message.</typeparam>
        public static void Debug<T>(this T sender, string msg)
        {
            LogHelper.Debug<T>(msg);
        }

        /// <summary>
        /// Writes an Info message into the log.
        /// </summary>
        /// <param name="sender">The sender of the info message.</param>
        /// <param name="msg">The debug message by itself.</param>
        /// <typeparam name="T">The type of the sender of the message.</typeparam>
        public static void Info<T>(this T sender, string msg)
        {
            LogHelper.Info<T>(msg);
        }

        /// <summary>
        /// Writes an Warning message into the log.
        /// </summary>
        /// <param name="sender">The sender of the warning message.</param>
        /// <param name="msg">The debug message by itself.</param>
        /// <param name="exc">The exception where this warning is based on.</param>
        /// <typeparam name="T">The type of the sender of the message.</typeparam>
        public static void Warn<T>(this T sender, string msg, Exception exc = null)
        {
            LogHelper.Warn<T>(string.Format("{0} > {1}{2}\t{3}", msg, exc.Message, Environment.NewLine, exc));
        }

        /// <summary>
        /// Writes an Error message into the log.
        /// </summary>
        /// <param name="sender">The sender of the error message.</param>
        /// <param name="msg">The debug message by itself.</param>
        /// <param name="exc">The exception where this fatal error is based on.</param>
        /// <typeparam name="T">The type of the sender of the message.</typeparam>
        public static void Fatal<T>(this T sender, string msg, Exception exc = null)
        {
            LogHelper.Error<T>(msg, 
                exc == null ? new Exception(msg) : exc);
        }
    }
}
