using System;
using System.Collections.Generic;
using TomLabs.SQuirreL.DataProviders;
using TomLabs.SQuirreL.Objects.Base;

namespace TomLabs.SQuirreL.Data
{
	public interface IStoredProcedure : IScript, IDbObject
	{
		
	}

	public class StoredProcedure<T> : DbObject<T>, IStoredProcedure where T : class, IDB, new()
	{
		private List<IDbObject> _affects;
		public List<IDbObject> Affects
		{
			get
			{
				if (!IsLoaded)
				{
					LoadScript();
				}

				return _affects;
			}
			set => _affects = value;
		}

		public StoredProcedure(string schema, string name, T db)
			: base(schema, name, db, () => db.GetScriptFor(name, ObjectTypes.StoredProcedures))
		{

		}


	}
}