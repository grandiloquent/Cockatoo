namespace Share{	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	public static   class GenericExtensions{		public static IEnumerable<TSource> AppendItems<TSource>(this IEnumerable<TSource> source, params TSource[] items)
		{
			return source.Concat(items);
		}
		public static bool AreContinuous<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> original)
		{
			int num = -1;
			List<TSource> list = System.Linq.Enumerable.ToList(original);
			foreach (TSource item in source) {
				int num2 = list.IndexOf(item);
				if (num > -1 && num2 != num + 1) {
					return false;
				}
				num = num2;
			}
			return true;
		}
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source) {
				if (seenKeys.Add(keySelector(element))) {
					yield return element;
				}
			}
		}

		public static IEnumerable<T> Except<T>(this IEnumerable<T> items, T itemToExclude)
		{
			if (items == null) {
				throw new ArgumentNullException("items");
			}
			return items.Except(new T[1] {
				itemToExclude
			});
		}

		public static int FindIndex<T>(this IEnumerable<T> items, Predicate<T> criterion)
		{
			int num = 0;
			foreach (T item in items) {
				if (criterion(item)) {
					return num;
				}
				num++;
			}
			return -1;
		}
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (T item in items) {
				action(item);
			}
		}
		public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
		{
			int num = 0;
			foreach (T item in items) {
				action(item, num);
				num++;
			}
		}
      
     
		public static TSource SingleOrNull<TSource>(this IEnumerable<TSource> source) where TSource : class
		{
			if (source == null) {
				return null;
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null) {
				if (list.Count == 1) {
					return list[0];
				}
			} else {
				using (IEnumerator<TSource> enumerator = source.GetEnumerator()) {
					if (!enumerator.MoveNext()) {
						return null;
					}
					TSource current = enumerator.Current;
					if (!enumerator.MoveNext()) {
						return current;
					}
				}
			}
			return null;
		}
}}