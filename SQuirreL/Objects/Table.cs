using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using TomLabs.OpenSource.SQuirreL.DataProviders;

namespace TomLabs.OpenSource.SQuirreL.Data
{
	public interface ITable : IDbObject
	{
		DataSet Columns { get; }
		DataSet Select(string where = null);
	}

	public class Table<T> : DbObject<T>, ITable, IDbObject where T : class, IDB, new()
	{
		private DataSet _columns;
		public DataSet Columns
		{
			get
			{
				if (_columns == null)
					Load(DB);

				return _columns;
			}
			private set { _columns = value; }
		}

		public IEnumerable<Trigger<T>> Triggers { get; set; }

		public Table(string schema, string name, T db)
			: base(schema, name, db, () => db.GetScriptFor(name, EDbObjects.Tables)) { }

		public Table(string schema, string name, T db, bool triggers) : this(schema, name, db)
		{

		}

		public override bool Load(T db)
		{
			try
			{
				Columns = db.GetColumnsInfo(Schema, Name);
				Script = GetScript();
				//foreach(var trigger in Triggers)
				//{
				//	trigger.Load(db);
				//}
				IsLoaded = true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				IsLoaded = false;
			}

			return IsLoaded;
		}

		public DataSet Select(string where = null)
		{
			where = where != null ? "WHERE " + where : "";

			return DB.ExecuteDataSet($"SELECT * FROM {Schema}.{Name} {where}");
		}
	}
}
