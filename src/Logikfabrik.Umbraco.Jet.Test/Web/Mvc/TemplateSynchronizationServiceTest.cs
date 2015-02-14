// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.IO;
using System.Linq;
using Logikfabrik.Umbraco.Jet.Extensions;
using Logikfabrik.Umbraco.Jet.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Logikfabrik.Umbraco.Jet.Test.Web.Mvc
{
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

            Assert.AreEqual(1, templatesToAdd1.Count());
            Assert.AreEqual(1, templatesToAdd2.Count());

            var templateToAdd1 = templatesToAdd1.First();
            var templateToAdd2 = templatesToAdd2.First();

            Assert.AreEqual(templateToAdd1, templateToAdd2.Path);
            Assert.AreEqual("Template2", templateToAdd2.Name);
            Assert.AreEqual("template2", templateToAdd2.Alias);
            Assert.AreEqual("Test template Template2.cshtml", templateToAdd2.Content);
        }
    }
}
