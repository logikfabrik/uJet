// <copyright file="TemplateServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Linq;
    using Shouldly;
    using Xunit;

    public class TemplateServiceTest
    {
        [Fact]
        public void CanGetTemplatePaths()
        {
            var service = new TemplateService(AppDomain.CurrentDomain);

            var templates = service.TemplatePaths;

            templates.Count().ShouldBe(2);
        }

        [Fact]
        public void CanGetTemplate()
        {
            var service = new TemplateService(AppDomain.CurrentDomain);

            var templatePath = service.TemplatePaths.First();

            service.GetTemplate(templatePath).ShouldNotBeNull();
        }
    }
}