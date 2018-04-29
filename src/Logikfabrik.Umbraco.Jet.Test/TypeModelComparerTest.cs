// <copyright file="TypeModelComparerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using AutoFixture.Xunit2;
    using Shouldly;
    using Utilities;
    using Xunit;

    public class TypeModelComparerTest
    {
        [Theory]
        [AutoData]
        public void CanCompareTypeModelsAsEqual(string typeName, string name)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, name).CreateType();

            var typeModelX = new DocumentType(modelType);
            var typeModelY = new DocumentType(modelType);

            var comparer = new TypeModelComparer<DocumentType, DocumentTypeAttribute>();

            comparer.Equals(typeModelX, typeModelY).ShouldBeTrue();
        }

        [Theory]
        [AutoData]
        public void CanNotCompareTypeModelsAsEqual(string typeName, string name)
        {
            var modelTypeX = new DocumentTypeModelTypeBuilder(typeName, name).CreateType();
            var modelTypeY = new DocumentTypeModelTypeBuilder(typeName, name).CreateType();

            var typeModelX = new DocumentType(modelTypeX);
            var typeModelY = new DocumentType(modelTypeY);

            var comparer = new TypeModelComparer<DocumentType, DocumentTypeAttribute>();

            comparer.Equals(typeModelX, typeModelY).ShouldBeFalse();
        }
    }
}