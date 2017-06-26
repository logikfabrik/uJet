namespace Logikfabrik.Umbraco.Jet.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// Extension methods for DataTypeService
    /// </summary>
    internal static class PreValueCollectionExtensions
    {
        /// <summary>
        /// Updates a PreValueCollection and returns the result as a Dictionary. No existing keys are deleted.
        /// </summary>
        /// <param name="preValueCollection">The data type's existing preValues</param>
        /// <param name="newPreValues">The preValues to add or update</param>
        /// <returns>The updated preValues</returns>
        internal static IDictionary<string, PreValue> AddOrUpdate(this PreValueCollection preValueCollection, IDictionary<string, string> newPreValues)
        {
            var prevaluesToUpdate = preValueCollection.FormatAsDictionary();

            if (newPreValues == null || !newPreValues.Any())
            {
                return prevaluesToUpdate;
            }

            foreach (var p in newPreValues)
            {
                if (prevaluesToUpdate.ContainsKey(p.Key))
                {
                    prevaluesToUpdate[p.Key].Value = p.Value;
                }
                else
                {
                    prevaluesToUpdate[p.Key] = new PreValue(p.Value);
                }
            }

            return prevaluesToUpdate;
        }
    }
}
