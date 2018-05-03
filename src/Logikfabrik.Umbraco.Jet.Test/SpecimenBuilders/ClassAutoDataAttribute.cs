// <copyright file="ClassAutoDataAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Xunit;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method)]
    public class ClassAutoDataAttribute : DataAttribute
    {
        public ClassAutoDataAttribute(Type @class)
        {
            ClassDataAttribute = new ClassDataAttribute(@class);
            AutoDataAttribute = new CustomAutoDataAttribute();
        }

        public DataAttribute ClassDataAttribute { get; }

        public DataAttribute AutoDataAttribute { get; }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var autoData = AutoDataAttribute.GetData(testMethod).Single();

            return ClassDataAttribute.GetData(testMethod).Select(classData => classData.Concat(autoData.Skip(classData.Length)).ToArray());
        }
    }
}
