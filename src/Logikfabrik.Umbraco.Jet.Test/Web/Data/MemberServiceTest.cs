// <copyright file="MemberServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

using AutoFixture.Xunit2;

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data
{
    using System;
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;
    using Jet.Web.Data;
    using Moq;
    using Shouldly;
    using Utilities;
    using Xunit;

    public class MemberServiceTest : TestBase
    {
        [Fact]
        public void CanGetMemberIdByConvention()
        {
            const int id = 123;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Id).Returns(id);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMember(id)).Returns(publishedContentMock.Object);

            var member = new MemberService(umbracoHelperWrapperMock.Object).GetMember<Models.MemberType>(id);

            member.Id.ShouldBe(id);
        }

        [Fact]
        public void CanGetMemberUrlByConvention()
        {
            const int id = 123;
            const string url = "/umbraco/jet";

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Url).Returns(url);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMember(id)).Returns(publishedContentMock.Object);

            var member = new MemberService(umbracoHelperWrapperMock.Object).GetMember<Models.MemberType>(id);

            member.Url.ShouldBe(url);
        }

        [Fact]
        public void CanGetMemberNameByConvention()
        {
            const int id = 123;
            const string name = "Umbraco Jet";

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Name).Returns(name);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMember(id)).Returns(publishedContentMock.Object);

            var member = new MemberService(umbracoHelperWrapperMock.Object).GetMember<Models.MemberType>(id);

            member.Name.ShouldBe(name);
        }

        [Fact]
        public void CanGetMemberCreateDateByConvention()
        {
            const int id = 123;
            var createDate = new DateTime(2015, 1, 1);

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.CreateDate).Returns(createDate);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMember(id)).Returns(publishedContentMock.Object);

            var member = new MemberService(umbracoHelperWrapperMock.Object).GetMember<Models.MemberType>(id);

            member.CreateDate.ShouldBe(createDate);
        }

        [Fact]
        public void CanGetMemberUpdateDateByConvention()
        {
            const int id = 123;
            var updateDate = new DateTime(2015, 1, 1);

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.UpdateDate).Returns(updateDate);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMember(id)).Returns(publishedContentMock.Object);

            var member = new MemberService(umbracoHelperWrapperMock.Object).GetMember<Models.MemberType>(id);

            member.UpdateDate.ShouldBe(updateDate);
        }

        [Fact]
        public void CanNotGetMemberForInvalidMemberType()
        {
            var type = TypeUtility.GetTypeBuilder("MyType", TypeUtility.GetTypeAttributes()).CreateType();

            var contentMock = new Mock<IPublishedContent>();

            var service = new MemberService(new Mock<IUmbracoHelperWrapper>().Object);

            Assert.Throws<ArgumentException>(() => service.GetMember(contentMock.Object, type));
        }

        [Theory]
        [AutoData]
        public void CanGetMemberForValidMemberType(string typeName, string name)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, name).CreateType();

            var contentMock = new Mock<IPublishedContent>();

            contentMock.Setup(m => m.Properties).Returns(new List<IPublishedProperty>());

            var service = new MemberService(new Mock<IUmbracoHelperWrapper>().Object);

            var member = service.GetMember(contentMock.Object, modelType);

            member.ShouldNotBeNull();
        }

        [Fact]
        public void CanGetMemberForMemberTypeWithStringProperty()
        {
            const int id = 123;
            const string stringPropertyName = "stringProperty";
            const string stringPropertyValue = "StringProperty";

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = new Mock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(stringPropertyName);
                property.Setup(m => m.Value).Returns(stringPropertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMember(id)).Returns(publishedContentMock.Object);

            var member = new MemberService(umbracoHelperWrapperMock.Object).GetMember<Models.MemberType>(id);

            member.StringProperty.ShouldBe(stringPropertyValue);
        }

        [Fact]
        public void CanGetMemberForMemberTypeWithIntegerProperty()
        {
            const int id = 123;
            const string integerPropertyName = "integerProperty";
            const int integerPropertyValue = 7;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = new Mock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(integerPropertyName);
                property.Setup(m => m.Value).Returns(integerPropertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMember(id)).Returns(publishedContentMock.Object);

            var member = new MemberService(umbracoHelperWrapperMock.Object).GetMember<Models.MemberType>(id);

            member.IntegerProperty.ShouldBe(integerPropertyValue);
        }

        [Fact]
        public void CanGetMemberForMemberTypeWithFloatingBinaryPointProperty()
        {
            const int id = 123;
            const string floatingBinaryPointPropertyName = "FloatingBinaryPointProperty";
            const float floatingBinaryPointPropertyValue = 2.2f;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = new Mock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(floatingBinaryPointPropertyName);
                property.Setup(m => m.Value).Returns(floatingBinaryPointPropertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMember(id)).Returns(publishedContentMock.Object);

            var member = new MemberService(umbracoHelperWrapperMock.Object).GetMember<Models.MemberType>(id);

            member.FloatingBinaryPointProperty.ShouldBe(floatingBinaryPointPropertyValue);
        }

        [Fact]
        public void CanGetMemberForMemberTypeWithFloatingDecimalPointProperty()
        {
            const int id = 123;
            const string floatingDecimalPointPropertyName = "FloatingDecimalPointProperty";
            const decimal floatingDecimalPointPropertyValue = 2.2m;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = new Mock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(floatingDecimalPointPropertyName);
                property.Setup(m => m.Value).Returns(floatingDecimalPointPropertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMember(id)).Returns(publishedContentMock.Object);

            var member = new MemberService(umbracoHelperWrapperMock.Object).GetMember<Models.MemberType>(id);

            member.FloatingDecimalPointProperty.ShouldBe(floatingDecimalPointPropertyValue);
        }

        [Fact]
        public void CanGetMemberForMemberTypeWithBooleanProperty()
        {
            const int id = 123;
            const string booleanPropertyName = "BooleanProperty";
            const bool booleanPropertyValue = true;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = new Mock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(booleanPropertyName);
                property.Setup(m => m.Value).Returns(booleanPropertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMember(id)).Returns(publishedContentMock.Object);

            var member = new MemberService(umbracoHelperWrapperMock.Object).GetMember<Models.MemberType>(id);

            member.BooleanProperty.ShouldBe(booleanPropertyValue);
        }

        [Fact]
        public void CanGetMemberForMemberTypeWithDateTimeProperty()
        {
            const int id = 123;
            const string dateTimePropertyName = "DateTimeProperty";
            var dateTimePropertyValue = DateTime.Now;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = new Mock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(dateTimePropertyName);
                property.Setup(m => m.Value).Returns(dateTimePropertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMember(id)).Returns(publishedContentMock.Object);

            var member = new MemberService(umbracoHelperWrapperMock.Object).GetMember<Models.MemberType>(id);

            member.DateTimeProperty.ShouldBe(dateTimePropertyValue);
        }
    }
}