// <copyright file="TypeServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypeServiceTest
    {
        [TestMethod]
        public void CanGetDocumentTypes()
        {
            var documentTypes = TypeService.Instance.DocumentTypes;

            Assert.IsTrue(documentTypes.Any());
        }

        [TestMethod]
        public void CannotGetAbstractDocumentTypes()
        {
            var documentTypes = TypeService.Instance.DocumentTypes;
            var containsAbstractType = documentTypes.Contains(typeof(TypeServiceTestAbstractDocumentType));

            Assert.IsFalse(containsAbstractType);
        }

        [TestMethod]
        public void CannotGetDocumentTypesWithoutDefaultConstructor()
        {
            var documentTypes = TypeService.Instance.DocumentTypes;
            var containsTypeWithoutDefaultConstructor = documentTypes.Contains(typeof(TypeServiceTestDocumentTypeWithoutDefaultConstructor));

            Assert.IsFalse(containsTypeWithoutDefaultConstructor);
        }

        [TestMethod]
        public void CanGetDataTypes()
        {
            var dataTypes = TypeService.Instance.DataTypes;

            Assert.IsTrue(dataTypes.Any());
        }

        [TestMethod]
        public void CannotGetAbstractDataTypes()
        {
            var dataTypes = TypeService.Instance.DataTypes;
            var containsAbstractType = dataTypes.Contains(typeof(TypeServiceTestAbstractDataType));

            Assert.IsFalse(containsAbstractType);
        }

        [TestMethod]
        public void CannotGetDataTypesWithoutDefaultConstructor()
        {
            var dataTypes = TypeService.Instance.DataTypes;
            var containsTypeWithoutDefaultConstructor = dataTypes.Contains(typeof(TypeServiceTestDataTypeWithoutDefaultConstructor));

            Assert.IsFalse(containsTypeWithoutDefaultConstructor);
        }

        [TestMethod]
        public void CanGetMediaTypes()
        {
            var mediaTypes = TypeService.Instance.MediaTypes;

            Assert.IsTrue(mediaTypes.Any());
        }

        [TestMethod]
        public void CannotGetAbstractMediaTypes()
        {
            var mediaTypes = TypeService.Instance.MediaTypes;
            var containsAbstractType = mediaTypes.Contains(typeof(TypeServiceTestAbstractMediaType));

            Assert.IsFalse(containsAbstractType);
        }

        [TestMethod]
        public void CannotGetMediaTypesWithoutDefaultConstructor()
        {
            var mediaTypes = TypeService.Instance.MediaTypes;
            var containsTypeWithoutDefaultConstructor = mediaTypes.Contains(typeof(TypeServiceTestMediaTypeWithoutDefaultConstructor));

            Assert.IsFalse(containsTypeWithoutDefaultConstructor);
        }

        [DocumentType("TypeServiceTestDocumentType")]
        public class TypeServiceTestDocumentType
        {
        }

        [DocumentType("TypeServiceTestAbstractDocumentType")]
        public abstract class TypeServiceTestAbstractDocumentType
        {
        }

        [DocumentType("TypeServiceTestDocumentTypeWithoutDefaultConstructor")]
        public class TypeServiceTestDocumentTypeWithoutDefaultConstructor
        {
            private TypeServiceTestDocumentTypeWithoutDefaultConstructor()
            {
            }

            // ReSharper disable once UnusedParameter.Local
            public TypeServiceTestDocumentTypeWithoutDefaultConstructor(object param)
                : this()
            {
            }
        }

        [MediaType("TypeServiceTestMediaType")]
        public class TypeServiceTestMediaType
        {
        }

        [MediaType("TypeServiceTestAbstractMediaType")]
        public abstract class TypeServiceTestAbstractMediaType
        {
        }

        [MediaType("TypeServiceTestMediaTypeWithoutDefaultConstructor")]
        public class TypeServiceTestMediaTypeWithoutDefaultConstructor
        {
            private TypeServiceTestMediaTypeWithoutDefaultConstructor()
            {
            }

            // ReSharper disable once UnusedParameter.Local
            public TypeServiceTestMediaTypeWithoutDefaultConstructor(object param)
                : this()
            {
            }
        }

        [DataType(typeof(int), "TypeServiceTestDataTypeEditor")]
        public class TypeServiceTestDataType
        {
        }

        [DataType(typeof(int), "TypeServiceTestDataTypeEditor")]
        public abstract class TypeServiceTestAbstractDataType
        {
        }

        [DataType(typeof(int), "TypeServiceTestDataTypeEditorWithoutDefaultConstructor")]
        public class TypeServiceTestDataTypeWithoutDefaultConstructor
        {
            private TypeServiceTestDataTypeWithoutDefaultConstructor()
            {
            }

            // ReSharper disable once UnusedParameter.Local
            public TypeServiceTestDataTypeWithoutDefaultConstructor(object param)
                : this()
            {
            }
        }
    }
}
