// <copyright file="MemberTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="MemberTypeTest" /> class.
    /// </summary>
    [TestClass]
    public class MemberTypeTest : TestBase
    {
        /// <summary>
        /// Test to get type for member type.
        /// </summary>
        [TestMethod]
        public void CanGetTypeFromAttribute()
        {
            var memberType = new Jet.MemberType(typeof(MemberType));

            Assert.AreSame(typeof(MemberType), memberType.Type);
        }

        /// <summary>
        /// Test to get name for member type.
        /// </summary>
        [TestMethod]
        public void CanGetNameFromAttribute()
        {
            var memberType = new Jet.MemberType(typeof(MemberType));

            Assert.AreEqual("MemberType", memberType.Name);
        }

        /// <summary>
        /// Test to get alias for member type.
        /// </summary>
        [TestMethod]
        public void CanGetAliasFromAttribute()
        {
            var memberType = new Jet.MemberType(typeof(MemberType));

            Assert.AreEqual("memberType", memberType.Alias);
        }

        /// <summary>
        /// Test to get ID for member type.
        /// </summary>
        [TestMethod]
        public void CanGetIdFromAttribute()
        {
            var memberType = new Jet.MemberType(typeof(MemberType));

            Assert.AreEqual(Guid.Parse("0b698529-3507-4f5b-9155-95a3b51ee574"), memberType.Id);
        }

        /// <summary>
        /// Test to get description for member type.
        /// </summary>
        [TestMethod]
        public void CanGetDescriptionFromAttribute()
        {
            var memberType = new Jet.MemberType(typeof(MemberType));

            Assert.AreEqual("Description", memberType.Description);
        }

        /// <summary>
        /// Test to get properties for member type.
        /// </summary>
        [TestMethod]
        public void CanGetProperties()
        {
            var memberType = new Jet.MemberType(typeof(MemberType));

            Assert.AreEqual(6, memberType.Properties.Count());
        }

        /// <summary>
        /// Test to get <see cref="string" /> property for member type.
        /// </summary>
        [TestMethod]
        public void CanGetStringProperty()
        {
            var member = new MemberType();
            var memberType = new Jet.MemberType(member.GetType());
            var property = memberType.Properties.First(p => p.Name == GetPropertyName(() => member.StringProperty));

            Assert.AreSame(typeof(string), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="int" /> property for member type.
        /// </summary>
        [TestMethod]
        public void CanGetIntegerProperty()
        {
            var member = new MemberType();
            var memberType = new Jet.MemberType(member.GetType());
            var property = memberType.Properties.First(p => p.Name == GetPropertyName(() => member.IntegerProperty));

            Assert.AreSame(typeof(int), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="decimal" /> property for member type.
        /// </summary>
        [TestMethod]
        public void CanGetFloatingDecimalPointProperty()
        {
            var member = new MemberType();
            var memberType = new Jet.MemberType(member.GetType());
            var property = memberType.Properties.First(p => p.Name == GetPropertyName(() => member.FloatingDecimalPointProperty));

            Assert.AreSame(typeof(decimal), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="float" /> property for member type.
        /// </summary>
        [TestMethod]
        public void CanGetFloatingBinaryPointProperty()
        {
            var member = new MemberType();
            var memberType = new Jet.MemberType(member.GetType());
            var property = memberType.Properties.First(p => p.Name == GetPropertyName(() => member.FloatingBinaryPointProperty));

            Assert.AreSame(typeof(float), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="DateTime" /> property for member type.
        /// </summary>
        [TestMethod]
        public void CanGetDateTimeProperty()
        {
            var member = new MemberType();
            var memberType = new Jet.MemberType(member.GetType());
            var property = memberType.Properties.First(p => p.Name == GetPropertyName(() => member.DateTimeProperty));

            Assert.AreSame(typeof(DateTime), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="bool" /> property for member type.
        /// </summary>
        [TestMethod]
        public void CanGetBooleanProperty()
        {
            var member = new MemberType();
            var memberType = new Jet.MemberType(member.GetType());
            var property = memberType.Properties.First(p => p.Name == GetPropertyName(() => member.BooleanProperty));

            Assert.AreSame(typeof(bool), property.Type);
        }

        /// <summary>
        /// Test to get non scaffolded property for member type.
        /// </summary>
        [TestMethod]
        public void CanNotGetNonScaffoldedProperty()
        {
            var member = new MemberType();
            var memberType = new Jet.MemberType(member.GetType());
            var property = memberType.Properties.FirstOrDefault(p => p.Name == GetPropertyName(() => member.NonScaffoldedStringProperty));

            Assert.IsNull(property);
        }

        /// <summary>
        /// Test to get property without setter for member type.
        /// </summary>
        [TestMethod]
        public void CanNotGetPropertyWithoutSetter()
        {
            var member = new MemberType();
            var memberType = new Jet.MemberType(member.GetType());
            var property = memberType.Properties.FirstOrDefault(p => p.Name == GetPropertyName(() => member.StringPropertyWithoutSetter));

            Assert.IsNull(property);
        }

        /// <summary>
        /// The <see cref="MemberType" /> class.
        /// </summary>
        [MemberType(
            "0b698529-3507-4f5b-9155-95a3b51ee574",
            "MemberType",
            Description = "Description")]
        protected class MemberType
        {
            // ReSharper disable once NotAccessedField.Local
            private string _stringPropertyWithoutGetter;

            /// <summary>
            /// Initializes a new instance of the <see cref="MemberType" /> class.
            /// </summary>
            public MemberType()
            {
                StringPropertyWithoutSetter = null;
                _stringPropertyWithoutGetter = null;
            }

            /// <summary>
            /// Gets or sets the string property value.
            /// </summary>
            /// <value>
            /// The string property value.
            /// </value>
            public string StringProperty { get; set; }

            /// <summary>
            /// Gets or sets the integer property value.
            /// </summary>
            /// <value>
            /// The integer property value.
            /// </value>
            public int IntegerProperty { get; set; }

            /// <summary>
            /// Gets or sets the floating binary point property value.
            /// </summary>
            /// <value>
            /// The floating binary point property value.
            /// </value>
            public float FloatingBinaryPointProperty { get; set; }

            /// <summary>
            /// Gets or sets the floating decimal point property value.
            /// </summary>
            /// <value>
            /// The floating decimal point property value.
            /// </value>
            public decimal FloatingDecimalPointProperty { get; set; }

            /// <summary>
            /// Gets or sets the boolean property value.
            /// </summary>
            /// <value>
            /// The boolean property value.
            /// </value>
#pragma warning disable SA1623
            public bool BooleanProperty { get; set; }
#pragma warning restore SA1623

            /// <summary>
            /// Gets or sets the DateTime property value.
            /// </summary>
            /// <value>
            /// The DateTime property value.
            /// </value>
            public DateTime DateTimeProperty { get; set; }

            /// <summary>
            /// Gets or sets the non scaffolded string property value.
            /// </summary>
            /// <value>
            /// The non scaffolded string property value.
            /// </value>
            [ScaffoldColumn(false)]
            public string NonScaffoldedStringProperty { get; set; }

            /// <summary>
            /// Gets the string property value, for property without setter.
            /// </summary>
            /// <value>
            /// The string property value.
            /// </value>
            public string StringPropertyWithoutSetter { get; }

            /// <summary>
            /// Sets the string property value, for property without getter.
            /// </summary>
            /// <value>
            /// The string property value.
            /// </value>
            public string StringPropertyWithoutGetter
            {
                set { _stringPropertyWithoutGetter = value; }
            }

            // ReSharper disable once UnusedMember.Local
            private string PrivateStringProperty { get; set; }
        }
    }
}
