// <copyright file="Container.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see cref="Container" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class Container : IContainer
    {
        private readonly IDictionary<Type, Type> _types = new Dictionary<Type, Type>();
        private readonly IDictionary<Type, object> _instances = new Dictionary<Type, object>();

        /// <inheritdoc />
        public void Register<TContract, TImplementation>()
            where TImplementation : TContract
        {
            _types[typeof(TContract)] = typeof(TImplementation);
        }

        /// <inheritdoc />
        public void Register<TContract>(TContract instance)
        {
            _instances[typeof(TContract)] = instance;
        }

        /// <inheritdoc />
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        private object Resolve(Type contract)
        {
            if (_instances.TryGetValue(contract, out var instance))
            {
                return instance;
            }

            var implementation = GetImplementation(contract);
            var constructor = implementation.GetConstructors()[0];
            var parameters = constructor.GetParameters();

            instance = parameters.Length == 0
                ? Activator.CreateInstance(implementation)
                : constructor.Invoke(parameters.Select(parameterInfo => Resolve(parameterInfo.ParameterType)).ToArray());

            _instances[instance.GetType()] = instance;

            return instance;
        }

        private Type GetImplementation(Type contract)
        {
            var implementations = _types.Where(pair => contract.IsAssignableFrom(pair.Key)).Select(pair => pair.Value);

            var implementation = implementations.SingleOrDefault();

            return implementation ?? contract;
        }
    }
}