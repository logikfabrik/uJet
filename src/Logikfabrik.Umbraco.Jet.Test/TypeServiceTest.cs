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
            TypeUtility.BuildAction typeAttributeBuildAction = typeBuilder =>
            {
                var constructor = typeof(DocumentTypeAttribute).GetConstructor(new[] { typeof(string) });

                if (constructor == null)
                {
                    return;
                }

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { "Name" }));
            };

            var type = TypeUtility.CreateType("MyType", new[] { typeAttributeBuildAction });

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
            TypeUtility.BuildAction typeAttributeBuildAction = typeBuilder =>
            {
                var constructor = typeof(DocumentTypeAttribute).GetConstructor(new[] { typeof(string) });

                if (constructor == null)
                {
                    return;
                }

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { "Name" }));
            };

            var type = TypeUtility.CreateType("MyType", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Abstract, new[] { typeAttributeBuildAction });

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
            TypeUtility.BuildAction typeAttributeBuildAction = typeBuilder =>
            {
                var constructor = typeof(DocumentTypeAttribute).GetConstructor(new[] { typeof(string) });

                if (constructor == null)
                {
                    return;
                }

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { "Name" }));
            };

            var type = TypeUtility.CreateType("MyType", new[] { typeAttributeBuildAction, TypeUtility.AbstractTypeBuildAction });

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
            TypeUtility.BuildAction typeAttributeBuildAction = typeBuilder =>
            {
                var constructor = typeof(DataTypeAttribute).GetConstructor(new[] { typeof(Type), typeof(string) });

                if (constructor == null)
                {
                    return;
                }

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { typeof(int), "Editor" }));
            };

            var type = TypeUtility.CreateType("MyType", new[] { typeAttributeBuildAction });

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
            TypeUtility.BuildAction typeAttributeBuildAction = typeBuilder =>
            {
                var constructor = typeof(DataTypeAttribute).GetConstructor(new[] { typeof(Type), typeof(string) });

                if (constructor == null)
                {
                    return;
                }

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { typeof(int), "Editor" }));
            };

            var type = TypeUtility.CreateType("MyType", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Abstract, new[] { typeAttributeBuildAction });

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var dataTypes = typeServiceMock.Object.DataTypes;

            Assert.IsFalse(dataTypes.Any());
        }

        /// <summary>
        /// Test to get data types without public default constructor.
        /// </summary>
        [TestMethod]
        public void CannotGetDataTypesWithoutPublicDefaultConstructor()
        {
            TypeUtility.BuildAction typeAttributeBuildAction = typeBuilder =>
            {
                var constructor = typeof(DataTypeAttribute).GetConstructor(new[] { typeof(Type), typeof(string) });

                if (constructor == null)
                {
                    return;
                }

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { typeof(int), "Editor" }));
            };

            var type = TypeUtility.CreateType("MyType", new[] { typeAttributeBuildAction, TypeUtility.AbstractTypeBuildAction });

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
            TypeUtility.BuildAction typeAttributeBuildAction = typeBuilder =>
            {
                var constructor = typeof(MediaTypeAttribute).GetConstructor(new[] { typeof(string) });

                if (constructor == null)
                {
                    return;
                }

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { "Name" }));
            };

            var type = TypeUtility.CreateType("MyType", new[] { typeAttributeBuildAction });

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
            TypeUtility.BuildAction typeAttributeBuildAction = typeBuilder =>
            {
                var constructor = typeof(MediaTypeAttribute).GetConstructor(new[] { typeof(string) });

                if (constructor == null)
                {
                    return;
                }

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { "Name" }));
            };

            var type = TypeUtility.CreateType("MyType", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Abstract, new[] { typeAttributeBuildAction });

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var mediaTypes = typeServiceMock.Object.MediaTypes;

            Assert.IsFalse(mediaTypes.Any());
        }

        /// <summary>
        /// Test to get media types without public default constructor.
        /// </summary>
        [TestMethod]
        public void CannotGetMediaTypesWithoutPublicDefaultConstructor()
        {
            TypeUtility.BuildAction typeAttributeBuildAction = typeBuilder =>
            {
                var constructor = typeof(MediaTypeAttribute).GetConstructor(new[] { typeof(string) });

                if (constructor == null)
                {
                    return;
                }

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { "Name" }));
            };

            var type = TypeUtility.CreateType("MyType", new[] { typeAttributeBuildAction, TypeUtility.AbstractTypeBuildAction });

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
            TypeUtility.BuildAction typeAttributeBuildAction = typeBuilder =>
            {
                var constructor = typeof(MemberTypeAttribute).GetConstructor(new Type[] { });

                if (constructor == null)
                {
                    return;
                }

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { }));
            };

            var type = TypeUtility.CreateType("MyType", new[] { typeAttributeBuildAction });

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
            TypeUtility.BuildAction typeAttributeBuildAction = typeBuilder =>
            {
                var constructor = typeof(MemberTypeAttribute).GetConstructor(new Type[] { });

                if (constructor == null)
                {
                    return;
                }

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { }));
            };

            var type = TypeUtility.CreateType("MyType", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Abstract, new[] { typeAttributeBuildAction });

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var memberTypes = typeServiceMock.Object.MemberTypes;

            Assert.IsFalse(memberTypes.Any());
        }

        /// <summary>
        /// Test to get member types without public default constructor.
        /// </summary>
        [TestMethod]
        public void CannotGetMemberTypesWithoutPublicDefaultConstructor()
        {
            TypeUtility.BuildAction typeAttributeBuildAction = typeBuilder =>
            {
                var constructor = typeof(MemberTypeAttribute).GetConstructor(new Type[] { });

                if (constructor == null)
                {
                    return;
                }

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { }));
            };

            var type = TypeUtility.CreateType("MyType", new[] { typeAttributeBuildAction, TypeUtility.AbstractTypeBuildAction });

            Func<IEnumerable<Assembly>> getAssemblies = () => new[] { type.Assembly };

            var typeServiceMock = new Mock<TypeService>(getAssemblies) { CallBase = true };

            var memberTypes = typeServiceMock.Object.MemberTypes;

            Assert.IsFalse(memberTypes.Any());
        }
    }
}
