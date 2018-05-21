// <copyright file="DefaultValueServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using AutoFixture;
    using global::Umbraco.Core.Models;
    using Logikfabrik.Umbraco.Jet.Extensions;
    using Moq;
    using Moq.AutoMock;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class DefaultValueServiceTest
    {
        [Theory]
        [ClassAutoData(typeof(CanSetPropertyDefaultValueClassData))]
        public void CanSetPropertyDefaultValueForDocumentTypes(Type propertyType, string propertyDefaultValue, string propertyName, DocumentTypeModelTypeBuilder[] builders)
        {
            var mocker = new AutoMocker();

            var documentTypeModelFinderMock = mocker.GetMock<IContentTypeModelFinder<DocumentType, DocumentTypeAttribute, IContentType>>();

            var contentMocks = new Mock<IContent>[builders.Length];

            for (var i = 0; i < builders.Length; i++)
            {
                builders[i].AddProperty(propertyName, propertyType, (object)propertyDefaultValue);

                var modelType = builders[i].Create(Scope.Public);

                var model = new DocumentType(modelType);

                var contentType = mocker.Get<IContentType>();

                var contentMock = mocker.GetMock<IContent>();

                contentMock.Setup(m => m.ContentType).Returns(contentType);
                contentMock.Setup(m => m.SetValue(propertyName.Alias(), It.Is<object>(value => value.ToString() == new DefaultValueAttribute(propertyType, propertyDefaultValue).Value.ToString()))).Verifiable();

                contentMocks[i] = contentMock;

                documentTypeModelFinderMock.Setup(m => m.Find(contentType, It.IsAny<DocumentType[]>())).Returns(new[] { model });
            }

            var defaultValueService = mocker.CreateInstance<DefaultValueService>();

            defaultValueService.SetDefaultValues(contentMocks.Select(m => m.Object));

            mocker.VerifyAll();
        }

        [Theory]
        [ClassAutoData(typeof(CanSetPropertyDefaultValueClassData))]
        public void CanSetPropertyDefaultValueForMediaTypes(Type propertyType, string propertyDefaultValue, string propertyName, MediaTypeModelTypeBuilder[] builders)
        {
            var mocker = new AutoMocker();

            var mediaTypeModelFinderMock = mocker.GetMock<IContentTypeModelFinder<Jet.MediaType, MediaTypeAttribute, IMediaType>>();

            var contentMocks = new Mock<IMedia>[builders.Length];

            for (var i = 0; i < builders.Length; i++)
            {
                builders[i].AddProperty(propertyName, propertyType, (object)propertyDefaultValue);

                var modelType = builders[i].Create(Scope.Public);

                var model = new Jet.MediaType(modelType);

                var contentType = mocker.Get<IMediaType>();

                var contentMock = mocker.GetMock<IMedia>();

                contentMock.Setup(m => m.ContentType).Returns(contentType);
                contentMock.Setup(m => m.SetValue(propertyName.Alias(), It.Is<object>(value => value.ToString() == new DefaultValueAttribute(propertyType, propertyDefaultValue).Value.ToString()))).Verifiable();

                contentMocks[i] = contentMock;

                mediaTypeModelFinderMock.Setup(m => m.Find(contentType, It.IsAny<Jet.MediaType[]>())).Returns(new[] { model });
            }

            var defaultValueService = mocker.CreateInstance<DefaultValueService>();

            defaultValueService.SetDefaultValues(contentMocks.Select(m => m.Object));

            mocker.VerifyAll();
        }

        [Theory]
        [ClassAutoData(typeof(CanSetPropertyDefaultValueClassData))]
        public void CanSetPropertyDefaultValueForMemberTypes(Type propertyType, string propertyDefaultValue, string propertyName, MemberTypeModelTypeBuilder[] builders)
        {
            var mocker = new AutoMocker();

            var memberTypeModelFinderMock = mocker.GetMock<IContentTypeModelFinder<Jet.MemberType, MemberTypeAttribute, IMemberType>>();

            var contentMocks = new Mock<IMember>[builders.Length];

            for (var i = 0; i < builders.Length; i++)
            {
                builders[i].AddProperty(propertyName, propertyType, (object)propertyDefaultValue);

                var modelType = builders[i].Create(Scope.Public);

                var model = new Jet.MemberType(modelType);

                var contentType = mocker.Get<IMemberType>();

                var contentMock = mocker.GetMock<IMember>();

                contentMock.Setup(m => m.ContentType).Returns(contentType);
                contentMock.Setup(m => m.SetValue(propertyName.Alias(), It.Is<object>(value => value.ToString() == new DefaultValueAttribute(propertyType, propertyDefaultValue).Value.ToString()))).Verifiable();

                contentMocks[i] = contentMock;

                memberTypeModelFinderMock.Setup(m => m.Find(contentType, It.IsAny<Jet.MemberType[]>())).Returns(new[] { model });
            }

            var defaultValueService = mocker.CreateInstance<DefaultValueService>();

            defaultValueService.SetDefaultValues(contentMocks.Select(m => m.Object));

            mocker.VerifyAll();
        }

        private class CanSetPropertyDefaultValueClassData : IEnumerable<object[]>
        {
            private readonly IFixture _fixture = new Fixture();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { typeof(string), _fixture.Create<string>() };
                yield return new object[] { typeof(int), _fixture.Create<int>().ToString() };
                yield return new object[] { typeof(float), _fixture.Create<float>().ToString(CultureInfo.CurrentCulture) };
                yield return new object[] { typeof(decimal), _fixture.Create<decimal>().ToString(CultureInfo.CurrentCulture) };
                yield return new object[] { typeof(bool), _fixture.Create<bool>().ToString() };
                yield return new object[] { typeof(DateTime), _fixture.Create<DateTime>().ToString(CultureInfo.CurrentCulture) };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}