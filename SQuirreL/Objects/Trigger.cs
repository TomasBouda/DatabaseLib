using TomLabs.SQuirreL.DataProviders;

namespace TomLabs.SQuirreL.Data
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