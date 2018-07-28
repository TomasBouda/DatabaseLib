using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using TomLabs.SQuirreL.Objects;

namespace TomLabs.SQuirreL.Misc
{
	internal static class Extensions
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

		public static IList<string> ColumnToList(this DataSet dataSet, string columnName)
		{
			return dataSet.Tables[0]?.AsEnumerable().Select(dataRow => dataRow.Field<string>(columnName)).ToList();
		}

		public static IList<RawDbObject> ToRawObject(this DataSet dataSet, string schemaColName = "SchemaName", string objectColName = "ObjectName")
		{
			return dataSet.Tables[0]?
				.AsEnumerable()
				.Select(
					dataRow => new RawDbObject(dataRow.Field<string>(schemaColName), dataRow.Field<string>(objectColName))
					)
				.ToList();
		}

		public static SqlCommand AddParam(this SqlCommand cmd, string paramName, string value, SqlDbType type, int size) // TODO
		{
			var param = cmd.Parameters.Add(paramName, type, size);
			param.Value = value;

			return cmd;
		}
	}
}