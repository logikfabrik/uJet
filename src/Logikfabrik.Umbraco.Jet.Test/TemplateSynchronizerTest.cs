// <copyright file="TemplateSynchronizerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Moq;
    using Moq.AutoMock;
    using Xunit;

    public class TemplateSynchronizerTest
    {
        [Fact]
        public void CanAddNewTemplates()
        {
            const string templatePath = "template.cshtml";

            var mocker = new AutoMocker();

            var templateServiceMock = mocker.GetMock<ITemplateService>();

            templateServiceMock.Setup(m => m.TemplatePaths).Returns(new[] { templatePath });
            templateServiceMock.Setup(m => m.GetTemplate(templatePath)).Returns(Mock.Of<ITemplate>());

            var fileServiceMock = mocker.GetMock<IFileService>();

            fileServiceMock.Setup(m => m.GetTemplates()).Returns(new List<ITemplate>());
            fileServiceMock.Setup(m => m.SaveTemplate(It.IsAny<ITemplate[]>(), 0)).Verifiable();

            var templateSynchronizer = mocker.CreateInstance<TemplateSynchronizer>();

            templateSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Fact]
        public void CanUpdateTemplates()
        {
            var mocker = new AutoMocker();

            var templateMock = new Mock<ITemplate>();

            templateMock.Setup(m => m.Content).Returns("@{Layout=\"master.cshtml\";}");

            var masterTemplateMock = new Mock<ITemplate>();

            templateMock.Setup(m => m.Alias).Returns("master");

            var fileServiceMock = mocker.GetMock<IFileService>();

            fileServiceMock.Setup(m => m.GetTemplates()).Returns(new List<ITemplate> { templateMock.Object, masterTemplateMock.Object });
            fileServiceMock.Setup(m => m.SaveTemplate(It.IsAny<ITemplate[]>(), 0)).Verifiable();

            var templateSynchronizer = mocker.CreateInstance<TemplateSynchronizer>();

            templateSynchronizer.Run();

            mocker.VerifyAll();
        }
    }
}