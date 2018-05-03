// <copyright file="DataTypeModelTypeBuilderSpecimenBuilder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using System;
    using AutoFixture;
    using AutoFixture.Kernel;
    using Utilities;

    public class DataTypeModelTypeBuilderSpecimenBuilder : SpecimenBuilder<DataTypeModelTypeBuilder>
    {
        protected override DataTypeModelTypeBuilder Create(ISpecimenContext context)
        {
            return new DataTypeModelTypeBuilder(context.Create<string>(), context.Create<Type>(), context.Create<string>());
        }
    }
}
