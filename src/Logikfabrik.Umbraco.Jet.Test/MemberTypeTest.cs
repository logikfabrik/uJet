// <copyright file="MemberTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection.Emit;
    using Shouldly;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class MemberTypeTest
    {
        [Theory]
        [CustomAutoData]
        public void CanGetTypeFromAttribute(MemberTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Public);

            var model = new MemberType(modelType);

            model.ModelType.ShouldBe(modelType);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetIdFromAttribute(string typeName, Guid id, string name)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, id, name).Create(Scope.Public);

            var model = new MemberType(modelType);

            model.Id.ShouldBe(id);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetNameFromAttribute(MemberTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Public);

            var model = new MemberType(modelType);

            model.Name.ShouldBe(builder.Name);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetDescriptionFromAttribute(MemberTypeModelTypeBuilder builder, string description)
        {
            builder.Description = description;

            var modelType = builder.Create(Scope.Public);

            var model = new MemberType(modelType);

            model.Description.ShouldBe(description);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetIconFromAttribute(MemberTypeModelTypeBuilder builder, string icon)
        {
            builder.Icon = icon;

            var modelType = builder.Create(Scope.Public);

            var model = new MemberType(modelType);

            model.Icon.ShouldBe(icon);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetIsContainerFromAttribute(MemberTypeModelTypeBuilder builder, bool isContainer)
        {
            builder.IsContainer = isContainer;

            var modelType = builder.Create(Scope.Public);

            var model = new MemberType(modelType);

            model.IsContainer.ShouldBe(isContainer);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetAliasFromAttribute(MemberTypeModelTypeBuilder builder, string alias)
        {
            builder.Alias = alias;

            var modelType = builder.Create(Scope.Public);

            var model = new MemberType(modelType);

            model.Alias.ShouldBe(alias);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetProperties(MemberType model)
        {
            model.Properties.ShouldNotBeNull();
        }

        [Theory]
        [ClassAutoData(typeof(ModelPropertyClassData))]
        public void CanGetPublicProperty(Type propertyType, string propertyName, MemberTypeModelTypeBuilder builder)
        {
            builder.AddProperty(Scope.Public, Accessors.GetSet, propertyName, propertyType);

            var modelType = builder.Create(Scope.Public);

            var model = new MemberType(modelType);

            var property = model.Properties.Single(p => p.Name == propertyName);

            property.Type.ShouldBe(propertyType);
        }

        [Theory]
        [ClassAutoData(typeof(ModelPropertyClassData))]
        public void CanNotGetPublicPropertyWithScaffoldingDisabled(Type propertyType, string propertyName, MemberTypeModelTypeBuilder builder)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            builder.AddProperty(Scope.Public, Accessors.GetSet, propertyName, propertyType, new[] { new CustomAttributeBuilder(typeof(ScaffoldColumnAttribute).GetConstructor(new[] { typeof(bool) }), new object[] { false }) });

            var modelType = builder.Create(Scope.Public);

            var model = new MemberType(modelType);

            var property = model.Properties.SingleOrDefault(p => p.Name == propertyName);

            property.ShouldBeNull();
        }

        [Theory]
        [ClassAutoData(typeof(ModelPropertyClassData))]
        public void CanNotGetPrivateProperty(Type propertyType, string propertyName, MemberTypeModelTypeBuilder builder)
        {
            builder.AddProperty(Scope.Private, Accessors.GetSet, propertyName, propertyType);

            var modelType = builder.Create(Scope.Public);

            var model = new MemberType(modelType);

            var property = model.Properties.SingleOrDefault(p => p.Name == propertyName);

            property.ShouldBeNull();
        }

        [Theory]
        [ClassAutoData(typeof(ModelPropertyClassData))]
        public void CanNotGetPublicReadOnlyProperty(Type propertyType, string propertyName, MemberTypeModelTypeBuilder builder)
        {
            builder.AddProperty(Scope.Public, Accessors.Get, propertyName, propertyType);

            var modelType = builder.Create(Scope.Public);

            var model = new MemberType(modelType);

            var property = model.Properties.SingleOrDefault(p => p.Name == propertyName);

            property.ShouldBeNull();
        }

        [Theory]
        [ClassAutoData(typeof(ModelPropertyClassData))]
        public void CanNotGetPublicWriteOnlyProperty(Type propertyType, string propertyName, MemberTypeModelTypeBuilder builder)
        {
            builder.AddProperty(Scope.Public, Accessors.Set, propertyName, propertyType);

            var modelType = builder.Create(Scope.Public);

            var model = new MemberType(modelType);

            var property = model.Properties.SingleOrDefault(p => p.Name == propertyName);

            property.ShouldBeNull();
        }
    }
}