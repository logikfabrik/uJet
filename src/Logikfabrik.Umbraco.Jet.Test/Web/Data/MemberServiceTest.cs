// <copyright file="MemberServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data
{
    using System;
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;
    using Jet.Web.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Utilities;

    [TestClass]
    public class MemberServiceTest : TestBase
    {
        [TestMethod]
        public void CanGetMemberIdByConvention()
        {
            const int id = 123;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Id).Returns(id);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMember(id)).Returns(publishedContentMock.Object);

            var member = new MemberService(umbracoHelperWrapperMock.Object).GetMember<Models.MemberType>(id);

            Assert.AreEqual(id, member.Id);
        }

        [TestMethod]
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

            Assert.AreEqual(url, member.Url);
        }

        [TestMethod]
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

            Assert.AreEqual(name, member.Name);
        }

        [TestMethod]
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

            Assert.AreEqual(createDate, member.CreateDate);
        }

        [TestMethod]
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

            Assert.AreEqual(updateDate, member.UpdateDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotGetMemberForInvalidMemberType()
        {
            var type = TypeUtility.GetTypeBuilder("MyType", TypeUtility.GetTypeAttributes()).CreateType();

            var contentMock = new Mock<IPublishedContent>();

            var service = new MemberService(new Mock<IUmbracoHelperWrapper>().Object);

            service.GetMember(contentMock.Object, type);
        }

        [TestMethod]
        public void CanGetMemberForValidMemberType()
        {
            var type = MemberTypeUtility.GetTypeBuilder().CreateType();

            var contentMock = new Mock<IPublishedContent>();

            contentMock.Setup(m => m.Properties).Returns(new List<IPublishedProperty>());

            var service = new MemberService(new Mock<IUmbracoHelperWrapper>().Object);

            var member = service.GetMember(contentMock.Object, type);

            Assert.IsNotNull(member);
        }

        [TestMethod]
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

            Assert.AreEqual(stringPropertyValue, member.StringProperty);
        }

        [TestMethod]
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

            Assert.AreEqual(integerPropertyValue, member.IntegerProperty);
        }

        [TestMethod]
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

            Assert.AreEqual(floatingBinaryPointPropertyValue, member.FloatingBinaryPointProperty);
        }

        [TestMethod]
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

            Assert.AreEqual(floatingDecimalPointPropertyValue, member.FloatingDecimalPointProperty);
        }

        [TestMethod]
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

            Assert.AreEqual(booleanPropertyValue, member.BooleanProperty);
        }

        [TestMethod]
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

            Assert.AreEqual(dateTimePropertyValue, member.DateTimeProperty);
        }
    }
}