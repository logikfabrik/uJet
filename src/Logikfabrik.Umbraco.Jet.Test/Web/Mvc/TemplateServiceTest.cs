// <copyright file="TemplateServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Mvc
{
    using System.Linq;
    using Jet.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TemplateServiceTest
    {
        [TestMethod]
        public void CanGetTemplates()
        {
            var templates = TemplateService.Instance.TemplatePaths;

            Assert.AreEqual(2, templates.Count());
        }

        [TestMethod]
        public void CanGetTemplateContent()
        {
            var templatePath = TemplateService.Instance.TemplatePaths.First();
            var content = TemplateService.Instance.GetContent(templatePath);

            Assert.AreEqual("Test template Template1.cshtml", content);
        }
    }
}
