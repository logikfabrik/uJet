namespace Logikfabrik.Umbraco.Jet.Test.Web.Mvc
{
    using System.Web.Mvc;
    using global::Umbraco.Web.Models;
    using Logikfabrik.Umbraco.Jet.Web.Mvc;
    using Moq;
    using Shouldly;
    using Xunit;

    public class ControllerContextExtensionsTest
    {
        [Fact]
        public void CanGetRenderModel()
        {
            var controllerContext = new ControllerContext();

            controllerContext.RouteData.DataTokens.Add(RouteDataTokenKey.Key, Mock.Of<IRenderModel>());

            var renderModel = controllerContext.GetRenderModel();

            renderModel.ShouldNotBeNull();
        }
    }
}
