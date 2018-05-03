// <copyright file="SpecimenBuilder{T}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using System;
    using AutoFixture.Kernel;

    public abstract class SpecimenBuilder<T> : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;

            if (type == null || type != typeof(T))
            {
                return new NoSpecimen();
            }

            return Create(context);
        }

        protected abstract T Create(ISpecimenContext context);
    }
}
