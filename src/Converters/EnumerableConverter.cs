namespace Collective
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Converts the members of an enumerable from one type to another on demand.
    /// </summary>
    public class EnumerableConverter<TOriginal, TConverted> : IEnumerable<TConverted>
    {
        private IEnumerable<TOriginal> enumerable;
        private Func<TOriginal, TConverted> converter;

        /// <summary>
        /// Initializes a new instance of the EnumerableConverter class.
        /// </summary>
        /// <param name="enumerable">The original enumerable.</param>
        public EnumerableConverter(IEnumerable<TOriginal> enumerable, Func<TOriginal, TConverted> converter)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }

            this.enumerable = enumerable;
            this.converter = converter;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<TConverted> GetEnumerator()
        {
            foreach (TOriginal item in this.enumerable)
            {
                yield return this.converter(item);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}