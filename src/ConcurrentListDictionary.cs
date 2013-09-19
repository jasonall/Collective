namespace Collective
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A <see cref="ListDictionary"/> with built-in thread safety.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public class ConcurrentListDictionary<TKey, TValue> : ListDictionary<TKey, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the ConcurrentListDictionary class.
        /// </summary>
        /// <param name="getHashForItem">A delegate which returns the hash for a particular item.</param>
        /// <param name="capacity">The initial list capacity (optional).</param>
        public ConcurrentListDictionary(Func<TValue, TKey> getHashForItem, int capacity = -1)
            : base(getHashForItem, capacity)
        {
            this.MakeConcurrent();
        }

        /// <summary>
        /// Initializes a new instance of the ConcurrentListDictionary class.
        /// </summary>
        /// <param name="getHashForItem">A delegate which returns the hash for a particular item.</param>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public ConcurrentListDictionary(Func<TValue, TKey> getHashForItem, IEnumerable<TValue> collection)
            : base(getHashForItem, collection)
        {
            this.MakeConcurrent();
        }
    }
}
