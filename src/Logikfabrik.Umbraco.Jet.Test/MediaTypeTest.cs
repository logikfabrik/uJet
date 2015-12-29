// <copyright file="MediaTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MediaTypeTest : TestBase
    {
        [TestMethod]
        public void CanGetComposition()
        {
            var mediaType = new MediaType(typeof(Models.MediaType));

            Assert.AreEqual(3, mediaType.Composition[typeof(Models.MediaType)].Count());
        }

        [TestMethod]
        public void CanGetTypeFromAttribute()
        {
            var mediaType = new MediaType(typeof(Models.MediaType));

            Assert.AreSame(typeof(Models.MediaType), mediaType.Type);
        }

        [TestMethod]
        public void CanGetNameFromAttribute()
        {
            var mediaType = new MediaType(typeof(Models.MediaType));

            Assert.AreEqual("MediaType", mediaType.Name);
        }

        [TestMethod]
        public void CanGetAliasFromAttribute()
        {
            var mediaType = new MediaType(typeof(Models.MediaType));

            Assert.AreEqual("mediaType", mediaType.Alias);
        }

        [TestMethod]
        public void CanGetIdFromAttribute()
        {
            var mediaType = new MediaType(typeof(Models.MediaType));

            Assert.AreEqual(Guid.Parse("7bbd6ff5-54ac-4b5a-80fb-4adabe366bcd"), mediaType.Id);
        }

        [TestMethod]
        public void CanGetDescriptionFromAttribute()
        {
            var mediaType = new MediaType(typeof(Models.MediaType));

            Assert.AreEqual("Description", mediaType.Description);
        }

        [TestMethod]
        public void CanGetAllowedAsRootFromAttribute()
        {
            var mediaType = new MediaType(typeof(Models.MediaType));

            Assert.AreEqual(true, mediaType.AllowedAsRoot);
        }

        [TestMethod]
        public void CanGetAllowedChildNodeTypesFromAttribute()
        {
            var mediaType = new MediaType(typeof(Models.MediaType));

            Assert.IsFalse(mediaType.AllowedChildNodeTypes.Any());
        }

        [TestMethod]
        public void CanGetProperties()
        {
            var mediaType = new MediaType(typeof(Models.MediaType));

            Assert.AreEqual(11, mediaType.Properties.Count());
        }

        [TestMethod]
        public void CanGetStringProperty()
        {
            var media = new Models.MediaType();
            var mediaType = new MediaType(media.GetType());
            var property = mediaType.Properties.First(p => p.Name == GetPropertyName(() => media.StringProperty));

            Assert.AreSame(typeof(string), property.Type);
        }

        [TestMethod]
        public void CanGetIntegerProperty()
        {
            var media = new Models.MediaType();
            var mediaType = new MediaType(media.GetType());
            var property = mediaType.Properties.First(p => p.Name == GetPropertyName(() => media.IntegerProperty));

            Assert.AreSame(typeof(int), property.Type);
        }

        [TestMethod]
        public void CanGetFloatingDecimalPointProperty()
        {
            var media = new Models.MediaType();
            var mediaType = new MediaType(media.GetType());
            var property = mediaType.Properties.First(p => p.Name == GetPropertyName(() => media.FloatingDecimalPointProperty));

            Assert.AreSame(typeof(decimal), property.Type);
        }

        [TestMethod]
        public void CanGetFloatingBinaryPointProperty()
        {
            var media = new Models.MediaType();
            var mediaType = new MediaType(media.GetType());
            var property = mediaType.Properties.First(p => p.Name == GetPropertyName(() => media.FloatingBinaryPointProperty));

            Assert.AreSame(typeof(float), property.Type);
        }

        [TestMethod]
        public void CanGetDateTimeProperty()
        {
            var media = new Models.MediaType();
            var mediaType = new MediaType(media.GetType());
            var property = mediaType.Properties.First(p => p.Name == GetPropertyName(() => media.DateTimeProperty));

            Assert.AreSame(typeof(DateTime), property.Type);
        }

        [TestMethod]
        public void CanGetBooleanProperty()
        {
            var media = new Models.MediaType();
            var mediaType = new MediaType(media.GetType());
            var property = mediaType.Properties.First(p => p.Name == GetPropertyName(() => media.BooleanProperty));

            Assert.AreSame(typeof(bool), property.Type);
        }

        [TestMethod]
        public void CannotGetNonScaffoldedProperty()
        {
            var media = new Models.MediaType();
            var mediaType = new MediaType(media.GetType());
            var property = mediaType.Properties.FirstOrDefault(p => p.Name == GetPropertyName(() => media.NonScaffoldedStringProperty));

            Assert.IsNull(property);
        }

        [TestMethod]
        public void CannotGetPropertyWithoutSetter()
        {
            var media = new Models.MediaType();
            var mediaType = new MediaType(media.GetType());
            var property = mediaType.Properties.FirstOrDefault(p => p.Name == GetPropertyName(() => media.StringPropertyWithoutSetter));

            Assert.IsNull(property);
        }

        [TestMethod]
        public void CannotGetPrivateProperty()
        {
            var media = new Models.MediaType();
            var mediaType = new MediaType(media.GetType());
            var property = mediaType.Properties.FirstOrDefault(p => p.Name == "PrivateStringProperty");

            Assert.IsNull(property);
        }
    }
}