// <copyright file="TypeServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The <see cref="TypeServiceTest" /> class.
    /// </summary>
    [TestClass]
    public class TypeServiceTest
    {
        /// <summary>
        /// Test to get document types.
        /// </summary>
        [TestMethod]
        public void CanGetDocumentTypes()
        {
            TypeUtility.GetAttributeBuilder<DocumentTypeAttribute> builder = () =>
            {
                var constructor = typeof(DocumentTypeAttribute).GetConstructor(new[] { typeof(string) });

                return constructor == null ? null : new CustomAttributeBuilder(constructor, new object[] { "Name" });
            };

            var type = TypeUtility.GetType("MyType", builder);

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var documentTypes = typeServiceMock.Object.DocumentTypes;

            Assert.AreEqual(1, documentTypes.Count());
        }

        /// <summary>
        /// Test to get abstract document types.
        /// </summary>
        [TestMethod]
        public void CannotGetAbstractDocumentTypes()
        {
            TypeUtility.GetAttributeBuilder<DocumentTypeAttribute> builder = () =>
            {
                var constructor = typeof(DocumentTypeAttribute).GetConstructor(new[] { typeof(string) });

                return constructor == null ? null : new CustomAttributeBuilder(constructor, new object[] { "Name" });
            };

            var type = TypeUtility.GetAbstractType("MyType", builder);

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var documentTypes = typeServiceMock.Object.DocumentTypes;

            Assert.IsFalse(documentTypes.Any());
        }

        /// <summary>
        /// Test to get document types without public default constructor.
        /// </summary>
        [TestMethod]
        public void CannotGetDocumentTypesWithoutPublicDefaultConstructor()
        {
            TypeUtility.GetAttributeBuilder<DocumentTypeAttribute> builder = () =>
            {
                var constructor = typeof(DocumentTypeAttribute).GetConstructor(new[] { typeof(string) });

                return constructor == null ? null : new CustomAttributeBuilder(constructor, new object[] { "Name" });
            };

            var type = TypeUtility.GetTypeWithoutPublicDefaultConstructor("MyType", builder);

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var documentTypes = typeServiceMock.Object.DocumentTypes;

            Assert.IsFalse(documentTypes.Any());
        }

        /// <summary>
        /// Test to get data types.
        /// </summary>
        [TestMethod]
        public void CanGetDataTypes()
        {
            TypeUtility.GetAttributeBuilder<DataTypeAttribute> builder = () =>
            {
                var constructor = typeof(DataTypeAttribute).GetConstructor(new[] { typeof(Type), typeof(string) });

                return constructor == null ? null : new CustomAttributeBuilder(constructor, new object[] { typeof(int), "Editor" });
            };

            var type = TypeUtility.GetType("MyType", builder);

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var dataTypes = typeServiceMock.Object.DataTypes;

            Assert.AreEqual(1, dataTypes.Count());
        }

        /// <summary>
        /// Test to get abstract data types.
        /// </summary>
        [TestMethod]
        public void CannotGetAbstractDataTypes()
        {
            TypeUtility.GetAttributeBuilder<DataTypeAttribute> builder = () =>
            {
                var constructor = typeof(DataTypeAttribute).GetConstructor(new[] { typeof(Type), typeof(string) });

                return constructor == null ? null : new CustomAttributeBuilder(constructor, new object[] { typeof(int), "Editor" });
            };

            var type = TypeUtility.GetAbstractType("MyType", builder);

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var dataTypes = typeServiceMock.Object.DataTypes;

            Assert.IsFalse(dataTypes.Any());
        }

        /// <summary>
        /// Test to get data types without public default constructor.
        /// </summary>
        [TestMethod]
        public void CannotGetDataTypesWithoutDefaultConstructor()
        {
            TypeUtility.GetAttributeBuilder<DataTypeAttribute> builder = () =>
            {
                var constructor = typeof(DataTypeAttribute).GetConstructor(new[] { typeof(Type), typeof(string) });

                return constructor == null ? null : new CustomAttributeBuilder(constructor, new object[] { typeof(int), "Editor" });
            };

            var type = TypeUtility.GetTypeWithoutPublicDefaultConstructor("MyType", builder);

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var dataTypes = typeServiceMock.Object.DataTypes;

            Assert.IsFalse(dataTypes.Any());
        }

        /// <summary>
        /// Test to get media types.
        /// </summary>
        [TestMethod]
        public void CanGetMediaTypes()
        {
            TypeUtility.GetAttributeBuilder<MediaTypeAttribute> builder = () =>
            {
                var constructor = typeof(MediaTypeAttribute).GetConstructor(new[] { typeof(string) });

                return constructor == null ? null : new CustomAttributeBuilder(constructor, new object[] { "Name" });
            };

            var type = TypeUtility.GetType("MyType", builder);

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var mediaTypes = typeServiceMock.Object.MediaTypes;

            Assert.AreEqual(1, mediaTypes.Count());
        }

        /// <summary>
        /// Test to get abstract media types.
        /// </summary>
        [TestMethod]
        public void CannotGetAbstractMediaTypes()
        {
            TypeUtility.GetAttributeBuilder<MediaTypeAttribute> builder = () =>
            {
                var constructor = typeof(MediaTypeAttribute).GetConstructor(new[] { typeof(string) });

                return constructor == null ? null : new CustomAttributeBuilder(constructor, new object[] { "Name" });
            };

            var type = TypeUtility.GetAbstractType("MyType", builder);

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var mediaTypes = typeServiceMock.Object.MediaTypes;

            Assert.IsFalse(mediaTypes.Any());
        }

        /// <summary>
        /// Test to get media types without public default constructor.
        /// </summary>
        [TestMethod]
        public void CannotGetMediaTypesWithoutDefaultConstructor()
        {
            TypeUtility.GetAttributeBuilder<MediaTypeAttribute> builder = () =>
            {
                var constructor = typeof(MediaTypeAttribute).GetConstructor(new[] { typeof(string) });

                return constructor == null ? null : new CustomAttributeBuilder(constructor, new object[] { "Name" });
            };

            var type = TypeUtility.GetTypeWithoutPublicDefaultConstructor("MyType", builder);

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var mediaTypes = typeServiceMock.Object.MediaTypes;

            Assert.IsFalse(mediaTypes.Any());
        }

        /// <summary>
        /// Test to get member types.
        /// </summary>
        [TestMethod]
        public void CanGetMemberTypes()
        {
            TypeUtility.GetAttributeBuilder<MemberTypeAttribute> builder = () =>
            {
                var constructor = typeof(MemberTypeAttribute).GetConstructor(new Type[] { });

                return constructor == null ? null : new CustomAttributeBuilder(constructor, new object[] { });
            };

            var type = TypeUtility.GetType("MyType", builder);

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var memberTypes = typeServiceMock.Object.MemberTypes;

            Assert.AreEqual(1, memberTypes.Count());
        }

        /// <summary>
        /// Test to get abstract member types.
        /// </summary>
        [TestMethod]
        public void CannotGetAbstractMemberTypes()
        {
            TypeUtility.GetAttributeBuilder<MemberTypeAttribute> builder = () =>
            {
                var constructor = typeof(MemberTypeAttribute).GetConstructor(new Type[] { });

                return constructor == null ? null : new CustomAttributeBuilder(constructor, new object[] { });
            };

            var type = TypeUtility.GetAbstractType("MyType", builder);

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var memberTypes = typeServiceMock.Object.MemberTypes;

            Assert.IsFalse(memberTypes.Any());
        }

        /// <summary>
        /// Test to get member types without public default constructor.
        /// </summary>
        [TestMethod]
        public void CannotGetMemberTypesWithoutDefaultConstructor()
        {
            TypeUtility.GetAttributeBuilder<MemberTypeAttribute> builder = () =>
            {
                var constructor = typeof(MemberTypeAttribute).GetConstructor(new Type[] { });

                return constructor == null ? null : new CustomAttributeBuilder(constructor, new object[] { });
            };

            var type = TypeUtility.GetTypeWithoutPublicDefaultConstructor("MyType", builder);

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var memberTypes = typeServiceMock.Object.MemberTypes;

            Assert.IsFalse(memberTypes.Any());
        }
    }
}
