// <copyright file="ContentTypeModelValidatorTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Reflection.Emit;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class ContentTypeModelValidatorTest
    {
        [Theory]
        [CustomAutoData]
        public void CanFindConflictById(string typeNameX, string typeNameY, Guid id, string name)
        {
            var modelX = new DocumentType(new DocumentTypeModelTypeBuilder(typeNameX, id, name).Create(Scope.Public));
            var modelY = new DocumentType(new DocumentTypeModelTypeBuilder(typeNameY, id, name).Create(Scope.Public));

            var contentTypeModelValidator = new ContentTypeModelValidator<DocumentType, DocumentTypeAttribute>();

            Assert.Throws<InvalidOperationException>(() => contentTypeModelValidator.Validate(new[] { modelX, modelY }));
        }

        [Theory]
        [CustomAutoData]
        public void CanFindConflictByAlias(string typeName, string name)
        {
            var modelX = new DocumentType(new DocumentTypeModelTypeBuilder(typeName, name).Create(Scope.Public));
            var modelY = new DocumentType(new DocumentTypeModelTypeBuilder(typeName, name).Create(Scope.Public));

            var contentTypeModelValidator = new ContentTypeModelValidator<DocumentType, DocumentTypeAttribute>();

            Assert.Throws<InvalidOperationException>(() => contentTypeModelValidator.Validate(new[] { modelX, modelY }));
        }

        [Theory]
        [CustomAutoData]
        public void CanFindConflictByPropertyId(DocumentTypeModelTypeBuilder builder, Guid propertyId, string propertyNameX, Type propertyTypeX, string propertyNameY, Type propertyTypeY)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            builder.AddProperty(Scope.Public, Accessor.GetSet, propertyNameX, propertyTypeX, new[] { new CustomAttributeBuilder(typeof(IdAttribute).GetConstructor(new[] { typeof(string) }), new object[] { propertyId.ToString() }) });
            builder.AddProperty(Scope.Public, Accessor.GetSet, propertyNameY, propertyTypeY, new[] { new CustomAttributeBuilder(typeof(IdAttribute).GetConstructor(new[] { typeof(string) }), new object[] { propertyId.ToString() }) });

            // ReSharper restore AssignNullToNotNullAttribute
            var model = new DocumentType(builder.Create(Scope.Public));

            var contentTypeModelValidator = new ContentTypeModelValidator<DocumentType, DocumentTypeAttribute>();

            Assert.Throws<InvalidOperationException>(() => contentTypeModelValidator.Validate(new[] { model }));
        }

        [Theory]
        [CustomAutoData]
        public void CanFindConflictByPropertyAlias(DocumentTypeModelTypeBuilder builder, string propertyAlias, string propertyNameX, Type propertyTypeX, string propertyNameY, Type propertyTypeY)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            builder.AddProperty(Scope.Public, Accessor.GetSet, propertyNameX, propertyTypeX, new[] { new CustomAttributeBuilder(typeof(AliasAttribute).GetConstructor(new[] { typeof(string) }), new object[] { propertyAlias }) });
            builder.AddProperty(Scope.Public, Accessor.GetSet, propertyNameY, propertyTypeY, new[] { new CustomAttributeBuilder(typeof(AliasAttribute).GetConstructor(new[] { typeof(string) }), new object[] { propertyAlias }) });

            // ReSharper restore AssignNullToNotNullAttribute
            var model = new DocumentType(builder.Create(Scope.Public));

            var contentTypeModelValidator = new ContentTypeModelValidator<DocumentType, DocumentTypeAttribute>();

            Assert.Throws<InvalidOperationException>(() => contentTypeModelValidator.Validate(new[] { model }));
        }
    }
}