// <copyright file="MemberTypeSynchronizationServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using Data;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Jet.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The <see cref="MemberTypeSynchronizationServiceTest" /> class.
    /// </summary>
    [TestClass]
    public class MemberTypeSynchronizationServiceTest
    {
        private const string IdForMemberTypeWithId = "65b9653f-d560-41c7-bc7e-71ea9dc70766";
        private const string NameForMemberTypeWithId = "MemberTypeWithId";
        private const string NameForMemberTypeWithoutId = "MemberTypeWithoutId";

        /// <summary>
        /// Test to create member type with and without ID.
        /// </summary>
        [TestMethod]
        public void CanCreateMemberTypeWithAndWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.MemberTypes).Returns(new[] { typeof(MemberTypeWithId), typeof(MemberTypeWithoutId) });

            var memberTypeWithIdMock = new Mock<IMemberType>();

            var memberTypeWithoutIdMock = new Mock<IMemberType>();

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new IMemberType[] { });
            memberTypeServiceMock.Setup(m => m.Get(NameForMemberTypeWithId.Alias())).Returns(memberTypeWithIdMock.Object);
            memberTypeServiceMock.Setup(m => m.Get(NameForMemberTypeWithoutId.Alias())).Returns(memberTypeWithoutIdMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizationService>(
                memberTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Synchronize();

            memberTypeSynchronizationServiceMock
                .Verify(m => m.CreateMemberType(It.IsAny<Jet.MemberType>()), Times.Exactly(2));
        }

        /// <summary>
        /// Test to create member type with ID.
        /// </summary>
        [TestMethod]
        public void CanCreateMemberTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.MemberTypes).Returns(new[] { typeof(MemberTypeWithId) });

            var memberTypeMock = new Mock<IMemberType>();

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new IMemberType[] { });
            memberTypeServiceMock.Setup(m => m.Get(NameForMemberTypeWithId.Alias())).Returns(memberTypeMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizationService>(
                memberTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Synchronize();

            memberTypeSynchronizationServiceMock
                .Verify(m => m.CreateMemberType(It.IsAny<Jet.MemberType>()), Times.Once);
        }

        /// <summary>
        /// Test to create member type without ID.
        /// </summary>
        [TestMethod]
        public void CanCreateMemberTypeWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.MemberTypes).Returns(new[] { typeof(MemberTypeWithoutId) });

            var memberTypeMock = new Mock<IMemberType>();

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new IMemberType[] { });
            memberTypeServiceMock.Setup(m => m.Get(NameForMemberTypeWithoutId.Alias())).Returns(memberTypeMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizationService>(
                memberTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Synchronize();

            memberTypeSynchronizationServiceMock
                .Verify(m => m.CreateMemberType(It.IsAny<Jet.MemberType>()), Times.Once);
        }

        /// <summary>
        /// Test to update member type with ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateMemberTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.MemberTypes).Returns(new[] { typeof(MemberTypeWithId) });

            var memberTypeMock = new Mock<IMemberType>();

            memberTypeMock.SetupAllProperties();

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new[] { memberTypeMock.Object });
            memberTypeServiceMock.Setup(m => m.Get(NameForMemberTypeWithId.Alias())).Returns(memberTypeMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(Guid.Parse(IdForMemberTypeWithId))).Returns(memberTypeMock.Object.Id);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizationService>(
                memberTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Synchronize();

            memberTypeSynchronizationServiceMock.Verify(m => m.UpdateMemberType(memberTypeMock.Object, It.IsAny<Jet.MemberType>()), Times.Once);
        }

        /// <summary>
        /// Test to update name for member type with ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateNameForMemberTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.MemberTypes).Returns(new[] { typeof(MemberTypeWithId) });

            var memberTypeMock = new Mock<IMemberType>();

            memberTypeMock.SetupAllProperties();

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new[] { memberTypeMock.Object });
            memberTypeServiceMock.Setup(m => m.Get(NameForMemberTypeWithId.Alias())).Returns(memberTypeMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(Guid.Parse(IdForMemberTypeWithId))).Returns(memberTypeMock.Object.Id);

            var memberTypeSynchronizationService = new MemberTypeSynchronizationService(
                memberTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object);

            memberTypeSynchronizationService.Synchronize();

            memberTypeMock.VerifySet(m => m.Name = NameForMemberTypeWithId, Times.Once);
        }

        /// <summary>
        /// Test to update member type without ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateMemberTypeWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.MemberTypes).Returns(new[] { typeof(MemberTypeWithoutId) });

            var memberTypeMock = new Mock<IMemberType>();

            memberTypeMock.Setup(m => m.Alias).Returns(NameForMemberTypeWithoutId.Alias());

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new[] { memberTypeMock.Object });
            memberTypeServiceMock.Setup(m => m.Get(NameForMemberTypeWithoutId.Alias())).Returns(memberTypeMock.Object);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizationService>(
                memberTypeServiceMock.Object,
                new Mock<IContentTypeRepository>().Object,
                typeServiceMock.Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Synchronize();

            memberTypeSynchronizationServiceMock.Verify(m => m.UpdateMemberType(memberTypeMock.Object, It.IsAny<Jet.MemberType>()), Times.Once);
        }

        /// <summary>
        /// The <see cref="MemberTypeWithId" /> class.
        /// </summary>
        [MemberType(
            IdForMemberTypeWithId,
            NameForMemberTypeWithId)]
        protected class MemberTypeWithId
        {
        }

        /// <summary>
        /// The <see cref="MemberTypeWithoutId" /> class.
        /// </summary>
        [MemberType(
            NameForMemberTypeWithoutId)]
        protected class MemberTypeWithoutId
        {
        }
    }
}
