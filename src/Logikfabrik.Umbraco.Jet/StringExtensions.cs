// <copyright file="StringExtensions.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// The <see cref="StringExtensions" /> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the Umbraco alias for the specified string.
        /// </summary>
        /// <param name="s">The string to get the Umbraco alias for.</param>
        /// <returns>The Umbraco alias.</returns>
        public static string Alias(this string s)
        {
            if (s == null)
            {
                return null;
            }

            s = Regex.Replace(s, @"[^a-zA-Z0-9]", string.Empty);

            return s.Length < 2 ? s.ToLowerInvariant() : string.Concat(char.ToLowerInvariant(s[0]), s.Substring(1));
        }
    }
}
