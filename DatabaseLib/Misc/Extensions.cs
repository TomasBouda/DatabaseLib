using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Database.Lib.Misc
{
	public static class Extensions
	{
		public static string ToStringSingle(this DataTable dataTable)
		{
			StringBuilder output = new StringBuilder();
			foreach (DataRow row in dataTable.Rows)
			{
				for (int i = 0; i < dataTable.Columns.Count; i++)
				{
					var text = row[i].ToString();
					output.AppendLine(text);
				}
			}
			return output.ToString();
		}

		public static string ToFormatedString(this DataTable dataTable)
		{
			StringBuilder output = new StringBuilder();

			var columnsWidths = new int[dataTable.Columns.Count];

			// Get column widths
			foreach (DataRow row in dataTable.Rows)
			{
				for (int i = 0; i < dataTable.Columns.Count; i++)
				{
					var length = row[i].ToString().Length;
					if (columnsWidths[i] < length)
						columnsWidths[i] = length;
				}
			}

			// Get Column Titles
			for (int i = 0; i < dataTable.Columns.Count; i++)
			{
				var length = dataTable.Columns[i].ColumnName.Length;
				if (columnsWidths[i] < length)
					columnsWidths[i] = length;
			}

			// Write Column titles
			for (int i = 0; i < dataTable.Columns.Count; i++)
			{
				var text = dataTable.Columns[i].ColumnName;
				output.Append("|" + PadCenter(text, columnsWidths[i] + 2));
			}
			output.Append("|\n" + new string('=', output.Length) + "\n");

			// Write Rows
			foreach (DataRow row in dataTable.Rows)
			{
				for (int i = 0; i < dataTable.Columns.Count; i++)
				{
					var text = row[i].ToString();
					output.Append("|" + PadCenter(text, columnsWidths[i] + 2));
				}
				output.Append("|\n");
			}
			return output.ToString();
		}

		private static string PadCenter(string text, int maxLength)
		{
			int diff = maxLength - text.Length;
			return new string(' ', diff / 2) + text + new string(' ', (int)(diff / 2.0 + 0.5));

		}

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
	}
}
