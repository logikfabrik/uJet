// <copyright file="TypeExtensionsTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Extensions
{
    using System;
    using AutoFixture.Xunit2;
    using Jet.Extensions;
    using Shouldly;
    using Utilities;
    using Xunit;

    public class TypeExtensionsTest
    {
        [Theory]
        [AutoData]
        public void IsDocumentType(string typeName, string name)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name).CreateType();

            modelType.IsModelType<DocumentTypeAttribute>().ShouldBeTrue();
        }

        [Fact]
        public void IsNotDocumentType()
        {
            typeof(object).IsModelType<DocumentTypeAttribute>().ShouldBeFalse();
        }

        [Theory]
        [AutoData]
        public void IsMediaType(string typeName, string name)
        {
            var modelType = new MediaTypeModelTypeBuilder(typeName, name).CreateType();

            modelType.IsModelType<MediaTypeAttribute>().ShouldBeTrue();
        }

        [Fact]
        public void IsNotMediaType()
        {
            typeof(object).IsModelType<MediaTypeAttribute>().ShouldBeFalse();
        }

        [Theory]
        [AutoData]
        public void IsMemberType(string typeName, string name)
        {
            var modelType = new MemberTypeModelTypeBuilder(typeName, name).CreateType();

            modelType.IsModelType<MemberTypeAttribute>().ShouldBeTrue();
        }

        [Fact]
        public void IsNotMemberType()
        {
            typeof(object).IsModelType<MemberTypeAttribute>().ShouldBeFalse();
        }

        [Theory]
        [AutoData]
        public void IsDataType(string typeName, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, type, editor).CreateType();

            modelType.IsModelType<DataTypeAttribute>().ShouldBeTrue();
        }

        [Fact]
        public void IsNotDataType()
        {
            typeof(object).IsModelType<DataTypeAttribute>().ShouldBeFalse();
        }
    }
}