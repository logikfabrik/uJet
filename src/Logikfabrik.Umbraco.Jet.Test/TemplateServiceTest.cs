// <copyright file="TemplateServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TemplateServiceTest
    {
        [TestMethod]
        public void CanGetTemplatePaths()
        {
            var templates = TemplateService.Instance.TemplatePaths;

            Assert.AreEqual(2, templates.Count());
        }

        [TestMethod]
        public void CanGetContent()
        {
            var templatePath = TemplateService.Instance.TemplatePaths.First();

            Assert.AreEqual(File.ReadAllText(templatePath), TemplateService.Instance.GetContent(templatePath));
        }

        [TestMethod]
        public void CanGetTemplate()
        {
            var templatePath = TemplateService.Instance.TemplatePaths.First();

            Assert.IsNotNull(TemplateService.Instance.GetTemplate(templatePath));
        }
    }
}