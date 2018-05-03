// <copyright file="TypeModelComparerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using Shouldly;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class TypeModelComparerTest
    {
        [Theory]
        [CustomAutoData]
        public void CanCompareDocumentTypesAsEqual(DocumentTypeModelTypeBuilder builder)
        {
            var modelType = builder.CreateType();

            var modelX = new DocumentType(modelType);
            var modelY = new DocumentType(modelType);

            var comparer = new TypeModelComparer<DocumentType, DocumentTypeAttribute>();

            comparer.Equals(modelX, modelY).ShouldBeTrue();
        }

        [Theory]
        [CustomAutoData]
        public void CanNotCompareDocumentTypesAsEqual(DocumentTypeModelTypeBuilder builder)
        {
            var modelTypeX = builder.CreateType();
            var modelTypeY = builder.CreateType();

            var modelX = new DocumentType(modelTypeX);
            var modelY = new DocumentType(modelTypeY);

            var comparer = new TypeModelComparer<DocumentType, DocumentTypeAttribute>();

            comparer.Equals(modelX, modelY).ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void CanCompareMediaTypesAsEqual(MediaTypeModelTypeBuilder builder)
        {
            var modelType = builder.CreateType();

            var modelX = new MediaType(modelType);
            var modelY = new MediaType(modelType);

            var comparer = new TypeModelComparer<MediaType, MediaTypeAttribute>();

            comparer.Equals(modelX, modelY).ShouldBeTrue();
        }

        [Theory]
        [CustomAutoData]
        public void CanNotCompareMediaTypesAsEqual(MediaTypeModelTypeBuilder builder)
        {
            var modelTypeX = builder.CreateType();
            var modelTypeY = builder.CreateType();

            var modelX = new MediaType(modelTypeX);
            var modelY = new MediaType(modelTypeY);

            var comparer = new TypeModelComparer<MediaType, MediaTypeAttribute>();

            comparer.Equals(modelX, modelY).ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void CanCompareMemberTypesAsEqual(MemberTypeModelTypeBuilder builder)
        {
            var modelType = builder.CreateType();

            var modelX = new MemberType(modelType);
            var modelY = new MemberType(modelType);

            var comparer = new TypeModelComparer<MemberType, MemberTypeAttribute>();

            comparer.Equals(modelX, modelY).ShouldBeTrue();
        }

        [Theory]
        [CustomAutoData]
        public void CanNotCompareMemberTypesAsEqual(MemberTypeModelTypeBuilder builder)
        {
            var modelTypeX = builder.CreateType();
            var modelTypeY = builder.CreateType();

            var modelX = new MemberType(modelTypeX);
            var modelY = new MemberType(modelTypeY);

            var comparer = new TypeModelComparer<MemberType, MemberTypeAttribute>();

            comparer.Equals(modelX, modelY).ShouldBeFalse();
        }

        [Theory]
        [CustomAutoData]
        public void CanCompareDataTypesAsEqual(DataTypeModelTypeBuilder builder)
        {
            var modelType = builder.CreateType();

            var modelX = new DataType(modelType);
            var modelY = new DataType(modelType);

            var comparer = new TypeModelComparer<DataType, DataTypeAttribute>();

            comparer.Equals(modelX, modelY).ShouldBeTrue();
        }

        [Theory]
        [CustomAutoData]
        public void CanNotCompareDataTypesAsEqual(DataTypeModelTypeBuilder builder)
        {
            var modelTypeX = builder.CreateType();
            var modelTypeY = builder.CreateType();

            var modelX = new DataType(modelTypeX);
            var modelY = new DataType(modelTypeY);

            var comparer = new TypeModelComparer<DataType, DataTypeAttribute>();

            comparer.Equals(modelX, modelY).ShouldBeFalse();
        }
    }
}