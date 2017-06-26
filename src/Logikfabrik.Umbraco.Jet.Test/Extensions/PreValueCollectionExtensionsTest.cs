namespace Logikfabrik.Umbraco.Jet.Test.Extensions
{
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Jet.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class PreValueCollectionExtensionsTest : TestBase
    {
        [TestMethod]
        public void ReturnsUpdatedPreValues()
        {
            var preValueCollection = new PreValueCollection(new Dictionary<string, PreValue>
            {
                { "a", new PreValue(1, "valueA") },
                { "b", new PreValue(2, "valueB") },
                { "c", new PreValue(3, "valueC") }
            });

            var newPreValues = new Dictionary<string, string>
            {
                { "c", "valueCNew" },
                { "d", "valueD" }
            };

            var updatedPreValues = preValueCollection.AddOrUpdate(newPreValues);

            Assert.IsTrue(updatedPreValues.ContainsKey("a")); // existing not in newPrevalues is not removed
            Assert.IsTrue(updatedPreValues["c"].Value == "valueCNew"); // existing has updated value
            Assert.IsTrue(updatedPreValues.ContainsKey("d")); // new preValue is added
        }
    }
}
