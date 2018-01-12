using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalStringOrdering
{
    /// <summary>
    /// Extension of the IEnumerable&lt;string&gt; interface for adding natural sorting.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Sorts the elements of a sequence of strings in ascending natural order according to a key.
        /// Sorting is case-insensitive.
        /// </summary>
        /// <param name="strings">A sequence of strings to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>An IOrderedEnumerable&lt;string&gt; whose elements are sorted according to a key.</returns>
        public static IOrderedEnumerable<string> OrderByNatural(this IEnumerable<string> strings, Func<string, string> keySelector)
        {
            return strings.OrderBy(keySelector, new NaturalStringComparer());
        }

        /// <summary>
        /// Sorts the elements of a sequence of strings in ascending natural order according to a key, ignoring or honoring their case.
        /// </summary>
        /// <param name="strings">A sequence of strings to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>An IOrderedEnumerable&lt;string&gt; whose elements are sorted according to a key.</returns>
        public static IOrderedEnumerable<string> OrderByNatural(this IEnumerable<string> strings, Func<string, string> keySelector, bool ignoreCase)
        {
            return strings.OrderBy(keySelector, new NaturalStringComparer(ignoreCase));
        }

        /// <summary>
        /// Sorts the elements of a sequence of strings in descending natural order according to a key.
        /// Sorting is case-insensitive.
        /// </summary>
        /// <param name="strings">A sequence of strings to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>An IOrderedEnumerable&lt;string&gt; whose elements are sorted according to a key.</returns>
        public static IOrderedEnumerable<string> OrderByNaturalDescending(this IEnumerable<string> strings, Func<string, string> keySelector)
        {
            return strings.OrderByDescending(keySelector, new NaturalStringComparer());
        }

        /// <summary>
        /// Sorts the elements of a sequence of strings in descending natural order according to a key, ignoring or honoring their case.
        /// </summary>
        /// <param name="strings">A sequence of strings to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>An IOrderedEnumerable&lt;string&gt; whose elements are sorted according to a key.</returns>
        public static IOrderedEnumerable<string> OrderByNaturalalDescending(this IEnumerable<string> strings, Func<string, string> keySelector, bool ignoreCase)
        {
            return strings.OrderByDescending(keySelector, new NaturalStringComparer(ignoreCase));
        }
    }
}
