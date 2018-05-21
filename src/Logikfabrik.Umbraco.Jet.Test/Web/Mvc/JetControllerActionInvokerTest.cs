// <copyright file="JetControllerActionInvokerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Mvc
{
    using Jet.Web.Mvc;
    using Shouldly;
    using Xunit;

    public class JetControllerActionInvokerTest
    {
        [Fact]
        public void CanGetActionNameForPreviewAction()
        {
            JetControllerActionInvoker.GetActionName(PreviewTemplateAttribute.TemplateName.Alias()).ShouldBe("Index");
        }

        [Fact]
        public void CanGetActionNameForIndexAction()
        {
            JetControllerActionInvoker.GetActionName("Index").ShouldBe("Index");
        }
    }
}
