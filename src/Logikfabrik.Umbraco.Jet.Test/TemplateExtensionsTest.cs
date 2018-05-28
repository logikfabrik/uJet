// <copyright file="TemplateExtensionsTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using global::Umbraco.Core.Models;
    using Moq;
    using Shouldly;
    using Xunit;

    public class TemplateExtensionsTest
    {
        [Fact]
        public void CanGetLayout()
        {
            var templateMock = new Mock<ITemplate>();

            templateMock.Setup(m => m.Content).Returns("@{Layout=\"master.cshtml\";}");

            templateMock.Object.GetLayout().ShouldBe("master");
        }
    }
}
