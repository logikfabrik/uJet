// <copyright file="IContainer.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    /// <summary>
    /// The <see cref="IContainer" /> interface.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Registers the specified implementation for the specified contract.
        /// </summary>
        /// <typeparam name="TContract">The contract type.</typeparam>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        void Register<TContract, TImplementation>()
            where TImplementation : TContract;

        /// <summary>
        /// Registers the specified instance for the specified contract.
        /// </summary>
        /// <typeparam name="TContract">The contract type.</typeparam>
        /// <param name="instance">The instance.</param>
        void Register<TContract>(TContract instance);

        T Resolve<T>();
    }
}