namespace Collective
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Converts the members of a list from one type to another on demand.
    /// </summary>
    public class ListConverter<TOriginal, TConverted> : IList<TConverted>
    {
        private IList<TOriginal> list;
        private Func<TOriginal, TConverted> converter;

        /// <summary>
        /// Initializes a new instance of the ListConverter class.
        /// </summary>
        /// <param name="list">The original list.</param>
        public ListConverter(IList<TOriginal> list, Func<TOriginal, TConverted> converter)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }

            this.list = list;
            this.converter = converter;
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.list.Count;
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
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public TConverted this[int index]
        {
            get
            {
                return this.converter(this.list[index]);
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<TConverted> GetEnumerator()
        {
            for (int i = 0; i < this.list.Count; ++i)
            {
                yield return this.converter(this.list[i]);
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
        /// Determines the index of a specific item in the list.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(TConverted item)
        {
            throw new NotSupportedException();
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
        /// Inserts an item to the list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the</param>
        public void Insert(int index, TConverted item)
        {
            throw new NotImplementedException();
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
        /// Removes the list item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
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
            for (int i = arrayIndex; i < array.Length; ++i)
            {
                array[i] = this.converter(this.list[i - arrayIndex]);
            }
        }
    }
}