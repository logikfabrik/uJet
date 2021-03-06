﻿// <copyright file="IDefaultValueService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="IDefaultValueService" /> interface.
    /// </summary>
    public interface IDefaultValueService
    {
        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        void SetDefaultValues(IEnumerable<IContent> content);

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        void SetDefaultValues(IEnumerable<IMedia> content);

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        void SetDefaultValues(IEnumerable<IMember> content);
    }
}