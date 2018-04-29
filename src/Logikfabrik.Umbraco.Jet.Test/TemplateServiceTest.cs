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
            var templates = TemplateService.Instance.TemplatePaths;

            templates.Count().ShouldBe(2);
        }

        [Fact]
        public void CanGetContent()
        {
            var templatePath = TemplateService.Instance.TemplatePaths.First();

            TemplateService.Instance.GetContent(templatePath).ShouldBe(File.ReadAllText(templatePath));
        }

        [Fact]
        public void CanGetTemplate()
        {
            var templatePath = TemplateService.Instance.TemplatePaths.First();

            TemplateService.Instance.GetTemplate(templatePath).ShouldNotBeNull();
        }
    }
}