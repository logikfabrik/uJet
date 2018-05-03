// <copyright file="CustomAutoDataAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using System;
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using AutoFixture.Xunit2;

    [AttributeUsage(AttributeTargets.Method)]
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute()
            : base(() =>
            {
                var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });

                fixture.Customizations.Add(new DataTypeModelTypeBuilderSpecimenBuilder());
                fixture.Customizations.Add(new DataTypeSpecimenBuilder());
                fixture.Customizations.Add(new DocumentTypeModelTypeBuilderSpecimenBuilder());
                fixture.Customizations.Add(new DocumentTypeSpecimenBuilder());
                fixture.Customizations.Add(new MediaTypeModelTypeBuilderSpecimenBuilder());
                fixture.Customizations.Add(new MediaTypeSpecimenBuilder());
                fixture.Customizations.Add(new MemberTypeModelTypeBuilderSpecimenBuilder());
                fixture.Customizations.Add(new MemberTypeSpecimenBuilder());

                return fixture;
            })
        {
        }
    }
}
