using Database.Lib.DBMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Data
{
	public class View<T> : DbObject<T>, IDbObject<T> where T : class, IDB<T>, new()
	{
		private string _script = null;
		public string Script
		{
			get
			{
				if (_script == null)
					Load(DB);

				return _script;
			}
			private set { _script = value; }
		}

		public View(string name, T db) : base(name)
		{
			Load(db);
		}

		public bool Load(T db)
		{
			try
			{
				Script = db.GetScriptFor(Name);
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
