// <copyright file="TemplateSynchronizationServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Mvc
{
    using System;
    using System.IO;
    using System.Linq;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Jet.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The <see cref="TemplateSynchronizationServiceTest" /> class.
    /// </summary>
    [TestClass]
    public class TemplateSynchronizationServiceTest
    {
        /// <summary>
        /// Test to get templates to add.
        /// </summary>
        [TestMethod]
        public void CanGetTemplatesToAdd()
        {
            var service = GetTemplateSynchronizationService();

            var templatesToAdd = service.GetTemplatesToAdd(service.GetTemplatesToAdd());

            Assert.AreEqual(2, templatesToAdd.Count());
        }

        /// <summary>
        /// Test to get layout (master) for template with layout.
        /// </summary>
        [TestMethod]
        public void CanGetLayoutForTemplateWithLayout()
        {
            var service = GetTemplateSynchronizationService();

            var template = GetTemplateMock(GetTemplatePath("Template1.cshtml"));

            Assert.AreEqual("Template1Master", service.GetLayout(template));
        }

        /// <summary>
        /// Test to get layout (master) for template without layout.
        /// </summary>
        [TestMethod]
        public void CannotGetLayoutForTemplateWithoutLayout()
        {
            var service = GetTemplateSynchronizationService();

            var template = GetTemplateMock(GetTemplatePath("Template2.cshtml"));

            Assert.IsNull(service.GetLayout(template));
        }

        /// <summary>
        /// Test to get paths to templates to add.
        /// </summary>
        [TestMethod]
        public void CanGetPathsToTemplatesToAdd()
        {
            var service = GetTemplateSynchronizationService();

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

        private static TemplateSynchronizationService GetTemplateSynchronizationService()
        {
            var fileServiceMock = new Mock<IFileService>();
            var templateServiceMock = new Mock<ITemplateService>();

            templateServiceMock.Setup(m => m.TemplatePaths).Returns(TemplateService.Instance.TemplatePaths);
            templateServiceMock.Setup(m => m.GetTemplate(It.IsAny<string>())).Returns<string>(GetTemplateMock);

            return new TemplateSynchronizationService(fileServiceMock.Object, templateServiceMock.Object);
        }
    }
}
