// <copyright file="MemberTypeSynchronizationServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MemberTypeSynchronizationServiceTest
    {
        [TestMethod]
        public void CanCreateMemberTypeWithAndWithoutId()
        {
            var memberTypeWithId = new Jet.MemberType(typeof(MemberTypeWithId));
            var memberTypeWithoutId = new Jet.MemberType(typeof(MemberTypeWithoutId));

            var memberTypeWithIdMock = new Mock<IMemberType>();
            var memberTypeWithoutIdMock = new Mock<IMemberType>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MemberTypes).Returns(new[] { memberTypeWithId, memberTypeWithoutId });

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new IMemberType[] { });
            memberTypeServiceMock.Setup(m => m.Get(memberTypeWithId.Alias)).Returns(memberTypeWithIdMock.Object);
            memberTypeServiceMock.Setup(m => m.Get(memberTypeWithoutId.Alias)).Returns(memberTypeWithoutIdMock.Object);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizationService>(
                memberTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Synchronize();

            memberTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(It.IsAny<Jet.MemberType>()), Times.Exactly(2));
        }

        [TestMethod]
        public void CanCreateMemberTypeWithId()
        {
            var memberTypeWithId = new Jet.MemberType(typeof(MemberTypeWithId));

            var memberTypeMock = new Mock<IMemberType>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MemberTypes).Returns(new[] { memberTypeWithId });

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new IMemberType[] { });
            memberTypeServiceMock.Setup(m => m.Get(memberTypeWithId.Alias)).Returns(memberTypeMock.Object);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizationService>(
                memberTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Synchronize();

            memberTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(memberTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanCreateMemberTypeWithoutId()
        {
            var memberTypeWithoutId = new Jet.MemberType(typeof(MemberTypeWithoutId));

            var memberTypeMock = new Mock<IMemberType>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MemberTypes).Returns(new[] { memberTypeWithoutId });

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new IMemberType[] { });
            memberTypeServiceMock.Setup(m => m.Get(memberTypeWithoutId.Alias)).Returns(memberTypeMock.Object);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizationService>(
                memberTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Synchronize();

            memberTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(memberTypeWithoutId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateMemberTypeWithId()
        {
            var memberTypeWithId = new Jet.MemberType(typeof(MemberTypeWithId));

            var memberTypeMock = new Mock<IMemberType>();

            memberTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MemberTypes).Returns(new[] { memberTypeWithId });
            typeResolverMock.Setup(m => m.ResolveType<Jet.MemberType, MemberTypeAttribute, IMemberType>(memberTypeWithId, It.IsAny<IMemberType[]>())).Returns(memberTypeMock.Object);

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new[] { memberTypeMock.Object });
            memberTypeServiceMock.Setup(m => m.Get(memberTypeWithId.Alias)).Returns(memberTypeMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetContentTypeId(memberTypeWithId.Id.Value)).Returns(memberTypeMock.Object.Id);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizationService>(
                memberTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Synchronize();

            memberTypeSynchronizationServiceMock.Verify(m => m.UpdateContentType(memberTypeMock.Object, memberTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateNameForMemberTypeWithId()
        {
            var memberTypeWithId = new Jet.MemberType(typeof(MemberTypeWithId));

            var memberTypeMock = new Mock<IMemberType>();

            memberTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MemberTypes).Returns(new[] { memberTypeWithId });
            typeResolverMock.Setup(m => m.ResolveType<Jet.MemberType, MemberTypeAttribute, IMemberType>(memberTypeWithId, It.IsAny<IMemberType[]>())).Returns(memberTypeMock.Object);

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new[] { memberTypeMock.Object });
            memberTypeServiceMock.Setup(m => m.Get(memberTypeWithId.Alias)).Returns(memberTypeMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetContentTypeId(memberTypeWithId.Id.Value)).Returns(memberTypeMock.Object.Id);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizationService>(
                memberTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Synchronize();

            memberTypeMock.VerifySet(m => m.Name = memberTypeWithId.Name, Times.Once);
        }

        [TestMethod]
        public void CanUpdateMemberTypeWithoutId()
        {
            var memberTypeWithoutId = new Jet.MemberType(typeof(MemberTypeWithoutId));

            var memberTypeMock = new Mock<IMemberType>();

            memberTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MemberTypes).Returns(new[] { memberTypeWithoutId });
            typeResolverMock.Setup(m => m.ResolveType<Jet.MemberType, MemberTypeAttribute, IMemberType>(memberTypeWithoutId, It.IsAny<IMemberType[]>())).Returns(memberTypeMock.Object);

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new[] { memberTypeMock.Object });
            memberTypeServiceMock.Setup(m => m.Get(memberTypeWithoutId.Alias)).Returns(memberTypeMock.Object);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizationService>(
                memberTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Synchronize();

            memberTypeSynchronizationServiceMock.Verify(m => m.UpdateContentType(memberTypeMock.Object, It.IsAny<Jet.MemberType>()), Times.Once);
        }

        [MemberType(
            "65b9653f-d560-41c7-bc7e-71ea9dc70766",
            "MemberTypeWithId")]
        protected class MemberTypeWithId
        {
        }

        [MemberType(
            "MemberTypeWithoutId")]
        protected class MemberTypeWithoutId
        {
        }
    }
}
