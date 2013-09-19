namespace Collective
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extensions to the <see cref="List"/> type.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Determines whether the list contains elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="list">The list to search within.</param>
        /// <param name="match">The predicate delegate that defines the conditions of the elements to search for.</param>
        /// <returns>true if the list contains one or more elements that match the conditions specified by the predicate; false otherwise.</returns>
        /// <typeparam name="T">The type of the element contained within the list.</typeparam>
        public static bool Exists<T>(this IList<T> list, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            for (int i = 0; i < list.Count; ++i)
            {
                if (match(list[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the first occurrence within the entire list.
        /// </summary>
        /// <param name="list">The list to search within.</param>
        /// <param name="match">The predicate delegate that defines the conditions of the elements to search for.</param>
        /// <returns>The first element that matches the conditions defined by the specified predicate; or null.</returns>
        /// <typeparam name="T">The type of the element contained within the list.</typeparam>
        public static T Find<T>(this IList<T> list, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            for (int i = 0; i < list.Count; ++i)
            {
                T item = list[i];
                if (match(item))
                {
                    return item;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified predicate. 
        /// </summary>
        /// <param name="list">The list to search within.</param>
        /// <param name="match">The predicate delegate that defines the conditions of the elements to search for.</param>
        /// <returns>A list containing all the elements that match the specified predicate.</returns>
        /// <typeparam name="T">The type of the element contained within the list.</typeparam>
        public static IList<T> FindAll<T>(this IList<T> list, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            List<T> resultSet = new List<T>();

            for (int i = 0; i < list.Count; ++i)
            {
                T item = list[i];
                if (match(item))
                {
                    resultSet.Add(item);
                }
            }

            return resultSet;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the index of the first occurrence within the list.
        /// </summary>
        /// <param name="list">The list to search within.</param>
        /// <param name="match">The predicate delegate that defines the conditions of the elements to search for.</param>
        /// <returns>The index of the first matching element.</returns>
        /// <typeparam name="T">The type of the element contained within the list.</typeparam>
        public static int FindIndex<T>(this IList<T> list, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return list.FindIndex(0, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the index of the first occurrence within the list.
        /// </summary>
        /// <param name="list">The list to search within.</param>
        /// <param name="startIndex">The starting index of the search.</param>
        /// <param name="match">The predicate delegate that defines the conditions of the elements to search for.</param>
        /// <returns>The index of the first matching element.</returns>
        /// <typeparam name="T">The type of the element contained within the list.</typeparam>
        public static int FindIndex<T>(this IList<T> list, int startIndex, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            for (int i = startIndex; i < list.Count; ++i)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the index of the first occurrence within the list.
        /// </summary>
        /// <param name="list">The list to search within.</param>
        /// <param name="startIndex">The starting index of the search.</param>
        /// <param name="count">The number of elements in the search to search.</param>
        /// <param name="match">The predicate delegate that defines the conditions of the elements to search for.</param>
        /// <returns>The index of the first matching element.</returns>
        /// <typeparam name="T">The type of the element contained within the list.</typeparam>
        public static int FindIndex<T>(this IList<T> list, int startIndex, int count, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            for (int i = 0; i < count; ++i)
            {
                if (match(list[i + startIndex]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the last occurrence within the entire list.
        /// </summary>
        /// <param name="list">The list to search within.</param>
        /// <param name="match">The predicate delegate that defines the conditions of the elements to search for.</param>
        /// <returns>The last element that matches the conditions defined by the specified predicate; or null.</returns>
        /// <typeparam name="T">The type of the element contained within the list.</typeparam>
        public static T FindLast<T>(this IList<T> list, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            for (int i = list.Count - 1; i >= 0; --i)
            {
                T item = list[i];
                if (match(item))
                {
                    return item;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the index of the last occurrence within the list.
        /// </summary>
        /// <param name="list">The list to search within.</param>
        /// <param name="match">The predicate delegate that defines the conditions of the elements to search for.</param>
        /// <returns>The index of the last matching element.</returns>
        /// <typeparam name="T">The type of the element contained within the list.</typeparam>
        public static int FindLastIndex<T>(this IList<T> list, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            return list.FindLastIndex(list.Count - 1, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the index of the last occurrence within the list.
        /// </summary>
        /// <param name="list">The list to search within.</param>
        /// <param name="startIndex">The starting index of the search.</param>
        /// <param name="match">The predicate delegate that defines the conditions of the elements to search for.</param>
        /// <returns>The index of the last matching element.</returns>
        /// <typeparam name="T">The type of the element contained within the list.</typeparam>
        public static int FindLastIndex<T>(this IList<T> list, int startIndex, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            for (int i = startIndex; i >= 0; --i)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the index of the last occurrence within the list.
        /// </summary>
        /// <param name="list">The list to search within.</param>
        /// <param name="startIndex">The starting index of the search.</param>
        /// <param name="count">The number of elements in the search to search.</param>
        /// <param name="match">The predicate delegate that defines the conditions of the elements to search for.</param>
        /// <returns>The index of the last matching element.</returns>
        /// <typeparam name="T">The type of the element contained within the list.</typeparam>
        public static int FindLastIndex<T>(this IList<T> list, int startIndex, int count, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            int counted = 0;
            for (int i = startIndex; i >= 0; --i)
            {
                if (counted++ == count)
                {
                    break;
                }

                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Determines whether every element in the list matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="list">The list to search within.</param>
        /// <param name="match">The predicate delegate that defines the conditions of the elements to match.</param>
        /// <returns>true if every element matches; false otherwise.</returns>
        /// <typeparam name="T">The type of the element contained within the list.</typeparam>
        public static bool TrueForAll<T>(this IList<T> list, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            for (int i = 0; i < list.Count; ++i)
            {
                if (!match(list[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Converts the list into a ListConverter.
        /// </summary>
        /// <typeparam name="TOriginal">The type of item in the original list.</typeparam>
        /// <typeparam name="TConverted">The type of item in the converted list.</typeparam>
        /// <param name="list">The list to convert.</param>
        /// <param name="converter">Func to convert list items.</param>
        /// <returns>The converted list.</returns>
        public static IList<TConverted> ToListConverter<TOriginal, TConverted>(this IList<TOriginal> list, Func<TOriginal, TConverted> converter)
        {
            return new ListConverter<TOriginal, TConverted>(list, converter);
        }
    }
}