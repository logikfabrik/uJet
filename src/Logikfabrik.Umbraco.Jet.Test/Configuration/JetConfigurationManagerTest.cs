// <copyright file="JetConfigurationManagerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Configuration
{
    using Jet.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JetConfigurationManagerTest
    {
        [TestMethod]
        public void CanGetSynchronizationModeForDataTypes()
        {
            Assert.IsTrue(JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DataTypes));
        }

        [TestMethod]
        public void CanGetSynchronizationModeForDocumentTypes()
        {
            Assert.IsTrue(JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DocumentTypes));
        }

        [TestMethod]
        public void CanGetSynchronizationModeForMediaTypes()
        {
            Assert.IsTrue(JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.MediaTypes));
        }

        [TestMethod]
        public void CanGetSynchronizationModeForMemberTypes()
        {
            Assert.IsTrue(JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.MemberTypes));
        }
    }
}
