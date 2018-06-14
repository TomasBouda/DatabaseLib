using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using TomLabs.Shadowgem.Extensions.String;
using TomLabs.SQuirreL.DataProviders;

namespace TomLabs.SQuirreL.Data
{
	public interface ITable : IDbObject
	{
		DataSet Columns { get; }
		
		IEnumerable<ITrigger> Triggers { get; }
	}

	public class Table<T> : DbObject<T>, ITable, IDbObject where T : class, IDB, new()
	{
		private DataSet _columns;
		public DataSet Columns
		{
			get
			{
				if (_columns == null)
					LoadScript(DB);

				return _columns;
			}
			private set { _columns = value; }
		}

		public IEnumerable<ITrigger> Triggers { get; private set; }

		public Table(string schema, string name, T db)
			: base(schema, name, db, () => db.GetScriptFor(name, ObjectTypes.Tables))
		{

		}

		public Table(string schema, string name, T db, bool triggers) : this(schema, name, db)
		{
		}

		protected override bool LoadScript(T db)
		{
			try
			{
				Columns = db.GetColumnsInfo(Schema, Name);
				NumberOfRows = db.ExecuteScalar($"SELECT COUNT(*) FROM {Schema}{Name}").ToInt(-1);
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
	}
}