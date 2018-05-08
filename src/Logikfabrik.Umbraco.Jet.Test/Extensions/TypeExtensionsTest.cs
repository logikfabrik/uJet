// <copyright file="TypeExtensionsTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Extensions
{
    using Jet.Extensions;
    using Shouldly;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class TypeExtensionsTest
    {
        [Theory]
        [CustomAutoData]
        public void IsDocumentType(DocumentTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Public);

            modelType.IsModelType<DocumentTypeAttribute>().ShouldBeTrue();
        }

        [Fact]
        public void IsNotDocumentType()
        {
            typeof(object).IsModelType<DocumentTypeAttribute>().ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void IsMediaType(MediaTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Public);

            modelType.IsModelType<MediaTypeAttribute>().ShouldBeTrue();
        }

        [Fact]
        public void IsNotMediaType()
        {
            typeof(object).IsModelType<MediaTypeAttribute>().ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void IsMemberType(MemberTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Public);

            modelType.IsModelType<MemberTypeAttribute>().ShouldBeTrue();
        }

        [Fact]
        public void IsNotMemberType()
        {
            typeof(object).IsModelType<MemberTypeAttribute>().ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void IsDataType(DataTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Public);

            modelType.IsModelType<DataTypeAttribute>().ShouldBeTrue();
        }

        [Fact]
        public void IsNotDataType()
        {
            typeof(object).IsModelType<DataTypeAttribute>().ShouldBeFalse();
        }
    }
}