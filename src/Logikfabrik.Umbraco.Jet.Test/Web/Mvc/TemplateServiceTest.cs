// <copyright file="TemplateServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Mvc
{
    using System.IO;
    using System.Linq;
    using Jet.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="TemplateServiceTest" /> class.
    /// </summary>
    [TestClass]
    public class TemplateServiceTest
    {
        /// <summary>
        /// Test to get template paths.
        /// </summary>
        [TestMethod]
        public void CanGetTemplatePaths()
        {
            var templates = TemplateService.Instance.TemplatePaths;

            Assert.AreEqual(2, templates.Count());
        }

        /// <summary>
        /// Test to get template content.
        /// </summary>
        [TestMethod]
        public void CanGetTemplateContent()
        {
            var templatePath = TemplateService.Instance.TemplatePaths.First();

            Assert.AreEqual(File.ReadAllText(templatePath), TemplateService.Instance.GetContent(templatePath));
        }

        /// <summary>
        /// Test to get template.
        /// </summary>
        [TestMethod]
        public void CanGetTemplate()
        {
            var templatePath = TemplateService.Instance.TemplatePaths.First();

            Assert.IsNotNull(TemplateService.Instance.GetTemplate(templatePath));
        }
    }
}
