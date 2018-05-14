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
        [InlineData(SynchronizationModes.DataTypes)]
        [InlineData(SynchronizationModes.DocumentTypes)]
        [InlineData(SynchronizationModes.MediaTypes)]
        [InlineData(SynchronizationModes.MemberTypes)]
        public void CanGetDefaultSynchronizationMode(SynchronizationModes synchronizationModes)
        {
            JetConfigurationManager.Synchronize.HasFlag(synchronizationModes).ShouldBeTrue();
        }
    }
}