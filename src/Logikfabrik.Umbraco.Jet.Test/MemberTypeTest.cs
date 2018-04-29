// <copyright file="MemberTypeTest.cs" company="Logikfabrik">
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

    public class MemberTypeTest
    {
        [Theory]
        [AutoData]
        public void CanGetTypeFromAttribute(string typeName, string name)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, name).CreateType();

            var model = new MemberType(modelType);

            model.ModelType.ShouldBe(modelType);
        }

        [Theory]
        [AutoData]
        public void CanGetIdFromAttribute(string typeName, Guid id, string name)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, id.ToString(), name).CreateType();

            var model = new MemberType(modelType);

            model.Id.ShouldBe(id);
        }

        [Theory]
        [AutoData]
        public void CanGetNameFromAttribute(string typeName, string name)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, name).CreateType();

            var model = new MemberType(modelType);

            model.Name.ShouldBe(name);
        }

        [Theory]
        [AutoData]
        public void CanGetDescriptionFromAttribute(string typeName, string name, string description)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, name) { Description = description }.CreateType();

            var model = new MemberType(modelType);

            model.Description.ShouldBe(description);
        }

        [Theory]
        [AutoData]
        public void CanGetIconFromAttribute(string typeName, string name, string icon)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, name) { Icon = icon }.CreateType();

            var model = new MemberType(modelType);

            model.Icon.ShouldBe(icon);
        }

        [Theory]
        [AutoData]
        public void CanGetIsContainerFromAttribute(string typeName, string name, bool isContainer)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, name) { IsContainer = isContainer }.CreateType();

            var model = new MemberType(modelType);

            model.IsContainer.ShouldBe(isContainer);
        }

        [Theory]
        [AutoData]
        public void CanGetAliasFromAttribute(string typeName, string name, string alias)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, name) { Alias = alias }.CreateType();

            var model = new MemberType(modelType);

            model.Alias.ShouldBe(alias);
        }

        [Theory]
        [AutoData]
        public void CanGetProperties(string typeName, string name)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, name).CreateType();

            var model = new MemberType(modelType);

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
            var typeBuilder = new MemberTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            typeBuilder.AddPublicProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new MemberType(modelType);

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
            var typeBuilder = new MemberTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            typeBuilder.AddPrivateProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new MemberType(modelType);

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
            var typeBuilder = new MemberTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            typeBuilder.AddPublicReadOnlyProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new MemberType(modelType);

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
            var typeBuilder = new MemberTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            typeBuilder.AddPublicWriteOnlyProperty(propertyName, propertyType);

            var modelType = typeBuilder.CreateType();

            var model = new MemberType(modelType);

            var property = model.Properties.SingleOrDefault(p => p.Name == propertyName);

            property.ShouldBeNull();
        }
    }
}