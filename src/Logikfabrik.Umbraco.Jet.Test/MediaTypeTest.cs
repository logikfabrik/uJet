// <copyright file="MediaTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="MediaTypeTest" /> class.
    /// </summary>
    [TestClass]
    public class MediaTypeTest : TestBase
    {
        /// <summary>
        /// Test to get type for media type.
        /// </summary>
        [TestMethod]
        public void CanGetTypeFromAttribute()
        {
            var mediaType = new Jet.MediaType(typeof(MediaType));

            Assert.AreSame(typeof(MediaType), mediaType.Type);
        }

        /// <summary>
        /// Test to get name for media type.
        /// </summary>
        [TestMethod]
        public void CanGetNameFromAttribute()
        {
            var mediaType = new Jet.MediaType(typeof(MediaType));

            Assert.AreEqual("MediaType", mediaType.Name);
        }

        /// <summary>
        /// Test to get alias for media type.
        /// </summary>
        [TestMethod]
        public void CanGetAliasFromAttribute()
        {
            var mediaType = new Jet.MediaType(typeof(MediaType));

            Assert.AreEqual("mediaType", mediaType.Alias);
        }

        /// <summary>
        /// Test to get ID for media type.
        /// </summary>
        [TestMethod]
        public void CanGetIdFromAttribute()
        {
            var mediaType = new Jet.MediaType(typeof(MediaType));

            Assert.AreEqual(Guid.Parse("7bbd6ff5-54ac-4b5a-80fb-4adabe366bcd"), mediaType.Id);
        }

        /// <summary>
        /// Test to get description for media type.
        /// </summary>
        [TestMethod]
        public void CanGetDescriptionFromAttribute()
        {
            var mediaType = new Jet.MediaType(typeof(MediaType));

            Assert.AreEqual("Description", mediaType.Description);
        }

        /// <summary>
        /// Test to get allowed as root for media type.
        /// </summary>
        [TestMethod]
        public void CanGetAllowedAsRootFromAttribute()
        {
            var mediaType = new Jet.MediaType(typeof(MediaType));

            Assert.AreEqual(true, mediaType.AllowedAsRoot);
        }

        /// <summary>
        /// Test to get allowed child node types for media type.
        /// </summary>
        [TestMethod]
        public void CanGetAllowedChildNodeTypesFromAttribute()
        {
            var mediaType = new Jet.MediaType(typeof(MediaType));

            Assert.IsFalse(mediaType.AllowedChildNodeTypes.Any());
        }

        /// <summary>
        /// Test to get properties for media type.
        /// </summary>
        [TestMethod]
        public void CanGetProperties()
        {
            var mediaType = new Jet.MediaType(typeof(MediaType));

            Assert.AreEqual(6, mediaType.Properties.Count());
        }

        /// <summary>
        /// Test to get <see cref="string" /> property for media type.
        /// </summary>
        [TestMethod]
        public void CanGetStringProperty()
        {
            var media = new MediaType();
            var mediaType = new Jet.MediaType(media.GetType());
            var property = mediaType.Properties.First(p => p.Name == GetPropertyName(() => media.StringProperty));

            Assert.AreSame(typeof(string), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="int" /> property for media type.
        /// </summary>
        [TestMethod]
        public void CanGetIntegerProperty()
        {
            var media = new MediaType();
            var mediaType = new Jet.MediaType(media.GetType());
            var property = mediaType.Properties.First(p => p.Name == GetPropertyName(() => media.IntegerProperty));

            Assert.AreSame(typeof(int), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="decimal" /> property for media type.
        /// </summary>
        [TestMethod]
        public void CanGetFloatingDecimalPointProperty()
        {
            var media = new MediaType();
            var mediaType = new Jet.MediaType(media.GetType());
            var property = mediaType.Properties.First(p => p.Name == GetPropertyName(() => media.FloatingDecimalPointProperty));

            Assert.AreSame(typeof(decimal), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="float" /> property for media type.
        /// </summary>
        [TestMethod]
        public void CanGetFloatingBinaryPointProperty()
        {
            var media = new MediaType();
            var mediaType = new Jet.MediaType(media.GetType());
            var property = mediaType.Properties.First(p => p.Name == GetPropertyName(() => media.FloatingBinaryPointProperty));

            Assert.AreSame(typeof(float), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="DateTime" /> property for media type.
        /// </summary>
        [TestMethod]
        public void CanGetDateTimeProperty()
        {
            var media = new MediaType();
            var mediaType = new Jet.MediaType(media.GetType());
            var property = mediaType.Properties.First(p => p.Name == GetPropertyName(() => media.DateTimeProperty));

            Assert.AreSame(typeof(DateTime), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="bool" /> property for media type.
        /// </summary>
        [TestMethod]
        public void CanGetBooleanProperty()
        {
            var media = new MediaType();
            var mediaType = new Jet.MediaType(media.GetType());
            var property = mediaType.Properties.First(p => p.Name == GetPropertyName(() => media.BooleanProperty));

            Assert.AreSame(typeof(bool), property.Type);
        }

        /// <summary>
        /// Test to get non scaffolded property for media type.
        /// </summary>
        [TestMethod]
        public void CanNotGetNonScaffoldedProperty()
        {
            var media = new MediaType();
            var mediaType = new Jet.MediaType(media.GetType());
            var property = mediaType.Properties.FirstOrDefault(p => p.Name == GetPropertyName(() => media.NonScaffoldedStringProperty));

            Assert.IsNull(property);
        }

        /// <summary>
        /// Test to get property without setter for media type.
        /// </summary>
        [TestMethod]
        public void CanNotGetPropertyWithoutSetter()
        {
            var media = new MediaType();
            var mediaType = new Jet.MediaType(media.GetType());
            var property = mediaType.Properties.FirstOrDefault(p => p.Name == GetPropertyName(() => media.StringPropertyWithoutSetter));

            Assert.IsNull(property);
        }

        /// <summary>
        /// The <see cref="MediaType" /> class.
        /// </summary>
        [MediaType(
            "7bbd6ff5-54ac-4b5a-80fb-4adabe366bcd",
            "MediaType",
            Description = "Description",
            AllowedAsRoot = true,
            AllowedChildNodeTypes = new Type[] { })]
        protected class MediaType
        {
            // ReSharper disable once NotAccessedField.Local
            private string stringPropertyWithoutGetter;

            /// <summary>
            /// Initializes a new instance of the <see cref="MediaType" /> class.
            /// </summary>
            public MediaType()
            {
                StringPropertyWithoutSetter = null;
                stringPropertyWithoutGetter = null;
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
            public bool BooleanProperty { get; set; }

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
                set { stringPropertyWithoutGetter = value; }
            }

            // ReSharper disable once UnusedMember.Local
            private string PrivateStringProperty { get; set; }
        }
    }
}
