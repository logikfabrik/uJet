// <copyright file="JetConfigurationManagerTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Configuration
{
    using Jet.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="JetConfigurationManagerTest" /> class.
    /// </summary>
    [TestClass]
    public class JetConfigurationManagerTest
    {
        /// <summary>
        /// Test for synchronization mode <see cref="SynchronizationMode.DataTypes" /> in <c>app.config</c>.
        /// </summary>
        [TestMethod]
        public void CanGetSynchronizationModeForDataTypes()
        {
            Assert.IsTrue(JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DataTypes));
        }

        /// <summary>
        /// Test for synchronization mode <see cref="SynchronizationMode.DocumentTypes" /> in <c>app.config</c>.
        /// </summary>
        [TestMethod]
        public void CanGetSynchronizationModeForDocumentTypes()
        {
            Assert.IsTrue(JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DocumentTypes));
        }

        /// <summary>
        /// Test for synchronization mode <see cref="SynchronizationMode.MediaTypes" /> in <c>app.config</c>.
        /// </summary>
        [TestMethod]
        public void CanGetSynchronizationModeForMediaTypes()
        {
            Assert.IsTrue(JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.MediaTypes));
        }

        /// <summary>
        /// Test for synchronization mode <see cref="SynchronizationMode.MemberTypes" /> in <c>app.config</c>.
        /// </summary>
        [TestMethod]
        public void CanGetSynchronizationModeForMemberTypes()
        {
            Assert.IsTrue(JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.MemberTypes));
        }
    }
}
