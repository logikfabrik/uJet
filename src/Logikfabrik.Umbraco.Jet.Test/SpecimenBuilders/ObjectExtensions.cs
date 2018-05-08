namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    public static class ObjectExtensions
    {
        public static object GetPropertyValue(this object obj, string name)
        {
            var type = obj.GetType();

            // ReSharper disable once PossibleNullReferenceException
            return type.GetProperty(name).GetValue(obj);
        }

        public static void SetPropertyValue(this object obj, string name, object value)
        {
            var type = obj.GetType();

            // ReSharper disable once PossibleNullReferenceException
            type.GetProperty(name).SetValue(obj, value);
        }
    }
}
