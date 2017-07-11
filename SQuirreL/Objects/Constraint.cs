using TomLabs.OpenSource.SQuirreL.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomLabs.OpenSource.SQuirreL.Data
{
	public interface ITrigger : IDbObject
	{
	}

	public class Trigger<T> : DbObject<T>, ITrigger, IDbObject where T : class, IDB, new()
	{
		public Trigger(string schema, string name, T db) 
			: base(schema, name, db, () => db.GetScriptFor(name, EDbObjects.None)) { }
	}
}
