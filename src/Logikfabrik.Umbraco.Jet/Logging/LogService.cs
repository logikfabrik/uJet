// <copyright file="LogService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Logging
{
    using System;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.ObjectResolution;

    /// <summary>
    /// The <see cref="LogService" /> class.
    /// </summary>
    public class LogService : ILogService
    {
        private readonly Lazy<ILogger> _logger = new Lazy<ILogger>(() =>
        {
            if (!ResolverBase<LoggerResolver>.HasCurrent || !ResolverBase<LoggerResolver>.Current.HasValue)
            {
                return null;
            }

            return ResolverBase<LoggerResolver>.Current.Logger;
        });

        /// <summary>
        /// Logs the specified entry.
        /// </summary>
        /// <typeparam name="T">The logging type.</typeparam>
        /// <param name="entry">The entry.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entry" /> is <c>null</c>.</exception>
        public void Log<T>(LogEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            var logger = _logger.Value;

            if (logger == null)
            {
                return;
            }

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (entry.Type)
            {
                case LogEntryType.Debug:
                    logger.Debug<T>(entry.Message);

                    break;

                case LogEntryType.Information:
                    logger.Info<T>(entry.Message);

                    break;

                case LogEntryType.Warning:
                    if (entry.Exception == null)
                    {
                        logger.Warn<T>(entry.Message);
                    }
                    else
                    {
                        logger.WarnWithException<T>(entry.Message, entry.Exception);
                    }

                    break;

                case LogEntryType.Error:
                    logger.Error<T>(entry.Message, entry.Exception);

                    break;
            }
        }
    }
}
