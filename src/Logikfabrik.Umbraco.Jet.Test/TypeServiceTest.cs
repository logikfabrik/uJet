// <copyright file="TypeServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Linq;
    using System.Reflection;
    using AutoFixture.Xunit2;
    using Moq.AutoMock;
    using Shouldly;
    using Utilities;
    using Xunit;

    public class TypeServiceTest
    {
        [Theory]
        [AutoData]
        public void CanGetDocumentTypes(string typeName, string name)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name).GetTypeBuilder().CreateType();

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.DocumentTypes.Count().ShouldBe(1);
        }

        [Theory]
        [AutoData]
        public void CanNotGetAbstractDocumentTypes(string typeName, string name)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name).CreateType(TypeAttributes.Abstract);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.DocumentTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [AutoData]
        public void CanNotGetDocumentTypesWithoutPublicDefaultConstructor(string typeName, string name)
        {
            var typeBuilder = new DocumentTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            var modelType = typeBuilder.CreateType();

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.DocumentTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [AutoData]
        public void CanGetMediaTypes(string typeName, string name)
        {
            var modelType = new MediaTypeModelTypeBuilder(typeName, name).GetTypeBuilder().CreateType();

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.MediaTypes.Count().ShouldBe(1);
        }

        [Theory]
        [AutoData]
        public void CanNotGetAbstractMediaTypes(string typeName, string name)
        {
            var modelType = new MediaTypeModelTypeBuilder(typeName, name).CreateType(TypeAttributes.Abstract);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.MediaTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [AutoData]
        public void CanNotGetMediaTypesWithoutPublicDefaultConstructor(string typeName, string name)
        {
            var typeBuilder = new MediaTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            var modelType = typeBuilder.CreateType();

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.MediaTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [AutoData]
        public void CanGetMemberTypes(string typeName, string name)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, name).CreateType();

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.MemberTypes.Count().ShouldBe(1);
        }

        [Theory]
        [AutoData]
        public void CanNotGetAbstractMemberTypes(string typeName, string name)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, name).CreateType(TypeAttributes.Abstract);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.MemberTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [AutoData]
        public void CanNotGetMemberTypesWithoutPublicDefaultConstructor(string typeName, string name)
        {
            var typeBuilder = new MemberTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            var modelType = typeBuilder.CreateType();

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.MemberTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [AutoData]
        public void CanGetDataTypes(string typeName, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, type, editor).CreateType();

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.DataTypes.Count().ShouldBe(1);
        }

        [Theory]
        [AutoData]
        public void CanNotGetAbstractDataTypes(string typeName, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, type, editor).CreateType(TypeAttributes.Abstract);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.DataTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [AutoData]
        public void CanNotGetDataTypesWithoutPublicDefaultConstructor(string typeName, Type type, string editor)
        {
            var typeBuilder = new DataTypeModelTypeBuilder(typeName, type, editor).GetTypeBuilder();

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            var modelType = typeBuilder.CreateType();

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.DataTypes.Any().ShouldBeFalse();
        }
    }
}