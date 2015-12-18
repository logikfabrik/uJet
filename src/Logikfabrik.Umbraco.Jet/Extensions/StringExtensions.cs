// <copyright file="StringExtensions.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Extensions
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
        /// <param name="firstLetterToLower">If set to false (standard is true), the alias is not forced to have an upper chase as first charachter.</param>
        /// <returns>The Umbraco alias.</returns>
        public static string Alias(this string s, bool firstLetterToLower)
        {
            if (s == null)
            {
                return null;
            }

            s = Regex.Replace(s, @"[^a-zA-Z0-9]", string.Empty);

            return firstLetterToLower 
                ? s.Length < 2 ? s.ToLowerInvariant() : string.Concat(char.ToLowerInvariant(s[0]), s.Substring(1))
                : s;
        }

        /// <summary>
        ///     cGets the Umbraco alias for the specified string. This method follows the Umbraco standards by having first character in alias as lower case.
        /// </summary>
        /// <param name="s">The string to get the Umbraco alias for.</param>
        /// <returns>The Umbraco alias.</returns>
        public static string Alias(this string s)
        {
            return Alias(s, true);
        }
    }
}
