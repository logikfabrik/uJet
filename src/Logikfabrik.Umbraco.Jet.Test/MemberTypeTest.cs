// <copyright file="MemberTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MemberTypeTest : TestBase
    {
        [TestMethod]
        public void CanGetTypeFromAttribute()
        {
            var memberType = new MemberType(typeof(Models.MemberType));

            Assert.AreSame(typeof(Models.MemberType), memberType.ModelType);
        }

        [TestMethod]
        public void CanGetNameFromAttribute()
        {
            var memberType = new MemberType(typeof(Models.MemberType));

            Assert.AreEqual("MemberType", memberType.Name);
        }

        [TestMethod]
        public void CanGetAliasFromAttribute()
        {
            var memberType = new MemberType(typeof(Models.MemberType));

            Assert.AreEqual("memberType", memberType.Alias);
        }

        [TestMethod]
        public void CanGetIdFromAttribute()
        {
            var memberType = new MemberType(typeof(Models.MemberType));

            Assert.AreEqual(Guid.Parse("0b698529-3507-4f5b-9155-95a3b51ee574"), memberType.Id);
        }

        [TestMethod]
        public void CanGetDescriptionFromAttribute()
        {
            var memberType = new MemberType(typeof(Models.MemberType));

            Assert.AreEqual("Description", memberType.Description);
        }

        [TestMethod]
        public void CanGetProperties()
        {
            var memberType = new MemberType(typeof(Models.MemberType));

            Assert.AreEqual(11, memberType.Properties.Count());
        }

        [TestMethod]
        public void CanGetStringProperty()
        {
            var member = new Models.MemberType();
            var memberType = new MemberType(member.GetType());
            var property = memberType.Properties.First(p => p.Name == GetPropertyName(() => member.StringProperty));

            Assert.AreSame(typeof(string), property.Type);
        }

        [TestMethod]
        public void CanGetIntegerProperty()
        {
            var member = new Models.MemberType();
            var memberType = new MemberType(member.GetType());
            var property = memberType.Properties.First(p => p.Name == GetPropertyName(() => member.IntegerProperty));

            Assert.AreSame(typeof(int), property.Type);
        }

        [TestMethod]
        public void CanGetFloatingDecimalPointProperty()
        {
            var member = new Models.MemberType();
            var memberType = new MemberType(member.GetType());
            var property = memberType.Properties.First(p => p.Name == GetPropertyName(() => member.FloatingDecimalPointProperty));

            Assert.AreSame(typeof(decimal), property.Type);
        }

        [TestMethod]
        public void CanGetFloatingBinaryPointProperty()
        {
            var member = new Models.MemberType();
            var memberType = new MemberType(member.GetType());
            var property = memberType.Properties.First(p => p.Name == GetPropertyName(() => member.FloatingBinaryPointProperty));

            Assert.AreSame(typeof(float), property.Type);
        }

        [TestMethod]
        public void CanGetDateTimeProperty()
        {
            var member = new Models.MemberType();
            var memberType = new MemberType(member.GetType());
            var property = memberType.Properties.First(p => p.Name == GetPropertyName(() => member.DateTimeProperty));

            Assert.AreSame(typeof(DateTime), property.Type);
        }

        [TestMethod]
        public void CanGetBooleanProperty()
        {
            var member = new Models.MemberType();
            var memberType = new MemberType(member.GetType());
            var property = memberType.Properties.First(p => p.Name == GetPropertyName(() => member.BooleanProperty));

            Assert.AreSame(typeof(bool), property.Type);
        }

        [TestMethod]
        public void CannotGetNonScaffoldedProperty()
        {
            var member = new Models.MemberType();
            var memberType = new MemberType(member.GetType());
            var property = memberType.Properties.FirstOrDefault(p => p.Name == GetPropertyName(() => member.NonScaffoldedStringProperty));

            Assert.IsNull(property);
        }

        [TestMethod]
        public void CannotGetPropertyWithoutSetter()
        {
            var member = new Models.MemberType();
            var memberType = new MemberType(member.GetType());
            var property = memberType.Properties.FirstOrDefault(p => p.Name == GetPropertyName(() => member.StringPropertyWithoutSetter));

            Assert.IsNull(property);
        }

        [TestMethod]
        public void CannotGetPrivateProperty()
        {
            var member = new Models.MemberType();
            var memberType = new MemberType(member.GetType());
            var property = memberType.Properties.FirstOrDefault(p => p.Name == "PrivateStringProperty");

            Assert.IsNull(property);
        }
    }
}