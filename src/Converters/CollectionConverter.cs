namespace Collective
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Converts the members of a collection from one type to another on demand.
    /// </summary>
    public class CollectionConverter<TOriginal, TConverted> : ICollection<TConverted>
    {
        private ICollection<TOriginal> collection;
        private Func<TOriginal, TConverted> converter;

        /// <summary>
        /// Initializes a new instance of the CollectionConverter class.
        /// </summary>
        /// <param name="collection">The original collection.</param>
        public CollectionConverter(ICollection<TOriginal> collection, Func<TOriginal, TConverted> converter)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }

            this.collection = collection;
            this.converter = converter;
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.collection.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly 
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<TConverted> GetEnumerator()
        {
            foreach (TOriginal item in this.collection)
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

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item">The item to locate in the collection.</param>
        /// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(TConverted item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The object to add to the collection.</param>
        public void Add(TConverted item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection.
        /// </summary>
        /// <param name="item">The object to remove from the collection.</param>
        /// <returns>true if item was successfully removed from the collection; otherwise, false.</returns>
        public bool Remove(TConverted item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Copies the elements of the collection to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements
        /// copied from collection. The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(TConverted[] array, int arrayIndex)
        {
            int i = arrayIndex;

            foreach (TOriginal item in this.collection)
            {
                if (i >= array.Length)
                {
                    break;
                }

                array[i] = this.converter(item);
            }
        }
    }
}