using System.Collections.Generic;
using TomLabs.SQuirreL.DataProviders;
using TomLabs.SQuirreL.Objects.Base;

namespace TomLabs.SQuirreL.Data
{
	public interface ITrigger : IScript, IDbObject
	{
	}

	public class Trigger<T> : DbObject<T>, ITrigger, IDbObject where T : class, IDB, new()
	{
		public Trigger(string schema, string name, T db)
			: base(schema, name, db, () => db.GetScriptFor(name, ObjectTypes.None)) { }

		public List<IDbObject> Affects => throw new System.NotImplementedException();
	}
}