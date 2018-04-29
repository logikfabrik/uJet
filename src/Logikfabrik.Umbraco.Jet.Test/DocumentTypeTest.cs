// <copyright file="DocumentTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Linq;
    using AutoFixture.Xunit2;
    using Shouldly;
    using Utilities;
    using Xunit;

    public class DocumentTypeTest
    {
        [Theory]
        [AutoData]
        public void CanGetTypeFromAttribute(string typeName, string name)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name).CreateType();

            var model = new DocumentType(modelType);

            model.ModelType.ShouldBe(modelType);
        }

        [Theory]
        [AutoData]
        public void CanGetIdFromAttribute(string typeName, Guid id, string name)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, id.ToString(), name).CreateType();

            var model = new DocumentType(modelType);

            model.Id.ShouldBe(id);
        }

        [Theory]
        [AutoData]
        public void CanGetNameFromAttribute(string typeName, string name)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name).CreateType();

            var model = new DocumentType(modelType);

            model.Name.ShouldBe(name);
        }

        [Theory]
        [AutoData]
        public void CanGetDescriptionFromAttribute(string typeName, string name, string description)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name) { Description = description }.CreateType();

            var model = new DocumentType(modelType);

            model.Description.ShouldBe(description);
        }

        [Theory]
        [AutoData]
        public void CanGetIconFromAttribute(string typeName, string name, string icon)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name) { Icon = icon }.CreateType();

            var model = new DocumentType(modelType);

            model.Icon.ShouldBe(icon);
        }

        [Theory]
        [AutoData]
        public void CanGetIsContainerFromAttribute(string typeName, string name, bool isContainer)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name) { IsContainer = isContainer }.CreateType();

            var model = new DocumentType(modelType);

            model.IsContainer.ShouldBe(isContainer);
        }

        [Theory]
        [AutoData]
        public void CanGetAliasFromAttribute(string typeName, string name, string alias)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name) { Alias = alias }.CreateType();

            var model = new DocumentType(modelType);

            model.Alias.ShouldBe(alias);
        }

        [Theory]
        [AutoData]
        public void CanGetThumbnailFromAttribute(string typeName, string name, string thumbnail)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name) { Thumbnail = thumbnail }.CreateType();

            var model = new DocumentType(modelType);

            model.Thumbnail.ShouldBe(thumbnail);
        }

        [Theory]
        [AutoData]
        public void CanGetAllowedAsRootFromAttribute(string typeName, string name, bool allowedAsRoot)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name) { AllowedAsRoot = allowedAsRoot }.CreateType();

            var model = new DocumentType(modelType);

            model.AllowedAsRoot.ShouldBe(allowedAsRoot);
        }

        [Theory]
        [AutoData]
        public void CanGetAllowedChildNodeTypesFromAttribute(string typeName, string name, Type[] allowedChildNodeTypes)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name) { AllowedChildNodeTypes = allowedChildNodeTypes }.CreateType();

            var model = new DocumentType(modelType);

            model.AllowedChildNodeTypes.Any().ShouldBeTrue();
        }

        [Theory]
        [AutoData]
        public void CanGetCompositionNodeTypesFromAttribute(string typeName, string name, Type[] compositionNodeTypes)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name) { CompositionNodeTypes = compositionNodeTypes }.CreateType();

            var model = new DocumentType(modelType);

            model.CompositionNodeTypes.Any().ShouldBeTrue();
        }

        [Theory]
        [AutoData]
        public void CanGetDefaultTemplateFromAttribute(string typeName, string name, string defaultTemplate)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name) { DefaultTemplate = defaultTemplate }.CreateType();

            var model = new DocumentType(modelType);

            model.DefaultTemplate.ShouldBe(defaultTemplate);
        }

        [Theory]
        [AutoData]
        public void CanGetTemplatesFromAttribute(string typeName, string name, string[] templates)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name) { Templates = templates }.CreateType();

            var model = new DocumentType(modelType);

            model.Templates.Any().ShouldBeTrue();
        }

        [Theory]
        [AutoData]
        public void CanGetProperties(string typeName, string name)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name).CreateType();

            var model = new DocumentType(modelType);

            model.Properties.ShouldNotBeNull();
        }

        [Theory]
        [InlineAutoData(typeof(string))]
        [InlineAutoData(typeof(int))]
        [InlineAutoData(typeof(decimal))]
        [InlineAutoData(typeof(float))]
        [InlineAutoData(typeof(DateTime))]
        [InlineAutoData(typeof(bool))]
        public void CanGetPublicProperty(Type propertyType, string propertyName, string typeName, string name)
        {
            var typeBuilder = new DocumentTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            typeBuilder.AddPublicProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new DocumentType(modelType);

            var property = model.Properties.Single(p => p.Name == propertyName);

            property.Type.ShouldBe(propertyType);
        }

        [Theory]
        [InlineAutoData(typeof(string))]
        [InlineAutoData(typeof(int))]
        [InlineAutoData(typeof(decimal))]
        [InlineAutoData(typeof(float))]
        [InlineAutoData(typeof(DateTime))]
        [InlineAutoData(typeof(bool))]
        public void CanNotGetPrivateProperty(Type propertyType, string propertyName, string typeName, string name)
        {
            var typeBuilder = new DocumentTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            typeBuilder.AddPrivateProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new DocumentType(modelType);

            var property = model.Properties.SingleOrDefault(p => p.Name == propertyName);

            property.ShouldBeNull();
        }

        [Theory]
        [InlineAutoData(typeof(string))]
        [InlineAutoData(typeof(int))]
        [InlineAutoData(typeof(decimal))]
        [InlineAutoData(typeof(float))]
        [InlineAutoData(typeof(DateTime))]
        [InlineAutoData(typeof(bool))]
        public void CanNotGetPublicReadOnlyProperty(Type propertyType, string propertyName, string typeName, string name)
        {
            var typeBuilder = new DocumentTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            typeBuilder.AddPublicReadOnlyProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new DocumentType(modelType);

            var property = model.Properties.SingleOrDefault(p => p.Name == propertyName);

            property.ShouldBeNull();
        }

        [Theory]
        [InlineAutoData(typeof(string))]
        [InlineAutoData(typeof(int))]
        [InlineAutoData(typeof(decimal))]
        [InlineAutoData(typeof(float))]
        [InlineAutoData(typeof(DateTime))]
        [InlineAutoData(typeof(bool))]
        public void CanNotGetPublicWriteOnlyProperty(Type propertyType, string propertyName, string typeName, string name)
        {
            var typeBuilder = new DocumentTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            typeBuilder.AddPublicWriteOnlyProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new DocumentType(modelType);

            var property = model.Properties.SingleOrDefault(p => p.Name == propertyName);

            property.ShouldBeNull();
        }
    }
}