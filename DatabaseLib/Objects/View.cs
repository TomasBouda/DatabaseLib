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
	}

	public class View<T> : DbObject<T>, IView, IDbObject where T : class, IDB, new()
	{
		public View(string schema, string name, T db) 
			: base(schema, name, db, () => db.GetScriptFor(name, EDbObjects.Views)) { }
	}
}
