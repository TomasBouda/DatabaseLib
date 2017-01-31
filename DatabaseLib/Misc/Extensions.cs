using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Database.Lib.Misc
{
	public static class Extensions
	{
		/// <summary>
		/// Classic "SQL" Like function
		/// </summary>
		/// <param name="toSearch"></param>
		/// <param name="toFind"></param>
		/// <returns></returns>
		public static bool Like(this string toSearch, string toFind)
		{
			return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>
	   (this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector)
		{
			return source.DistinctBy(keySelector, null);
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>
			(this IEnumerable<TSource> source,
			 Func<TSource, TKey> keySelector,
			 IEqualityComparer<TKey> comparer)
		{
			source.ThrowIfNull("source");
			keySelector.ThrowIfNull("keySelector");
			return DistinctByImpl(source, keySelector, comparer);
		}

		private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>
			(IEnumerable<TSource> source,
			 Func<TSource, TKey> keySelector,
			 IEqualityComparer<TKey> comparer)
		{
			HashSet<TKey> knownKeys = new HashSet<TKey>(comparer);
			foreach (TSource element in source)
			{
				if (knownKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

		public static void ThrowIfNull<T>(this T data, string name) where T : class
		{
			if (data == null)
			{
				throw new ArgumentNullException(name);
			}
		}

		public static IList<string> ColumnToList(this DataSet dataSet, string columnName)
		{
			return dataSet.Tables[0]?.AsEnumerable().Select(dataRow => dataRow.Field<string>(columnName)).ToList();
		}

		public static SqlCommand AddParam(this SqlCommand cmd, string paramName, string value, SqlDbType type, int size) // TODO
		{
			var param = cmd.Parameters.Add(paramName, type, size);
			param.Value = value;

			return cmd;
		}
	}
}
