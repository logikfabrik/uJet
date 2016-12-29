// <copyright file="MemberTypeSynchronizerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MemberTypeSynchronizerTest
    {
        [TestMethod]
        public void CanCreateMemberTypeWithAndWithoutId()
        {
            var logServiceMock = new Mock<ILogService>();

            var memberTypeWithId = new Jet.MemberType(typeof(MemberTypeWithId));
            var memberTypeWithoutId = new Jet.MemberType(typeof(MemberTypeWithoutId));

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MemberTypes).Returns(Array.AsReadOnly(new[] { memberTypeWithId, memberTypeWithoutId }));

            var memberTypes = new List<IMemberType>();

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(memberTypes);
            memberTypeServiceMock.Setup(m => m.Save(It.IsAny<IMemberType>(), 0)).Callback((IMemberType memberType, int userId) => { memberTypes.Add(memberType); });

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizer>(
                logServiceMock.Object,
                memberTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Run();

            memberTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(It.IsAny<Jet.MemberType>()), Times.Exactly(2));
        }

        [TestMethod]
        public void CanCreateMemberTypeWithId()
        {
            var logServiceMock = new Mock<ILogService>();

            var memberTypeWithId = new Jet.MemberType(typeof(MemberTypeWithId));

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MemberTypes).Returns(Array.AsReadOnly(new[] { memberTypeWithId }));

            var memberTypes = new List<IMemberType>();

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(memberTypes);
            memberTypeServiceMock.Setup(m => m.Save(It.IsAny<IMemberType>(), 0)).Callback((IMemberType memberType, int userId) => { memberTypes.Add(memberType); });

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizer>(
                logServiceMock.Object,
                memberTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Run();

            memberTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(memberTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanCreateMemberTypeWithoutId()
        {
            var logServiceMock = new Mock<ILogService>();

            var memberTypeWithoutId = new Jet.MemberType(typeof(MemberTypeWithoutId));

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MemberTypes).Returns(Array.AsReadOnly(new[] { memberTypeWithoutId }));

            var memberTypes = new List<IMemberType>();

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(memberTypes);
            memberTypeServiceMock.Setup(m => m.Save(It.IsAny<IMemberType>(), 0)).Callback((IMemberType memberType, int userId) => { memberTypes.Add(memberType); });

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizer>(
                logServiceMock.Object,
                memberTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Run();

            memberTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(memberTypeWithoutId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateMemberTypeWithId()
        {
            var logServiceMock = new Mock<ILogService>();

            var memberTypeWithId = new Jet.MemberType(typeof(MemberTypeWithId));

            var memberTypeMock = new Mock<IMemberType>();

            memberTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MemberTypes).Returns(Array.AsReadOnly(new[] { memberTypeWithId }));

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new[] { memberTypeMock.Object });

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetContentTypeId(memberTypeWithId.Id.Value)).Returns(memberTypeMock.Object.Id);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizer>(
                logServiceMock.Object,
                memberTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Run();

            memberTypeSynchronizationServiceMock.Verify(m => m.UpdateContentType(memberTypeMock.Object, memberTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateNameForMemberTypeWithId()
        {
            var logServiceMock = new Mock<ILogService>();

            var memberTypeWithId = new Jet.MemberType(typeof(MemberTypeWithId));

            var memberTypeMock = new Mock<IMemberType>();

            memberTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MemberTypes).Returns(Array.AsReadOnly(new[] { memberTypeWithId }));

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new[] { memberTypeMock.Object });

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetContentTypeId(memberTypeWithId.Id.Value)).Returns(memberTypeMock.Object.Id);

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizer>(
                logServiceMock.Object,
                memberTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Run();

            memberTypeMock.VerifySet(m => m.Name = memberTypeWithId.Name, Times.Once);
        }

        [TestMethod]
        public void CanUpdateMemberTypeWithoutId()
        {
            var logServiceMock = new Mock<ILogService>();

            var memberTypeWithoutId = new Jet.MemberType(typeof(MemberTypeWithoutId));

            var memberTypeMock = new Mock<IMemberType>();

            memberTypeMock.SetupAllProperties();
            memberTypeMock.Object.Alias = memberTypeWithoutId.Alias;

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MemberTypes).Returns(Array.AsReadOnly(new[] { memberTypeWithoutId }));

            var memberTypeServiceMock = new Mock<IMemberTypeService>();

            memberTypeServiceMock.Setup(m => m.GetAll()).Returns(new[] { memberTypeMock.Object });

            var memberTypeSynchronizationServiceMock = new Mock<MemberTypeSynchronizer>(
                logServiceMock.Object,
                memberTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            memberTypeSynchronizationServiceMock.Object.Run();

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