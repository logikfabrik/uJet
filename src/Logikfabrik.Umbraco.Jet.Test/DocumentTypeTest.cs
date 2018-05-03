// <copyright file="DocumentTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Linq;
    using AutoFixture.Xunit2;
    using Shouldly;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class DocumentTypeTest
    {
        [Theory]
        [CustomAutoData]
        public void CanGetTypeFromAttribute(DocumentTypeModelTypeBuilder builder)
        {
            var modelType = builder.CreateType();

            var model = new DocumentType(modelType);

            model.ModelType.ShouldBe(modelType);
        }

        [Theory]
        [AutoData]
        public void CanGetIdFromAttribute(string typeName, Guid id, string name)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, id, name).CreateType();

            var model = new DocumentType(modelType);

            model.Id.ShouldBe(id);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetNameFromAttribute(DocumentTypeModelTypeBuilder builder)
        {
            var modelType = builder.CreateType();

            var model = new DocumentType(modelType);

            model.Name.ShouldBe(builder.Name);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetDescriptionFromAttribute(DocumentTypeModelTypeBuilder builder, string description)
        {
            builder.Description = description;

            var modelType = builder.CreateType();

            var model = new DocumentType(modelType);

            model.Description.ShouldBe(description);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetIconFromAttribute(DocumentTypeModelTypeBuilder builder, string icon)
        {
            builder.Icon = icon;

            var modelType = builder.CreateType();

            var model = new DocumentType(modelType);

            model.Icon.ShouldBe(icon);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetIsContainerFromAttribute(DocumentTypeModelTypeBuilder builder, bool isContainer)
        {
            builder.IsContainer = isContainer;

            var modelType = builder.CreateType();

            var model = new DocumentType(modelType);

            model.IsContainer.ShouldBe(isContainer);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetAliasFromAttribute(DocumentTypeModelTypeBuilder builder, string alias)
        {
            builder.Alias = alias;

            var modelType = builder.CreateType();

            var model = new DocumentType(modelType);

            model.Alias.ShouldBe(alias);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetThumbnailFromAttribute(DocumentTypeModelTypeBuilder builder, string thumbnail)
        {
            builder.Thumbnail = thumbnail;

            var modelType = builder.CreateType();

            var model = new DocumentType(modelType);

            model.Thumbnail.ShouldBe(thumbnail);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetAllowedAsRootFromAttribute(DocumentTypeModelTypeBuilder builder, bool allowedAsRoot)
        {
            builder.AllowedAsRoot = allowedAsRoot;

            var modelType = builder.CreateType();

            var model = new DocumentType(modelType);

            model.AllowedAsRoot.ShouldBe(allowedAsRoot);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetAllowedChildNodeTypesFromAttribute(DocumentTypeModelTypeBuilder builder, Type[] allowedChildNodeTypes)
        {
            builder.AllowedChildNodeTypes = allowedChildNodeTypes;

            var modelType = builder.CreateType();

            var model = new DocumentType(modelType);

            model.AllowedChildNodeTypes.Any().ShouldBeTrue();
        }

        [Theory]
        [CustomAutoData]
        public void CanGetCompositionNodeTypesFromAttribute(DocumentTypeModelTypeBuilder builder, Type[] compositionNodeTypes)
        {
            builder.CompositionNodeTypes = compositionNodeTypes;

            var modelType = builder.CreateType();

            var model = new DocumentType(modelType);

            model.CompositionNodeTypes.Any().ShouldBeTrue();
        }

        [Theory]
        [CustomAutoData]
        public void CanGetDefaultTemplateFromAttribute(DocumentTypeModelTypeBuilder builder, string defaultTemplate)
        {
            builder.DefaultTemplate = defaultTemplate;

            var modelType = builder.CreateType();

            var model = new DocumentType(modelType);

            model.DefaultTemplate.ShouldBe(defaultTemplate);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetTemplatesFromAttribute(DocumentTypeModelTypeBuilder builder, string[] templates)
        {
            builder.Templates = templates;

            var modelType = builder.CreateType();

            var model = new DocumentType(modelType);

            model.Templates.Any().ShouldBeTrue();
        }

        [Theory]
        [CustomAutoData]
        public void CanGetProperties(DocumentType model)
        {
            model.Properties.ShouldNotBeNull();
        }

        [Theory]
        [CustomInlineAutoData(typeof(string))]
        [CustomInlineAutoData(typeof(int))]
        [CustomInlineAutoData(typeof(decimal))]
        [CustomInlineAutoData(typeof(float))]
        [CustomInlineAutoData(typeof(DateTime))]
        [CustomInlineAutoData(typeof(bool))]
        public void CanGetPublicProperty(Type propertyType, string propertyName, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new DocumentType(modelType);

            var property = model.Properties.Single(p => p.Name == propertyName);

            property.Type.ShouldBe(propertyType);
        }

        [Theory]
        [CustomInlineAutoData(typeof(string))]
        [CustomInlineAutoData(typeof(int))]
        [CustomInlineAutoData(typeof(decimal))]
        [CustomInlineAutoData(typeof(float))]
        [CustomInlineAutoData(typeof(DateTime))]
        [CustomInlineAutoData(typeof(bool))]
        public void CanNotGetPrivateProperty(Type propertyType, string propertyName, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPrivateProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new DocumentType(modelType);

            var property = model.Properties.SingleOrDefault(p => p.Name == propertyName);

            property.ShouldBeNull();
        }

        [Theory]
        [CustomInlineAutoData(typeof(string))]
        [CustomInlineAutoData(typeof(int))]
        [CustomInlineAutoData(typeof(decimal))]
        [CustomInlineAutoData(typeof(float))]
        [CustomInlineAutoData(typeof(DateTime))]
        [CustomInlineAutoData(typeof(bool))]
        public void CanNotGetPublicReadOnlyProperty(Type propertyType, string propertyName, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicReadOnlyProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new DocumentType(modelType);

            var property = model.Properties.SingleOrDefault(p => p.Name == propertyName);

            property.ShouldBeNull();
        }

        [Theory]
        [CustomInlineAutoData(typeof(string))]
        [CustomInlineAutoData(typeof(int))]
        [CustomInlineAutoData(typeof(decimal))]
        [CustomInlineAutoData(typeof(float))]
        [CustomInlineAutoData(typeof(DateTime))]
        [CustomInlineAutoData(typeof(bool))]
        public void CanNotGetPublicWriteOnlyProperty(Type propertyType, string propertyName, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicWriteOnlyProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new DocumentType(modelType);

            var property = model.Properties.SingleOrDefault(p => p.Name == propertyName);

            property.ShouldBeNull();
        }
    }
}