// <copyright file="DataTypeModelTypeBuilder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;
    using System.Linq;

    public class DataTypeModelTypeBuilder : ModelTypeBuilder<DataTypeAttribute>
    {
        public DataTypeModelTypeBuilder(string typeName, Type type, string editor)
            : base(typeName)
        {
            Type = type;
            Editor = editor;
        }

        public DataTypeModelTypeBuilder(string typeName, Guid id, Type type, string editor)
            : base(typeName, id)
        {
            Type = type;
            Editor = editor;
        }

        public Type Type { get; }

        public string Editor { get; }

        protected override object[] GetModelAttributeConstructorArguments()
        {
            return base.GetModelAttributeConstructorArguments().Concat(new object[] { Type, Editor }).ToArray();
        }

        protected override string[] GetModelAttributePropertyNames()
        {
            return new string[] { };
        }
    }
}
