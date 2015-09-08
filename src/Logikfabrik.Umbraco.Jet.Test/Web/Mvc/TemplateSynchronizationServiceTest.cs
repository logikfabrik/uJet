// <copyright file="TemplateSynchronizationServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Mvc
{
    using System.IO;
    using System.Linq;
    using Extensions;
    using Jet.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    [TestClass]
    public class TemplateSynchronizationServiceTest
    {
        [TestMethod]
        public void CanGetTemplatesToAdd()
        {
            var template = new Moq.Mock<ITemplate>();

            template.Setup(m => m.Path).Returns("Views\\Template1.cshtml");
            template.Setup(m => m.Name).Returns("Template1");
            template.Setup(m => m.Alias).Returns("template1");

            var fileService = new Moq.Mock<IFileService>();

            fileService.Setup(m => m.GetTemplates(Moq.It.IsAny<string[]>()))
                .Returns(new[] { template.Object });

            var templateService = new Moq.Mock<ITemplateService>();

            templateService.Setup(m => m.TemplatePaths).Returns(TemplateService.Instance.TemplatePaths);
            templateService.Setup(m =>
                m.GetTemplate(Moq.It.IsAny<string>())).Returns<string>(
                    templatePath =>
                    {
                        var t = new Moq.Mock<ITemplate>();

                        t.Setup(m => m.Path).Returns(templatePath);
                        t.Setup(m => m.Name).Returns(Path.GetFileNameWithoutExtension(templatePath));
                        t.Setup(m => m.Alias).Returns(Path.GetFileNameWithoutExtension(templatePath).Alias());
                        t.Setup(m => m.Content).Returns(TemplateService.Instance.GetContent(templatePath));

                        return t.Object;
                    });

            var service = new TemplateSynchronizationService(fileService.Object, templateService.Object);

            var templatesToAdd1 = service.GetTemplatesToAdd().ToArray();
            var templatesToAdd2 = service.GetTemplatesToAdd(templatesToAdd1).ToArray();

            Assert.AreEqual(1, templatesToAdd1.Length);
            Assert.AreEqual(1, templatesToAdd2.Length);

            var templateToAdd1 = templatesToAdd1.First();
            var templateToAdd2 = templatesToAdd2.First();

            Assert.AreEqual(templateToAdd1, templateToAdd2.Path);
            Assert.AreEqual("Template2", templateToAdd2.Name);
            Assert.AreEqual("template2", templateToAdd2.Alias);
            Assert.AreEqual("Test template Template2.cshtml", templateToAdd2.Content);
        }
    }
}
