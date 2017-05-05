using TomLabs.OpenSource.SQuirreL.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomLabs.OpenSource.SQuirreL.Data
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
