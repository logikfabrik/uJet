// <copyright file="JetConfigurationManagerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Configuration
{
    using Jet.Configuration;
    using Shouldly;
    using Xunit;

    public class JetConfigurationManagerTest
    {
        [Theory]
        [InlineData(SynchronizationMode.DataTypes)]
        [InlineData(SynchronizationMode.DocumentTypes)]
        [InlineData(SynchronizationMode.MediaTypes)]
        [InlineData(SynchronizationMode.MemberTypes)]
        public void CanGetDefaultSynchronizationMode(SynchronizationMode synchronizationMode)
        {
            JetConfigurationManager.Synchronize.HasFlag(synchronizationMode).ShouldBeTrue();
        }
    }
}