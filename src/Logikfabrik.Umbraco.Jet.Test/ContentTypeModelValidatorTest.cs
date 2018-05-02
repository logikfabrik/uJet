// <copyright file="ContentTypeModelValidatorTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Reflection.Emit;
    using AutoFixture.Xunit2;
    using Utilities;
    using Xunit;

    public class ContentTypeModelValidatorTest
    {
        [Theory]
        [AutoData]
        public void CanFindConflictById(string typeNameX, string typeNameY, Guid id, string name)
        {
            var modelX = new DocumentType(new DocumentTypeModelTypeBuilder(typeNameX, id.ToString(), name).CreateType());
            var modelY = new DocumentType(new DocumentTypeModelTypeBuilder(typeNameY, id.ToString(), name).CreateType());

            var contentTypeModelValidator = new ContentTypeModelValidator<DocumentType, DocumentTypeAttribute>();

            Assert.Throws<InvalidOperationException>(() => contentTypeModelValidator.Validate(new[] { modelX, modelY }));
        }

        [Theory]
        [AutoData]
        public void CanFindConflictByAlias(string typeName, string name)
        {
            var modelX = new DocumentType(new DocumentTypeModelTypeBuilder(typeName, name).CreateType());
            var modelY = new DocumentType(new DocumentTypeModelTypeBuilder(typeName, name).CreateType());

            var contentTypeModelValidator = new ContentTypeModelValidator<DocumentType, DocumentTypeAttribute>();

            Assert.Throws<InvalidOperationException>(() => contentTypeModelValidator.Validate(new[] { modelX, modelY }));
        }

        [Theory]
        [AutoData]
        public void CanFindConflictByPropertyId(string typeName, string name, Guid propertyId, Type propertyTypeX, string propertyNameX, Type propertyTypeY, string propertyNameY)
        {
            var typeBuilder = new DocumentTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            // ReSharper disable AssignNullToNotNullAttribute
            typeBuilder.AddPublicProperty(propertyNameX, propertyTypeX, new[] { new CustomAttributeBuilder(typeof(IdAttribute).GetConstructor(new[] { typeof(string) }), new object[] { propertyId.ToString() }) });
            typeBuilder.AddPublicProperty(propertyNameY, propertyTypeY, new[] { new CustomAttributeBuilder(typeof(IdAttribute).GetConstructor(new[] { typeof(string) }), new object[] { propertyId.ToString() }) });

            // ReSharper restore AssignNullToNotNullAttribute
            var model = new DocumentType(typeBuilder.CreateType());

            var contentTypeModelValidator = new ContentTypeModelValidator<DocumentType, DocumentTypeAttribute>();

            Assert.Throws<InvalidOperationException>(() => contentTypeModelValidator.Validate(new[] { model }));
        }

        [Theory]
        [AutoData]
        public void CanFindConflictByPropertyAlias(string typeName, string name, string propertyAlias, Type propertyTypeX, string propertyNameX, Type propertyTypeY, string propertyNameY)
        {
            var typeBuilder = new DocumentTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            // ReSharper disable AssignNullToNotNullAttribute
            typeBuilder.AddPublicProperty(propertyNameX, propertyTypeX, new[] { new CustomAttributeBuilder(typeof(AliasAttribute).GetConstructor(new[] { typeof(string) }), new object[] { propertyAlias }) });
            typeBuilder.AddPublicProperty(propertyNameY, propertyTypeY, new[] { new CustomAttributeBuilder(typeof(AliasAttribute).GetConstructor(new[] { typeof(string) }), new object[] { propertyAlias }) });

            // ReSharper restore AssignNullToNotNullAttribute
            var model = new DocumentType(typeBuilder.CreateType());

            var contentTypeModelValidator = new ContentTypeModelValidator<DocumentType, DocumentTypeAttribute>();

            Assert.Throws<InvalidOperationException>(() => contentTypeModelValidator.Validate(new[] { model }));
        }
    }
}