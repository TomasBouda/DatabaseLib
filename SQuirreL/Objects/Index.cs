using TomLabs.SQuirreL.DataProviders;

namespace TomLabs.SQuirreL.Data
{
	public interface IIndex : IDbObject
	{
	}

	public class Index<T> : DbObject<T>, ITrigger, IDbObject where T : class, IDB, new()
	{
		public Index(string schema, string name, T db)
			: base(schema, name, db, () => db.GetScriptFor(name, EDbObjects.None)) { }
	}
}