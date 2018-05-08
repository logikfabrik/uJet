// <copyright file="MediaTypeSpecimenBuilder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using Utilities;

    public class MediaTypeSpecimenBuilder : SpecimenBuilder<MediaType>
    {
        protected override MediaType Create(ISpecimenContext context)
        {
            return new MediaType(context.Create<MediaTypeModelTypeBuilder>().Create(Scope.Public));
        }
    }
}
