using Database.Lib.DataProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Data
{
	public class Table<T> : DbObject<T>, IDbObject<T> where T : class, IDB<T>, new()
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

		public Table(string schema, string name, T db) : base(schema, name)
		{
			DB = db;
		}

		public Table(string schema, string name, T db, bool triggers) : this(schema, name, db)
		{
			if(triggers)
				Triggers = db.GetTriggers(Name)?.Select(t => new Trigger<T>(schema, t, db));	// TODO takhle ne!!!
		}

		public bool Load(T db)
		{
			try
			{
				Columns = db.GetColumnsInfo(Schema, Name);
				foreach(var trigger in Triggers)
				{
					trigger.Load(db);
				}
				IsLoaded = true;
			}
			catch
			{
				IsLoaded = false;
			}

			return IsLoaded;
		}
	}
}
