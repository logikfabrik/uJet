// <copyright file="StringExtensions.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Extensions
{
    /// <summary>
    /// The <see cref="StringExtensions" /> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the Umbraco alias.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>The Umbraco alias.</returns>
        public static string Alias(this string s)
        {
            if (s == null)
            {
                return null;
            }

            s = s.Replace(" ", string.Empty);

            return s.Length < 2 ? s.ToLowerInvariant() : string.Concat(char.ToLowerInvariant(s[0]), s.Substring(1));
        }
    }
}
