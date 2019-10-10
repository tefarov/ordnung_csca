using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.General
{
    /// <summary>
    /// General helpers
    /// </summary>
    public static class HLP_G
    {
        public static string NotNull(this string item, string exceptionmessage = null)
        {
            if (string.IsNullOrEmpty(item)) throw new ArgumentNullException(string.IsNullOrEmpty(exceptionmessage) ? "Invalid argument" : exceptionmessage);
            return item;
        }
        /// <summary>
        /// This will return an item if it's not null, otherwise through an Exception
        /// </summary>
        /// <typeparam name="T">Type of items to be checked for null</typeparam>
        /// <param name="item"></param>
        /// <param name="exceptionmessage">Exception's message</param>
        /// <returns></returns>
        public static T NotNull<T>(this T item, string exceptionmessage = null) where T : class
        {
            if (item == null) throw new ArgumentNullException(string.IsNullOrEmpty(exceptionmessage) ? "Invalid argument" : exceptionmessage);
            return item;
        }
        /// <summary>
        /// This will return an item if it's not null, otherwise through an Exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="defaultitem"></param>
        /// <returns></returns>
        public static T NotNull<T>(this T item, T defaultitem) where T : class
        {
            if (item == null && defaultitem == null) throw new ArgumentNullException("Default item is null");
            else if (item == null) return defaultitem;
            return item;
        }

        /// <summary>
        /// This applies immediatly some action to all the items in an enumeration (Enumeration will be enumerated right now)
        /// </summary>
        /// <param name="action">an action to apply</param>
        /// <param name="processnulls">if set to false will not process any null-items</param>
        public static IEnumerable<T> Apply<T>(this IEnumerable<T> items, Action<T> action, bool processnulls = false)
        {
            var enm = items.GetEnumerator();
            while (enm.MoveNext())
                if (processnulls || enm.Current != null)
                    action(enm.Current);

            return items;
        }
        /// <summary>
        /// This applies immediatly some action to all the items in an array. This doesn't create any enumerators, unlike IEnumearable.ApplyNow()
        /// </summary>
        /// <param name="action">an action to apply</param>
        /// <param name="processnulls">if set to false will not process any null-items</param>
        public static T[] Apply<T>(this T[] items, Action<T> action, bool processnulls = false)
        {
            T itm;
            for (int i = 0; i < items.Length; i++) {
                if ((itm = items[i]) != null || processnulls)
                    action(itm);
            }

            return items;
        }
        /// <summary>
        /// This applies immediatly some action to all the items in an array. This doesn't create any enumerators, unlike IEnumearable.ApplyNow()
        /// </summary>
        /// <param name="action">an action to apply</param>
        /// <param name="processnulls">if set to false will not process any null-items</param>
        public static T[] Apply<T>(this T[] items, Action<int, T> action, bool processnulls = false)
        {
            T itm;
            for (int i = 0; i < items.Length; i++) {
                if ((itm = items[i]) != null || processnulls)
                    action(i, itm);
            }

            return items;
        }
        /// <summary>
        /// Applies some action to all the items in the enumeration at the time of enumeration (Enumeration will not be enumerated now)
        /// </summary>
        /// <param name="action">an action to apply</param>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            var enm = items.GetEnumerator();
            while (enm.MoveNext()) {
                action(enm.Current);
                yield return enm.Current;
            }
        }

        public static IEnumerable<T> Single<T>(T item)
        {
            yield return item;
        }
        /// <summary>
        /// This returns a single item from the sequence if a sequence consists of a single item. Otherwise empty value
        /// </summary>
        /// <typeparam name="T">Type of items in a sequence</typeparam>
        /// <param name="items">The sequence</param>
        public static T SingleOrNone<T>(this IEnumerable<T> items)
        {
            if (items == null) return default(T);

            int c = 0;
            T val = default(T);
            var enm = items.GetEnumerator();

            while (enm.MoveNext()) {
                if (++c > 1) return default(T);
                val = enm.Current;
            }

            return val;
        }

        /// <summary>
        /// This makes ToArray a bit lighter for large collections with a known size
        /// </summary>
        public static T[] ToArray<T>(this IEnumerable<T> items, int count)
        {
            if (count == HLP_G.Auto) count = items.Count();

            T[] aitm = new T[count];

            int i = 0;
            var enm = items.GetEnumerator();
            while (enm.MoveNext()) {
                if (i >= aitm.Length) return aitm;
                aitm[i++] = enm.Current;
            }
            return aitm;
        }

        public const int Auto = -21245478;
    }
}
