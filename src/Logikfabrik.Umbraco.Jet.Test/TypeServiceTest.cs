// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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
