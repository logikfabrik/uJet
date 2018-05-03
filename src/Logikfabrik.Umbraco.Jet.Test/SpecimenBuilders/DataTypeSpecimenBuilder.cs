// <copyright file="DataTypeSpecimenBuilder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using Utilities;

    public class DataTypeSpecimenBuilder : SpecimenBuilder<DataType>
    {
        protected override DataType Create(ISpecimenContext context)
        {
            return new DataType(context.Create<DataTypeModelTypeBuilder>().CreateType());
        }
    }
}
