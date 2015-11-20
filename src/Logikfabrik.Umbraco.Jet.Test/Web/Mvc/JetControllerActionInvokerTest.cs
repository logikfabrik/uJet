// <copyright file="JetControllerActionInvokerTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Mvc
{
    using Jet.Extensions;
    using Jet.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="JetControllerActionInvokerTest" /> class.
    /// </summary>
    [TestClass]
    public class JetControllerActionInvokerTest
    {
        /// <summary>
        /// Test for action name.
        /// </summary>
        [TestMethod]
        public void CanGetActionNameForPreviewAction()
        {
            Assert.AreEqual("Index", JetControllerActionInvoker.GetActionName(PreviewTemplateAttribute.TemplateName.Alias()));
        }

        /// <summary>
        /// Test for action name.
        /// </summary>
        [TestMethod]
        public void CanGetActionNameForIndexAction()
        {
            Assert.AreEqual("Index", JetControllerActionInvoker.GetActionName("Index"));
        }
    }
}
