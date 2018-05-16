// <copyright file="JetModelBinderTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Mvc
{
    using System;
    using System.Web.Mvc;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Web.Models;
    using Logikfabrik.Umbraco.Jet.Web.Data;
    using Logikfabrik.Umbraco.Jet.Web.Mvc;
    using Moq;
    using Shouldly;
    using SpecimenBuilders;
    using Xunit;

    public class JetModelBinderTest
    {
        [Theory]
        [CustomAutoData]
        public void CanBindModel(int contentId, Type containerType, Type modelType)
        {
            var contentMock = new Mock<IPublishedContent>();

            contentMock.Setup(m => m.Id).Returns(contentId);

            var renderModelMock = new Mock<IRenderModel>();

            renderModelMock.Setup(m => m.Content).Returns(contentMock.Object);

            var controllerContext = new ControllerContext();

            controllerContext.RouteData.DataTokens.Add(RouteDataTokenKey.Key, renderModelMock.Object);

            var bindingContext = new ModelBindingContext { ModelMetadata = new ModelMetadata(Mock.Of<ModelMetadataProvider>(), containerType, null, modelType, null) };

            var documentServiceMock = new Mock<IContentService>();

            documentServiceMock.Setup(m => m.GetContent(contentMock.Object, modelType)).Returns(new object());

            var modelBinder = new JetModelBinder(documentServiceMock.Object, new[] { modelType });

            var model = modelBinder.BindModel(controllerContext, bindingContext);

            model.ShouldNotBeNull();
        }
    }
}