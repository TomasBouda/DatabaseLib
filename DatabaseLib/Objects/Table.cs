﻿using Database.Lib.DataProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Data
{
	public interface ITable : IDbObject
	{
		DataSet Columns { get; }
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
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
				IsLoaded = false;
			}

			return IsLoaded;
		}
	}
}
