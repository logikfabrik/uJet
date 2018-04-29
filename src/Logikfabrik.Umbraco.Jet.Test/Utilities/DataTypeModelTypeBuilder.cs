namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;
    using System.Linq;

    public class DataTypeModelTypeBuilder : ModelTypeBuilder<DataTypeAttribute>
    {
        private readonly Type _type;
        private readonly string _editor;

        public DataTypeModelTypeBuilder(string typeName, Type type, string editor)
            : base(typeName)
        {
            _type = type;
            _editor = editor;
        }

        public DataTypeModelTypeBuilder(string typeName, string id, Type type, string editor)
            : base(typeName, id)
        {
            _type = type;
            _editor = editor;
        }

        protected override object[] GetAttributeConstructorArguments()
        {
            return base.GetAttributeConstructorArguments().Concat(new object[] { _type, _editor }).ToArray();
        }

        protected override string[] GetAttributePropertyNames()
        {
            return new string[] { };
        }
    }
}
