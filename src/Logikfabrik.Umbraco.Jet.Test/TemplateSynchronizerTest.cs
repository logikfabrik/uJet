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
    using Moq;
    using Shouldly;
    using Xunit;

    public class TemplateSynchronizerTest
    {
        [Fact]
        public void CanGetTemplatesToAdd()
        {
            var service = GetTemplateSynchronizer();

            var templatesToAdd = service.GetTemplatesToAdd(service.GetTemplatesToAdd());

            templatesToAdd.Count().ShouldBe(2);
        }

        [Fact]
        public void CanGetLayoutForTemplateWithLayout()
        {
            var service = GetTemplateSynchronizer();

            var template = GetTemplateMock(GetTemplatePath("Template1.cshtml"));

            service.GetLayout(template).ShouldBe("Template1Master");
        }

        [Fact]
        public void CannotGetLayoutForTemplateWithoutLayout()
        {
            var service = GetTemplateSynchronizer();

            var template = GetTemplateMock(GetTemplatePath("Template2.cshtml"));

            service.GetLayout(template).ShouldBeNull();
        }

        [Fact]
        public void CanGetPathsToTemplatesToAdd()
        {
            var service = GetTemplateSynchronizer();

            var templatesToAdd = service.GetTemplatesToAdd();

            GetTemplatePath("Template1.cshtml").ShouldBe(templatesToAdd.First());
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