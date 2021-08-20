using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TimeDataViewer.Core
{
    public static class StringHelper
    {
        private static readonly Regex FormattingExpression = new Regex("{(?<Property>.+?)(?<Format>\\:.*?)?}");

        // Creates a valid format string on the form "{0:###}".
        public static string CreateValidFormatString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "{0}";
            }

            if (input.Contains("{"))
            {
                return input;
            }

            return string.Concat("{0:", input, "}");
        }

        public static string Format(IFormatProvider provider, string formatString, object item, params object[] values)
        {
            // Replace items on the format {Property[:Formatstring]}
            var s = FormattingExpression.Replace(
                formatString,
                delegate (Match match)
                {
                    var property = match.Groups["Property"].Value;
                    if (property.Length > 0 && char.IsDigit(property[0]))
                    {
                        return match.Value;
                    }

                    var pi = item.GetType().GetRuntimeProperty(property);
                    if (pi == null)
                    {
                        return string.Empty;
                    }

                    var v = pi.GetValue(item, null);
                    var format = match.Groups["Format"].Value;

                    var fs = "{0" + format + "}";
                    return string.Format(provider, fs, v);
                });

            // Also apply the standard formatting
            s = string.Format(provider, s, values);
            return s;
        }

        /// <summary>
        /// Formats each item in a sequence by the specified format string and property.
        /// </summary>
        /// <param name="source">The source target.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="formatString">The format string. The format argument {0} can be used for the value of the property in each element of the sequence.</param>
        /// <param name="provider">The format provider.</param>
        /// <exception cref="System.InvalidOperationException">Could not find property.</exception>
        public static IEnumerable<string> Format(this IEnumerable source, string propertyName, string formatString, IFormatProvider provider)
        {
            var fs = CreateValidFormatString(formatString);
            if (string.IsNullOrEmpty(propertyName))
            {
                foreach (var element in source)
                {
                    yield return string.Format(provider, fs, element);
                }
            }
            else
            {
                var reflectionPath = new ReflectionPath(propertyName);
                foreach (var element in source)
                {
                    var value = reflectionPath.GetValue(element);
                    yield return string.Format(provider, fs, value);
                }
            }
        }
    }
}
