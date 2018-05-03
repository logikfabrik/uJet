// <copyright file="MediaTypeModelTypeBuilderSpecimenBuilder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using Utilities;

    public class MediaTypeModelTypeBuilderSpecimenBuilder : SpecimenBuilder<MediaTypeModelTypeBuilder>
    {
        protected override MediaTypeModelTypeBuilder Create(ISpecimenContext context)
        {
            return new MediaTypeModelTypeBuilder(context.Create<string>(), context.Create<string>());
        }
    }
}
