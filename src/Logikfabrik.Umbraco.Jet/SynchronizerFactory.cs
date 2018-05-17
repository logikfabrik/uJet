// <copyright file="SynchronizerFactory.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Collections.Generic;
    using Configuration;
    using EnsureThat;
    using Web.Mvc;

    /// <summary>
    /// The <see cref="SynchronizerFactory" /> class.
    /// </summary>
    public class SynchronizerFactory
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizerFactory" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SynchronizerFactory(IContainer container)
        {
            Ensure.That(container).IsNotNull();

            _container = container;
        }

        public IEnumerable<ISynchronizer> Create(SynchronizationModes synchronize)
        {
            var synchronizers = new List<ISynchronizer>();

            if (synchronize.HasFlag(SynchronizationModes.DataTypes))
            {
                synchronizers.Add(_container.Resolve<DataTypeSynchronizer>());
            }

            if (synchronize.HasFlag(SynchronizationModes.DocumentTypes))
            {
                synchronizers.Add(_container.Resolve<TemplateSynchronizer>());
                synchronizers.Add(_container.Resolve<PreviewTemplateSynchronizer>());
                synchronizers.Add(_container.Resolve<DocumentTypeSynchronizer>());
            }

            if (synchronize.HasFlag(SynchronizationModes.MediaTypes))
            {
                synchronizers.Add(_container.Resolve<MediaTypeSynchronizer>());
            }

            if (synchronize.HasFlag(SynchronizationModes.MemberTypes))
            {
                synchronizers.Add(_container.Resolve<MemberTypeSynchronizer>());
            }

            return synchronizers;
        }
    }
}
