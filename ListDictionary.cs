namespace Collective
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A list with a dictionary for fast lookups.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public class ListDictionary<TKey, TValue> : IList<TValue>, IList
    {
        private object l;
        private List<TValue> list;
        private Dictionary<TKey, TValue> hash;
        private Func<TValue, TKey> getHash;

        /// <summary>
        /// Initializes a new instance of the ListDictionary class.
        /// </summary>
        /// <param name="getHashForItem">A delegate which returns the hash for a particular item.</param>
        /// <param name="capacity">The initial list capacity (optional).</param>
        public ListDictionary(Func<TValue, TKey> getHashForItem, int capacity = -1)
        {
            if (getHashForItem == null)
            {
                throw new ArgumentNullException("getHashForItem");
            }

            this.getHash = getHashForItem;

            if (capacity > 0)
            {
                this.list = new List<TValue>(capacity);
                this.hash = new Dictionary<TKey, TValue>(capacity);
            }
            else
            {
                this.list = new List<TValue>();
                this.hash = new Dictionary<TKey, TValue>();
            }
        }

        /// <summary>
        /// Initializes a new instance of the ListDictionary class.
        /// </summary>
        /// <param name="getHashForItem">A delegate which returns the hash for a particular item.</param>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public ListDictionary(Func<TValue, TKey> getHashForItem, IEnumerable<TValue> collection)
        {
            if (getHashForItem == null)
            {
                throw new ArgumentNullException("getHashForItem");
            }

            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            this.getHash = getHashForItem;
            this.list = new List<TValue>(collection);
            this.hash = new Dictionary<TKey, TValue>();

            foreach (TValue item in collection)
            {
                this.hash.Add(getHashForItem(item), item);
            }
        }

        /// <summary>
        /// Gets the internal collection lock.
        /// </summary>
        public object SyncLock
        {
            get
            {
                return this.l;
            }
        }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.RunUnderLock(() =>
                {
                    return this.list.Count;
                });
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the has a fixed size.
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether access to the collection is thread-safe.
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return this.l != null;
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the collection.
        /// </summary>
        object System.Collections.ICollection.SyncRoot
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public TValue this[int index]
        {
            get
            {
                return this.RunUnderLock(() =>
                {
                    return this.list[index];
                });
            }

            set
            {
                this.RunUnderLock(() =>
                {
                    TValue existingItem = this.list[index];
                    if (!object.Equals(existingItem, value))
                    {
                        this.list[index] = value;

                        if (existingItem != null)
                        {
                            this.hash.Remove(this.getHash(existingItem));
                        }

                        if (value != null)
                        {
                            this.hash.Add(this.getHash(value), value);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        object System.Collections.IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                this[index] = (TValue)value;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<TValue> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the list.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <returns>The index of item if found in the list; otherwise, -1;</returns>
        public int IndexOf(TValue item)
        {
            return this.RunUnderLock(() =>
            {
                return this.list.IndexOf(item);
            });
        }

        /// <summary>
        /// Determines the index of a specific item in the collection.
        /// </summary>
        /// <param name="value">The value to locate in the collection.</param>
        /// <returns>The index of item if found in the collection; otherwise, -1;</returns>
        int System.Collections.IList.IndexOf(object value)
        {
            return this.IndexOf((TValue)value);
        }

        /// <summary>
        /// Determines the index of a specific item in the list.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <param name="index">The starting index of the search.</param>
        /// <returns>The index of item if found in the list; otherwise, -1;</returns>
        public int IndexOf(TValue item, int index)
        {
            return this.RunUnderLock(() =>
            {
                return this.list.IndexOf(item, index);
            });
        }

        /// <summary>
        /// Determines the index of a specific item in the list.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <param name="index">The starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The index of item if found in the list; otherwise, -1;</returns>
        public int IndexOf(TValue item, int index, int count)
        {
            return this.RunUnderLock(() =>
            {
                return this.list.IndexOf(item, index, count);
            });
        }

        /// <summary>
        /// Determines the last index of a specific item in the list.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <returns>The index of last item if found in the list; otherwise, -1;</returns>
        public int LastIndexOf(TValue item)
        {
            return this.RunUnderLock(() =>
            {
                return this.list.LastIndexOf(item);
            });
        }

        /// <summary>
        /// Determines the last index of a specific item in the list.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <param name="index">The starting index of the search.</param>
        /// <returns>The index of last item if found in the list; otherwise, -1;</returns>
        public int LastIndexOf(TValue item, int index)
        {
            return this.RunUnderLock(() =>
            {
                return this.list.LastIndexOf(item, index);
            });
        }

        /// <summary>
        /// Determines the last index of a specific item in the list.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <param name="index">The starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The index of last item if found in the list; otherwise, -1;</returns>
        public int LastIndexOf(TValue item, int index, int count)
        {
            return this.RunUnderLock(() =>
            {
                return this.list.LastIndexOf(item, index, count);
            });
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the collection.</param>
        /// <returns>true if the item is found in the collection; otherwise false.</returns>
        public bool Contains(TValue item)
        {
            return this.RunUnderLock(() =>
            {
                return this.hash.ContainsKey(this.getHash(item));
            });
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="value">The value to locate in the collection.</param>
        /// <returns>true if the item is found in the collection; otherwise false.</returns>
        bool System.Collections.IList.Contains(object value)
        {
            return this.Contains((TValue)value);
        }

        /// <summary>
        /// Determines whether the collection contains a value with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the collection.</param>
        /// <returns>true if the item is found in the collection; otherwise false.</returns>
        public bool ContainsKey(TKey key)
        {
            return this.RunUnderLock(() =>
            {
                return this.hash.ContainsKey(key);
            });
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The item to add to the collection.</param>
        public void Add(TValue item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.RunUnderLock(() =>
            {
                this.list.Add(item);
                this.hash.Add(this.getHash(item), item);
            });
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="value">The value to add to the collection.</param>
        /// <returns>Returns the index of the added item.</returns>
        int System.Collections.IList.Add(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return this.RunUnderLock(() =>
            {
                this.Add((TValue)value);
                return this.list.Count - 1;
            });
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the list.
        /// </summary>
        /// <param name="collection">The collection to add.</param>
        public void AddRange(IEnumerable<TValue> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            if (collection != null)
            {
                this.RunUnderLock(() =>
                {
                    this.list.AddRange(collection);
                    foreach (TValue item in collection)
                    {
                        if (item != null)
                        {
                            this.hash.Add(this.getHash(item), item);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Inserts an item to the list at the specified index.
        /// </summary>
        /// <param name="index">The index at which the item should be inserted.</param>
        /// <param name="item">The item to insert.</param>
        public void Insert(int index, TValue item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.RunUnderLock(() =>
            {
                this.list.Insert(index, item);
                this.hash.Add(this.getHash(item), item);
            });
        }

        /// <summary>
        /// Inserts an item to the collection at the specified index.
        /// </summary>
        /// <param name="index">The index at which the item should be inserted.</param>
        /// <param name="value">The value to insert.</param>
        void System.Collections.IList.Insert(int index, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this.Insert(index, (TValue)value);
        }

        /// <summary>
        /// Inserts an item to the list at the specified index.
        /// </summary>
        /// <param name="index">The index at which the item should be inserted.</param>
        /// <param name="collection">The items to insert.</param>
        public void InsertRange(int index, IEnumerable<TValue> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            this.RunUnderLock(() =>
            {
                this.list.InsertRange(index, collection);

                foreach (TValue item in collection)
                {
                    if (item == null)
                    {
                        throw new ArgumentNullException("item", "The collection may not contain null items.");
                    }

                    this.hash.Add(this.getHash(item), item);
                }
            });
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>true if the item was removed; false otherwise.</returns>
        public bool Remove(TValue item)
        {
            if (item == null)
            {
                return false;
            }

            return this.RunUnderLock(() =>
            {
                this.hash.Remove(this.getHash(item));
                return this.list.Remove(item);
            });
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection.
        /// </summary>
        /// <param name="value">The value to remove.</param>
        void System.Collections.IList.Remove(object value)
        {
            this.Remove((TValue)value);
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The predicate.</param>
        /// <returns>Returns the number of elements removed.</returns>
        public int RemoveAll(Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return this.RunUnderLock(() =>
            {
                int removeCount = 0;

                for (int i = this.list.Count - 1; i >= 0; --i)
                {
                    TValue item = this.list[i];
                    if (item != null && match(item))
                    {
                        ++removeCount;
                        this.list.RemoveAt(i);
                        this.hash.Remove(this.getHash(item));
                    }
                }

                return removeCount;
            });
        }

        /// <summary>
        /// Removes the list item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            this.RunUnderLock(() =>
            {
                TValue item = this.list[index];
                this.list.RemoveAt(index);

                if (item != null)
                {
                    this.hash.Remove(this.getHash(item));
                }
            });
        }

        /// <summary>
        /// Removes a range of elements from the list.
        /// </summary>
        /// <param name="index">The starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public void RemoveRange(int index, int count)
        {
            this.RunUnderLock(() =>
            {
                if (index + count >= this.list.Count)
                {
                    throw new ArgumentException("Out of range");
                }

                for (int i = index + count - 1; i >= index; --count)
                {
                    TValue item = this.list[i];
                    this.list.RemoveAt(i);
                    if (item != null)
                    {
                        this.hash.Remove(this.getHash(item));
                    }
                }
            });
        }

        /// <summary>
        /// Sets the values of all items in the list to be equal to the given collection.
        /// </summary>
        /// <param name="items">The items to set.</param>
        public void Set(IList<TValue> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            this.RunUnderLock(() =>
            {
                // First, replace all the items up to the point where both lists end.
                // If the items are the same, nothing will happen.
                for (int i = 0; i < Math.Min(items.Count, this.list.Count); ++i)
                {
                    bool added = false;
                    TValue item = items[i];

                    if (!object.Equals(this.list[i], item))
                    {
                        if (this.hash.ContainsKey(this.getHash(item)))
                        {
                            // An item with the same hash already exists somewhere in the collection. 
                            // If it is in a different index, we'll have to remove it.
                            if (EqualityComparer<TKey>.Default.Equals(this.getHash(this.list[i]), this.getHash(item)))
                            {
                                // The items are different but have the same hash.
                                this.list[i] = item;
                                this.hash[this.getHash(item)] = item;
                                added = true;
                            }
                            else
                            {
                                // The item at this index is totally different.
                                this.hash.Remove(this.getHash(item));
                            }
                        }

                        if (!added)
                        {
                            this.list[i] = item;
                            this.hash.Add(this.getHash(item), item);
                        }
                    }
                }

                if (items.Count > this.list.Count)
                {
                    // Add the rest of the new items.
                    for (int i = this.list.Count; i < items.Count; ++i)
                    {
                        TValue item = items[i];

                        if (item == null)
                        {
                            throw new ArgumentNullException("item");
                        }

                        this.list.Add(item);
                    }
                }
                else if (items.Count < this.list.Count)
                {
                    // Remove all of the extra items.
                    for (int i = items.Count; i < this.list.Count; ++i)
                    {
                        this.list.RemoveAt(items.Count);
                    }
                }
            });
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            this.RunUnderLock(() =>
            {
                this.list.Clear();
                this.hash.Clear();
            });
        }

        /// <summary>
        /// Searches the entire sorted list for an element using the default comparer and returns the index of the element.
        /// </summary>
        /// <param name="item">The object to locate.</param>
        /// <returns>Returns the index of the item if it is found; otherwise a negative number that is the bitwise
        /// complement of the index of the next element that is larger than item or, if there is
        /// no larger element, the bitwise complement of the count.</returns>
        public int BinarySearch(TValue item)
        {
            return this.RunUnderLock(() =>
            {
                return this.list.BinarySearch(item);
            });
        }

        /// <summary>
        /// Searches the entire sorted list for an element using the specified comparer and returns the index of the element.
        /// </summary>
        /// <param name="item">The object to locate.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>Returns the index of the item if it is found; otherwise a negative number that is the bitwise
        /// complement of the index of the next element that is larger than item or, if there is
        /// no larger element, the bitwise complement of the count.</returns>
        public int BinarySearch(TValue item, IComparer<TValue> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.BinarySearch(item, comparer);
            });
        }

        /// <summary>
        /// Searches a range of elements in the list for an element using the specified comparer and returns the index of the element.
        /// </summary>
        /// <param name="index">The starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="item">The object to locate.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>Returns the index of the item if it is found; otherwise a negative number that is the bitwise
        /// complement of the index of the next element that is larger than item or, if there is
        /// no larger element, the bitwise complement of the count.</returns>
        public int BinarySearch(int index, int count, TValue item, IComparer<TValue> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.BinarySearch(index, count, item, comparer);
            });
        }

        /// <summary>
        /// Copies the elements of the collection to an array, starting at the beginning of the array.
        /// </summary>
        /// <param name="array">The array that is the destination of the elements.</param>
        public void CopyTo(TValue[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            this.RunUnderLock(() =>
            {
                this.list.CopyTo(array);
            });
        }

        /// <summary>
        /// Copies the elements of the collection to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">The array that is the destination of the elements.</param>
        /// <param name="arrayIndex">The index in the array at which copying begins.</param>
        public void CopyTo(TValue[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            this.RunUnderLock(() =>
            {
                this.list.CopyTo(array, arrayIndex);
            });
        }

        /// <summary>
        /// Copies the elements of the collection to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">The array that is the destination of the elements.</param>
        /// <param name="index">The index in the array at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            this.RunUnderLock(() =>
            {
                ((IList)this.list).CopyTo(array, index);
            });
        }

        /// <summary>
        /// Copies the elements of the collection to an array, starting at a particular array index.
        /// </summary>
        /// <param name="index">The index at which copying begins.</param>
        /// <param name="array">The array that is the destination of the elements.</param>
        /// <param name="arrayIndex">The index in the array at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        public void CopyTo(int index, TValue[] array, int arrayIndex, int count)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            RunUnderLock(() =>
            {
                this.list.CopyTo(index, array, arrayIndex, count);
            });
        }

        /// <summary>
        /// Determines whether the list contains elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The predicate delegate.</param>
        /// <returns>Returns true if the list contains one or more elements that match; false otherwise.</returns>
        public bool Exists(Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.Exists(match);
            });
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The predicate delegate.</param>
        /// <returns>Returns the first matching item, or null.</returns>
        public TValue Find(Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.Find(match);
            });
        }

        /// <summary>
        /// Retrieves all elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The predicate delegate.</param>
        /// <returns>Returns the list of matching elements.</returns>
        public IList<TValue> FindAll(Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.FindAll(match);
            });
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the first occurrence.
        /// </summary>
        /// <param name="match">The predicate delegate.</param>
        /// <returns>Returns the index of the first occurrence of a matching elements; or -1.</returns>
        public int FindIndex(Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.FindIndex(match);
            });
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the first occurrence.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="match">The predicate delegate.</param>
        /// <returns>Returns the index of the first occurrence of a matching elements; or -1.</returns>
        public int FindIndex(int startIndex, Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.FindIndex(startIndex, match);
            });
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the first occurrence within
        /// the range of elements in the list that starts at the specified index and contains 
        /// the specified number of elements.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The predicate delegate.</param>
        /// <returns>Returns the index of the first occurrence of a matching elements; or -1.</returns>
        public int FindIndex(int startIndex, int count, Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.FindIndex(startIndex, count, match);
            });
        }

        /// <summary>
        /// Searches for the last element that matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The predicate delegate.</param>
        /// <returns>Returns the last matching item, or null.</returns>
        public TValue FindLast(Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.FindLast(match);
            });
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the last occurrence.
        /// </summary>
        /// <param name="match">The predicate delegate.</param>
        /// <returns>Returns the index of the last occurrence of a matching elements; or -1.</returns>
        public int FindLastIndex(Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.FindLastIndex(match);
            });
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the last occurrence.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="match">The predicate delegate.</param>
        /// <returns>Returns the index of the last occurrence of a matching elements; or -1.</returns>
        public int FindLastIndex(int startIndex, Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.FindLastIndex(startIndex, match);
            });
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the last occurrence within
        /// the range of elements in the list that starts at the specified index and contains 
        /// the specified number of elements.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The predicate delegate.</param>
        /// <returns>Returns the index of the last occurrence of a matching elements; or -1.</returns>
        public int FindLastIndex(int startIndex, int count, Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.FindLastIndex(startIndex, count, match);
            });
        }

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source list. 
        /// </summary>
        /// <param name="index">The index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>Returns a shallow copy of a range of elements in the list.</returns>
        public List<TValue> GetRange(int index, int count)
        {
            return this.RunUnderLock(() =>
            {
                return this.list.GetRange(index, count);
            });
        }

        /// <summary>
        /// Reverses the order of the elements in the list.
        /// </summary>
        public void Reverse()
        {
            this.RunUnderLock(() =>
            {
                this.list.Reverse();
            });
        }

        /// <summary>
        /// Reverses the order of the elements in the list.
        /// </summary>
        /// <param name="index">The starting index of the range to reverse.</param>
        /// <param name="count">THe number of elements to reverse.</param>
        public void Reverse(int index, int count)
        {
            this.RunUnderLock(() =>
            {
                this.list.Reverse(index, count);
            });
        }

        /// <summary>
        /// Sorts the elements in the list.
        /// </summary>
        public void Sort()
        {
            this.RunUnderLock(() =>
            {
                this.list.Sort();
            });
        }

        /// <summary>
        /// Sorts the elements in the list.
        /// </summary>
        /// <param name="comparison">The comparison to use when comparing elements.</param>
        public void Sort(Comparison<TValue> comparison)
        {
            if (comparison == null)
            {
                throw new ArgumentNullException("comparison");
            }

            this.RunUnderLock(() =>
            {
                this.list.Sort(comparison);
            });
        }

        /// <summary>
        /// Sorts the elements in the list.
        /// </summary>
        /// <param name="comparer">The comparer to use when comparing elements.</param>
        public void Sort(IComparer<TValue> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            this.RunUnderLock(() =>
            {
                this.list.Sort(comparer);
            });
        }

        /// <summary>
        /// Sorts the elements in the list.
        /// </summary>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">The comparer to use when comparing elements.</param>
        public void Sort(int index, int count, IComparer<TValue> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            this.RunUnderLock(() =>
            {
                this.list.Sort(index, count, comparer);
            });
        }

        /// <summary>
        /// Copies the elements of the list to an array.
        /// </summary>
        /// <returns>Returns the array.</returns>
        public TValue[] ToArray()
        {
            return this.RunUnderLock(() =>
            {
                return this.list.ToArray();
            });
        }

        /// <summary>
        /// Determines whether every element in the list matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The delegate that defines the conditions to check against.</param>
        /// <returns>Returns true if every element matches the conditions.</returns>
        public bool TrueForAll(Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return this.RunUnderLock(() =>
            {
                return this.list.TrueForAll(match);
            });
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the 
        /// specified key, if the key is found.</param>
        /// <returns>Returns true if the collection contains an element with the specified key; otherwise false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            bool result = false;
            TValue v = default(TValue);

            this.RunUnderLock(() =>
            {
                result = this.hash.TryGetValue(key, out v);
            });

            value = v;
            return result;
        }

        /// <summary>
        /// Enables concurrency locking on the type.
        /// </summary>
        protected void MakeConcurrent()
        {
            if (this.l == null)
            {
                this.l = new object();
            }
        }


        /// <summary>
        /// Runs the given func under the lock, if concurrency is enabled.
        /// </summary>
        /// <typeparam name="T">The type of value to return.</typeparam>
        /// <param name="f">The func to run.</param>
        /// <returns>Returns the return value.</returns>
        private T RunUnderLock<T>(Func<T> f)
        {
            if (this.l != null)
            {
                lock (this.l)
                {
                    return f();
                }
            }
            else
            {
                return f();
            }
        }


        /// <summary>
        /// Runs the given action under the lock, if concurrency is enabled.
        /// </summary>
        /// <typeparam name="T">The type of value to return.</typeparam>
        /// <param name="a">The action to run.</param>
        /// <returns>Returns the return value.</returns>
        private void RunUnderLock(Action a)
        {
            if (this.l != null)
            {
                lock (this.l)
                {
                    a();
                }
            }
            else
            {
                a();
            }
        }
    }
}
