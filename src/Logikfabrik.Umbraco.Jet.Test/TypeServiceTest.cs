// <copyright file="TypeServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System.Linq;
    using Moq.AutoMock;
    using Shouldly;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class TypeServiceTest
    {
        [Theory]
        [CustomAutoData]
        public void CanGetDocumentTypes(DocumentTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Public);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.DocumentTypes.Count().ShouldBe(1);
        }

        [Theory]
        [CustomAutoData]
        public void CanNotGetAbstractDocumentTypes(DocumentTypeModelTypeBuilder builder)
        {
            builder.IsAbstractType = true;

            var modelType = builder.Create(Scope.Public);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.DocumentTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void CanNotGetDocumentTypesWithoutPublicDefaultConstructor(DocumentTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Private);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.DocumentTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void CanGetMediaTypes(MediaTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Public);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.MediaTypes.Count().ShouldBe(1);
        }

        [Theory]
        [CustomAutoData]
        public void CanNotGetAbstractMediaTypes(MediaTypeModelTypeBuilder builder)
        {
            builder.IsAbstractType = true;

            var modelType = builder.Create(Scope.Public);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.MediaTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void CanNotGetMediaTypesWithoutPublicDefaultConstructor(MediaTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Private);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.MediaTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void CanGetMemberTypes(MemberTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Public);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.MemberTypes.Count().ShouldBe(1);
        }

        [Theory]
        [CustomAutoData]
        public void CanNotGetAbstractMemberTypes(MemberTypeModelTypeBuilder builder)
        {
            builder.IsAbstractType = true;

            var modelType = builder.Create(Scope.Public);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.MemberTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void CanNotGetMemberTypesWithoutPublicDefaultConstructor(MemberTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Private);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.MemberTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void CanGetDataTypes(DataTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Public);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.DataTypes.Count().ShouldBe(1);
        }

        [Theory]
        [CustomAutoData]
        public void CanNotGetAbstractDataTypes(DataTypeModelTypeBuilder builder)
        {
            builder.IsAbstractType = true;

            var modelType = builder.Create(Scope.Public);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.DataTypes.Any().ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void CanNotGetDataTypesWithoutPublicDefaultConstructor(DataTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Private);

            var mocker = new AutoMocker();

            var assemblyLoader = mocker.GetMock<IAssemblyLoader>();

            assemblyLoader.Setup(m => m.GetAssemblies()).Returns(new[] { modelType.Assembly });

            var typeService = mocker.CreateInstance<TypeService>();

            typeService.DataTypes.Any().ShouldBeFalse();
        }
    }
}