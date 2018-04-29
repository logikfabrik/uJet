namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System.Linq;

    public abstract class ContentTypeModelTypeBuilder<T> : ModelTypeBuilder<T>
        where T : ContentTypeModelAttribute
    {
        private readonly string _name;

        protected ContentTypeModelTypeBuilder(string typeName, string name)
            : base(typeName)
        {
            _name = name;
        }

        protected ContentTypeModelTypeBuilder(string typeName, string id, string name)
            : base(typeName, id)
        {
            _name = name;
        }

        public string Description { get; set; }

        public string Icon { get; set; }

        public bool IsContainer { get; set; }

        public string Alias { get; set; }

        protected override object[] GetAttributeConstructorArguments()
        {
            return base.GetAttributeConstructorArguments().Concat(new[] { _name }).ToArray();
        }

        protected override string[] GetAttributePropertyNames()
        {
            return new[]
            {
                GetPropertyName(() => Description),
                GetPropertyName(() => Icon),
                GetPropertyName(() => IsContainer),
                GetPropertyName(() => Alias)
            };
        }
    }
}
