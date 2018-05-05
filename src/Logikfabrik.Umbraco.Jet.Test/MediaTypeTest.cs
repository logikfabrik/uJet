// <copyright file="MediaTypeTest.cs" company="Logikfabrik">
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

    public class MediaTypeTest
    {
        [Theory]
        [CustomAutoData]
        public void CanGetTypeFromAttribute(MediaTypeModelTypeBuilder builder)
        {
            var modelType = builder.CreateType();

            var model = new MediaType(modelType);

            model.ModelType.ShouldBe(modelType);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetIdFromAttribute(string typeName, Guid id, string name)
        {
            var modelType = new MediaTypeModelTypeBuilder(typeName, id, name).CreateType();

            var model = new MediaType(modelType);

            model.Id.ShouldBe(id);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetNameFromAttribute(MediaTypeModelTypeBuilder builder)
        {
            var modelType = builder.CreateType();

            var model = new MediaType(modelType);

            model.Name.ShouldBe(builder.Name);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetDescriptionFromAttribute(MediaTypeModelTypeBuilder builder, string description)
        {
            builder.Description = description;

            var modelType = builder.CreateType();

            var model = new MediaType(modelType);

            model.Description.ShouldBe(description);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetIconFromAttribute(MediaTypeModelTypeBuilder builder, string icon)
        {
            builder.Icon = icon;

            var modelType = builder.CreateType();

            var model = new MediaType(modelType);

            model.Icon.ShouldBe(icon);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetIsContainerFromAttribute(MediaTypeModelTypeBuilder builder, bool isContainer)
        {
            builder.IsContainer = isContainer;

            var modelType = builder.CreateType();

            var model = new MediaType(modelType);

            model.IsContainer.ShouldBe(isContainer);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetAliasFromAttribute(MediaTypeModelTypeBuilder builder, string alias)
        {
            builder.Alias = alias;

            var modelType = builder.CreateType();

            var model = new MediaType(modelType);

            model.Alias.ShouldBe(alias);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetThumbnailFromAttribute(MediaTypeModelTypeBuilder builder, string thumbnail)
        {
            builder.Thumbnail = thumbnail;

            var modelType = builder.CreateType();

            var model = new MediaType(modelType);

            model.Thumbnail.ShouldBe(thumbnail);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetAllowedAsRootFromAttribute(MediaTypeModelTypeBuilder builder, bool allowedAsRoot)
        {
            builder.AllowedAsRoot = allowedAsRoot;

            var modelType = builder.CreateType();

            var model = new MediaType(modelType);

            model.AllowedAsRoot.ShouldBe(allowedAsRoot);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetAllowedChildNodeTypesFromAttribute(MediaTypeModelTypeBuilder builder, Type[] allowedChildNodeTypes)
        {
            builder.AllowedChildNodeTypes = allowedChildNodeTypes;

            var modelType = builder.CreateType();

            var model = new MediaType(modelType);

            model.AllowedChildNodeTypes.Any().ShouldBeTrue();
        }

        [Theory]
        [CustomAutoData]
        public void CanGetCompositionNodeTypesFromAttribute(MediaTypeModelTypeBuilder builder, Type[] compositionNodeTypes)
        {
            builder.CompositionNodeTypes = compositionNodeTypes;

            var modelType = builder.CreateType();

            var model = new MediaType(modelType);

            model.CompositionNodeTypes.Any().ShouldBeTrue();
        }

        [Theory]
        [CustomAutoData]
        public void CanGetProperties(MediaType model)
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
        public void CanGetPublicProperty(Type propertyType, string propertyName, MediaTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new MediaType(modelType);

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
        public void CanNotGetPrivateProperty(Type propertyType, string propertyName, MediaTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPrivateProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new MediaType(modelType);

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
        public void CanNotGetPublicReadOnlyProperty(Type propertyType, string propertyName, MediaTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicReadOnlyProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new MediaType(modelType);

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
        public void CanNotGetPublicWriteOnlyProperty(Type propertyType, string propertyName, MediaTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicWriteOnlyProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new MediaType(modelType);

            var property = model.Properties.SingleOrDefault(p => p.Name == propertyName);

            property.ShouldBeNull();
        }
    }
}