// <copyright file="TypeResolverTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using Moq;
    using Moq.AutoMock;
    using Xunit;

    public class TypeResolverTest
    {
        [Fact]
        public void CanGetDocumentTypes()
        {
            var mocker = new AutoMocker();

            var typeResolver = mocker.CreateInstance<TypeResolver>();

            var typeServiceMock = mocker.GetMock<ITypeService>();

            var _ = typeResolver.DocumentTypes;

            typeServiceMock.Verify(m => m.DocumentTypes, Times.Once);
        }

        [Fact]
        public void CanGetMediaTypes()
        {
            var mocker = new AutoMocker();

            var typeResolver = mocker.CreateInstance<TypeResolver>();

            var typeServiceMock = mocker.GetMock<ITypeService>();

            var _ = typeResolver.MediaTypes;

            typeServiceMock.Verify(m => m.MediaTypes, Times.Once);
        }

        [Fact]
        public void CanGetMemberTypes()
        {
            var mocker = new AutoMocker();

            var typeResolver = mocker.CreateInstance<TypeResolver>();

            var typeServiceMock = mocker.GetMock<ITypeService>();

            var _ = typeResolver.MemberTypes;

            typeServiceMock.Verify(m => m.MemberTypes, Times.Once);
        }

        [Fact]
        public void CanGetDataTypes()
        {
            var mocker = new AutoMocker();

            var typeResolver = mocker.CreateInstance<TypeResolver>();

            var typeServiceMock = mocker.GetMock<ITypeService>();

            var _ = typeResolver.DataTypes;

            typeServiceMock.Verify(m => m.DataTypes, Times.Once);
        }
    }
}
