// <copyright file="TemplateServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System.IO;
    using System.Linq;
    using Shouldly;
    using Xunit;

    public class TemplateServiceTest
    {
        [Fact]
        public void CanGetTemplatePaths()
        {
            var service = new TemplateService();

            var templates = service.TemplatePaths;

            templates.Count().ShouldBe(2);
        }

        [Fact]
        public void CanGetContent()
        {
            var service = new TemplateService();

            var templatePath = service.TemplatePaths.First();

            service.GetContent(templatePath).ShouldBe(File.ReadAllText(templatePath));
        }

        [Fact]
        public void CanGetTemplate()
        {
            var service = new TemplateService();

            var templatePath = service.TemplatePaths.First();

            service.GetTemplate(templatePath).ShouldNotBeNull();
        }
    }
}