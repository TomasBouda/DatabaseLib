using Database.Lib.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Data
{
	public interface IView : IDbObject
	{
		string Script { get; }
	}

	public class View<T> : DbObject<T>, IView, IDbObject where T : class, IDB, new()
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

		public View(string schema, string name, T db) : base(schema, name)
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
