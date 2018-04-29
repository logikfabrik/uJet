namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    public class MediaTypeModelTypeBuilder : ComposableContentTypeModelTypeBuilder<MediaTypeAttribute>
    {
        public MediaTypeModelTypeBuilder(string typeName, string name)
            : base(typeName, name)
        {
        }

        public MediaTypeModelTypeBuilder(string typeName, string id, string name)
            : base(typeName, id, name)
        {
        }
    }
}
