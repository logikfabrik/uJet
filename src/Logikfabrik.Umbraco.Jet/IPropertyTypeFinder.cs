namespace Logikfabrik.Umbraco.Jet
{
    public interface IPropertyTypeFinder
    {
        /// <summary>
        /// Finds the property types matching the specified model.
        /// </summary>
        /// <param name="modelNeedle">The model to find the property types for.</param>
        /// <param name="propertyTypesHaystack">The haystack of property types.</param>
        /// <returns>The found property types.</returns>
        global::Umbraco.Core.Models.PropertyType[] Find(PropertyTypeModel modelNeedle, global::Umbraco.Core.Models.PropertyType[] propertyTypesHaystack);
    }
}