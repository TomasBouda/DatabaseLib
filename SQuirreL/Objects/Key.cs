using TomLabs.SQuirreL.DataProviders;

namespace TomLabs.SQuirreL.Data
{
	public interface IKey : IDbObject
	{
	}

	public class Key<T> : DbObject<T>, ITrigger, IDbObject where T : class, IDB, new()
	{
		public Key(string schema, string name, T db)
			: base(schema, name, db, () => db.GetScriptFor(name, EDbObjects.None)) { }
	}
}