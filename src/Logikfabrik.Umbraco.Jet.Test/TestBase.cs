// <copyright file="TestBase.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Logging;
    using Moq;

    /// <summary>
    /// The <see cref="TestBase" /> class.
    /// </summary>
    public abstract class TestBase
    {
        /// <summary>
        /// Gets the type service mock.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The type service mock.</returns>
        protected static ITypeService GetTypeService(Type type)
        {
            Assembly[] GetAssemblies() => new[] { type.Assembly };

            return new TypeService(new Mock<ILogService>().Object, null);
        }

        protected static Mock<TypeResolver> GetTypeResolverMock(Type type)
        {
            var typeService = GetTypeService(type);

            return new Mock<TypeResolver>(typeService) { CallBase = true };
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>The property name.</returns>
        protected static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }
    }
}