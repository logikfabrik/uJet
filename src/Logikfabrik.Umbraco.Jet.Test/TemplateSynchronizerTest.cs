// <copyright file="TemplateSynchronizerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class TemplateSynchronizerTest
    {
        [TestMethod]
        public void CanGetTemplatesToAdd()
        {
            var service = GetTemplateSynchronizer();

            var templatesToAdd = service.GetTemplatesToAdd(service.GetTemplatesToAdd());

            Assert.AreEqual(2, templatesToAdd.Count());
        }

        [TestMethod]
        public void CanGetLayoutForTemplateWithLayout()
        {
            var service = GetTemplateSynchronizer();

            var template = GetTemplateMock(GetTemplatePath("Template1.cshtml"));

            Assert.AreEqual("Template1Master", service.GetLayout(template));
        }

        [TestMethod]
        public void CannotGetLayoutForTemplateWithoutLayout()
        {
            var service = GetTemplateSynchronizer();

            var template = GetTemplateMock(GetTemplatePath("Template2.cshtml"));

            Assert.IsNull(service.GetLayout(template));
        }

        [TestMethod]
        public void CanGetPathsToTemplatesToAdd()
        {
            var service = GetTemplateSynchronizer();

            var templatesToAdd = service.GetTemplatesToAdd();

            Assert.AreEqual(templatesToAdd.First(), GetTemplatePath("Template1.cshtml"));
        }

        private static string GetTemplatePath(string fileName)
        {
            return string.Format("{0}{1}Views{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, fileName);
        }

        private static ITemplate GetTemplateMock(string templatePath)
        {
            var templateMock = new Mock<ITemplate>();

            templateMock.Setup(m => m.Path).Returns(templatePath);
            templateMock.Setup(m => m.Content).Returns(System.IO.File.ReadAllText(templatePath));

            return templateMock.Object;
        }

        private static TemplateSynchronizer GetTemplateSynchronizer()
        {
            var fileServiceMock = new Mock<IFileService>();
            var templateServiceMock = new Mock<ITemplateService>();

            templateServiceMock.Setup(m => m.TemplatePaths).Returns(TemplateService.Instance.TemplatePaths);
            templateServiceMock.Setup(m => m.GetTemplate(It.IsAny<string>())).Returns<string>(GetTemplateMock);

            return new TemplateSynchronizer(fileServiceMock.Object, templateServiceMock.Object);
        }
    }
}