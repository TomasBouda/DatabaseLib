using Database.Lib.DBMS;
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

		public Table(string name, T db) : base(name)
		{
			DB = db;
		}

		public bool Load(T db)
		{
			try
			{
				Columns = db.GetColumnsInfo(Name);
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
