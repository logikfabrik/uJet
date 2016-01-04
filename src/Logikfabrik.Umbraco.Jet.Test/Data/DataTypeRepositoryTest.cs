// <copyright file="DataTypeRepositoryTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Data
{
    using System;
    using Jet.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DataTypeRepositoryTest
    {
        [TestMethod]
        public void CanGetDefinitionModelId()
        {
            var modelId = Guid.Parse("22f44f53-81de-4c2e-a9b9-186ea053d234");

            var dataTypeRepositoryMock = new Mock<DataTypeRepository>(new Mock<IDatabaseWrapper>().Object)
            {
                CallBase = true
            };

            dataTypeRepositoryMock.Setup(m => m.GetDefinitionByDefinitionId(5)).Returns(new DataType { DefinitionId = 5, Id = modelId });

            Assert.AreEqual(modelId, dataTypeRepositoryMock.Object.GetDefinitionModelId(5));
        }

        [TestMethod]
        public void CanGetDefinitionModelIdFromCache()
        {
            var modelId = Guid.Parse("22f44f53-81de-4c2e-a9b9-186ea053d234");

            var dataTypeRepositoryMock = new Mock<DataTypeRepository>(new Mock<IDatabaseWrapper>().Object)
            {
                CallBase = true
            };

            dataTypeRepositoryMock.Setup(m => m.GetDefinitionByDefinitionId(5)).Returns(new DataType { DefinitionId = 5, Id = modelId });

            dataTypeRepositoryMock.Object.GetDefinitionModelId(5);
            dataTypeRepositoryMock.Object.GetDefinitionModelId(5);

            dataTypeRepositoryMock.Verify(m => m.GetDefinitionByDefinitionId(5), Times.Once);
        }

        [TestMethod]
        public void CanGetDefinitionId()
        {
            var modelId = Guid.Parse("22f44f53-81de-4c2e-a9b9-186ea053d234");

            var dataTypeRepositoryMock = new Mock<DataTypeRepository>(new Mock<IDatabaseWrapper>().Object)
            {
                CallBase = true
            };

            dataTypeRepositoryMock.Setup(m => m.GetDefinitionById(modelId)).Returns(new DataType { DefinitionId = 5, Id = modelId });

            Assert.AreEqual(5, dataTypeRepositoryMock.Object.GetDefinitionId(modelId));
        }

        [TestMethod]
        public void CanGetDefinitionIdFromCache()
        {
            var modelId = Guid.Parse("22f44f53-81de-4c2e-a9b9-186ea053d234");

            var dataTypeRepositoryMock = new Mock<DataTypeRepository>(new Mock<IDatabaseWrapper>().Object)
            {
                CallBase = true
            };

            dataTypeRepositoryMock.Setup(m => m.GetDefinitionById(modelId)).Returns(new DataType { DefinitionId = 5, Id = modelId });

            dataTypeRepositoryMock.Object.GetDefinitionId(modelId);
            dataTypeRepositoryMock.Object.GetDefinitionId(modelId);

            dataTypeRepositoryMock.Verify(m => m.GetDefinitionById(modelId), Times.Once);
        }
    }
}