using TomLabs.SQuirreL.DataProviders;

namespace TomLabs.SQuirreL.Data
{
	public interface IConstraint : IDbObject
	{
	}

	public class Constraint<T> : DbObject<T>, ITrigger, IDbObject where T : class, IDB, new()
	{
		public Constraint(string schema, string name, T db)
			: base(schema, name, db, () => db.GetScriptFor(name, EDbObjects.None)) { }
	}
}