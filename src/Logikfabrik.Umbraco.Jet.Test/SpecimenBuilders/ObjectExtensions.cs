namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    public static class ObjectExtensions
    {
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            var type = obj.GetType();

            // ReSharper disable once PossibleNullReferenceException
            return type.GetProperty(propertyName).GetValue(obj);
        }
    }
}
