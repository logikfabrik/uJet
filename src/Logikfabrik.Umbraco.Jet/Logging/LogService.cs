// <copyright file="LogService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Logging
{
    using System;
    using EnsureThat;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.ObjectResolution;

    /// <summary>
    /// The <see cref="LogService" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
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

        /// <inheritdoc />
        public void Log<T>(LogEntry entry)
        {
            EnsureArg.IsNotNull(entry);

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
