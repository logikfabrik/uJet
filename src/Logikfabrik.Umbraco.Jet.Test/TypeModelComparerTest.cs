// <copyright file="TypeModelComparerTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypeModelComparerTest
    {
        [TestMethod]
        public void CanCompareTypeModelsAsEqual()
        {
            var comparer = new TypeModelComparer<DocumentType, DocumentTypeAttribute>();

            var x = new DocumentType(typeof(Models.DocumentTypeA));
            var y = new DocumentType(typeof(Models.DocumentTypeA));

            Assert.IsTrue(comparer.Equals(x, y));
        }

        [TestMethod]
        public void CannotCompareTypeModelsAsEqual()
        {
            var comparer = new TypeModelComparer<DocumentType, DocumentTypeAttribute>();

            var x = new DocumentType(typeof(Models.DocumentTypeA));
            var y = new DocumentType(typeof(Models.DocumentTypeB));

            Assert.IsFalse(comparer.Equals(x, y));
        }
    }
}