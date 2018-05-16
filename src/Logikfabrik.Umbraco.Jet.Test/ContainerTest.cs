// <copyright file="ContainerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using Shouldly;
    using Xunit;

    public class ContainerTest
    {
        private interface IService
        {
        }

        [Fact]
        public void CanResolveByContractType()
        {
            var container = new Container();

            container.Register<IContainer, Container>();

            var resolved = container.Resolve<IContainer>();

            resolved.ShouldNotBeNull();
        }

        [Fact]
        public void CanResolveByImplementationType()
        {
            var container = new Container();

            container.Register<IContainer, Container>();

            var resolved = container.Resolve<Container>();

            resolved.ShouldNotBeNull();
        }

        [Fact]
        public void CanResolveByInstanceWithImplicitContract()
        {
            var instance = new Service();

            var container = new Container();

            container.Register(instance);

            var resolved = container.Resolve<Service>();

            resolved.ShouldBeSameAs(instance);
        }

        [Fact]
        public void CanResolveByInstanceWithExplicitContract()
        {
            var instance = new Service();

            var container = new Container();

            container.Register<IService>(instance);

            var resolved = container.Resolve<IService>();

            resolved.ShouldBeSameAs(instance);
        }

        private class Service : IService
        {
        }
    }
}
