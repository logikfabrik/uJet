﻿// <copyright file="ModelServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using Moq;
    using Moq.AutoMock;
    using Xunit;

    public class ModelServiceTest
    {
        [Fact]
        public void CanGetDocumentTypes()
        {
            var mocker = new AutoMocker();

            var modelService = mocker.CreateInstance<ModelService>();

            var modelTypeServiceMock = mocker.GetMock<IModelTypeService>();

            // ReSharper disable once UnusedVariable
            var documentTypes = modelService.DocumentTypes;

            modelTypeServiceMock.Verify(m => m.DocumentTypes, Times.Once);
        }

        [Fact]
        public void CanGetMediaTypes()
        {
            var mocker = new AutoMocker();

            var modelService = mocker.CreateInstance<ModelService>();

            var modelTypeServiceMock = mocker.GetMock<IModelTypeService>();

            // ReSharper disable once UnusedVariable
            var mediaTypes = modelService.MediaTypes;

            modelTypeServiceMock.Verify(m => m.MediaTypes, Times.Once);
        }

        [Fact]
        public void CanGetMemberTypes()
        {
            var mocker = new AutoMocker();

            var modelService = mocker.CreateInstance<ModelService>();

            var modelTypeServiceMock = mocker.GetMock<IModelTypeService>();

            // ReSharper disable once UnusedVariable
            var memberTypes = modelService.MemberTypes;

            modelTypeServiceMock.Verify(m => m.MemberTypes, Times.Once);
        }

        [Fact]
        public void CanGetDataTypes()
        {
            var mocker = new AutoMocker();

            var modelService = mocker.CreateInstance<ModelService>();

            var modelTypeServiceMock = mocker.GetMock<IModelTypeService>();

            // ReSharper disable once UnusedVariable
            var dataTypes = modelService.DataTypes;

            modelTypeServiceMock.Verify(m => m.DataTypes, Times.Once);
        }
    }
}
