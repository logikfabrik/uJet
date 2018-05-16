namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Container : IContainer
    {
        private readonly IDictionary<Type, Type> _types = new Dictionary<Type, Type>();
        private readonly IDictionary<Type, object> _instances = new Dictionary<Type, object>();

        public void Register<TContract, TImplementation>()
            where TImplementation : TContract
        {
            _types[typeof(TContract)] = typeof(TImplementation);
        }

        public void Register<TContract>(TContract instance)
        {
            _instances[typeof(TContract)] = instance;
        }

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
