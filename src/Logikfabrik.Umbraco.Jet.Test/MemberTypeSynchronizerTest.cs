// <copyright file="MemberTypeSynchronizerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Jet.Data;
    using Moq;
    using Moq.AutoMock;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class MemberTypeSynchronizerTest
    {
        [Theory]
        [CustomAutoData]
        public void CanCreateModelWithoutId(Jet.MemberType model)
        {
            var mocker = new AutoMocker();

            var memberTypeSynchronizer = mocker.CreateInstance<MemberTypeSynchronizer>();

            var modelServiceMock = mocker.GetMock<IModelService>();

            modelServiceMock
                .Setup(m => m.MemberTypes)
                .Returns(new[] { model });

            var memberTypes = new List<IMemberType>();

            var memberTypeServiceMock = mocker.GetMock<IMemberTypeService>();

            memberTypeServiceMock
                .Setup(m => m.GetAll())
                .Returns(memberTypes);

            memberTypeServiceMock
                .Setup(m => m.Save(It.Is<IMemberType>(memberType => memberType.Id == 0), 0))
                .Callback((IMemberType memberType, int userId) => { memberTypes.Add(memberType); })
                .Verifiable();

            memberTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [CustomAutoData]
        public void CanCreateModelWithId(string typeName, Guid id, string name)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, id, name).Create(Scope.Public);

            var model = new Jet.MemberType(modelType);

            var mocker = new AutoMocker();

            var memberTypeSynchronizer = mocker.CreateInstance<MemberTypeSynchronizer>();

            var modelServiceMock = mocker.GetMock<IModelService>();

            modelServiceMock
                .Setup(m => m.MemberTypes)
                .Returns(new[] { model });

            var memberTypes = new List<IMemberType>();

            var memberTypeServiceMock = mocker.GetMock<IMemberTypeService>();

            memberTypeServiceMock
                .Setup(m => m.GetAll())
                .Returns(memberTypes);

            memberTypeServiceMock
                .Setup(m => m.Save(It.Is<IMemberType>(memberType => memberType.Id == 0), 0))
                .Callback((IMemberType memberType, int userId) => { memberTypes.Add(memberType); })
                .Verifiable();

            var typeRepositoryMock = mocker.GetMock<ITypeRepository>();

            typeRepositoryMock.Setup(m => m.SetContentTypeId(id, It.IsAny<int>())).Verifiable();

            memberTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [CustomAutoData]
        public void CanUpdateModelWithoutId(Jet.MemberType model)
        {
            var mocker = new AutoMocker();

            mocker.Use<IContentTypeFinder<Jet.MemberType, MemberTypeAttribute, IMemberType>>(mocker.CreateInstance<ContentTypeFinder<Jet.MemberType, MemberTypeAttribute, IMemberType>>());

            var memberTypeSynchronizer = mocker.CreateInstance<MemberTypeSynchronizer>();

            var modelServiceMock = mocker.GetMock<IModelService>();

            modelServiceMock
                .Setup(m => m.MemberTypes)
                .Returns(new[] { model });

            var memberTypeMock = mocker.GetMock<IMemberType>();

            memberTypeMock.Setup(m => m.Alias).Returns(model.Alias);

            var memberTypeServiceMock = mocker.GetMock<IMemberTypeService>();

            memberTypeServiceMock
                .Setup(m => m.GetAll())
                .Returns(new[] { memberTypeMock.Object });

            memberTypeServiceMock.Setup(m => m.Save(memberTypeMock.Object, 0)).Verifiable();

            memberTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [CustomAutoData]
        public void CanUpdateModelWithId(string typeName, Guid id, string name)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, id, name).Create(Scope.Public);

            var model = new Jet.MemberType(modelType);

            var mocker = new AutoMocker();

            mocker.Use<IContentTypeFinder<Jet.MemberType, MemberTypeAttribute, IMemberType>>(mocker.CreateInstance<ContentTypeFinder<Jet.MemberType, MemberTypeAttribute, IMemberType>>());

            var memberTypeSynchronizer = mocker.CreateInstance<MemberTypeSynchronizer>();

            var modelServiceMock = mocker.GetMock<IModelService>();

            modelServiceMock
                .Setup(m => m.MemberTypes)
                .Returns(new[] { model });

            var memberTypeMock = mocker.GetMock<IMemberType>();

            memberTypeMock.Setup(m => m.Alias).Returns(model.Alias);

            var memberTypeServiceMock = mocker.GetMock<IMemberTypeService>();

            memberTypeServiceMock
                .Setup(m => m.GetAll())
                .Returns(new[] { memberTypeMock.Object });

            memberTypeServiceMock.Setup(m => m.Save(memberTypeMock.Object, 0)).Verifiable();

            var typeRepositoryMock = mocker.GetMock<ITypeRepository>();

            typeRepositoryMock.Setup(m => m.SetContentTypeId(id, memberTypeMock.Object.Id)).Verifiable();

            memberTypeSynchronizer.Run();

            mocker.VerifyAll();
        }
    }
}