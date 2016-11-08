using Database.Lib.Data;
using Database.Lib.DBMS;
using Database.Lib.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib
{
	public enum Sort
	{
		none,
		asc,
		desc
	}

	[Flags]
	public enum EDbObjects : int
	{
		None = 0,
		Tables = 1,
		Columns = 2,
		Views = 4,
		StoredProcedures = 8,

		All = Tables | Views | StoredProcedures | Columns
	}


	public class DatabaseManager<T> : IDisposable
		where T : class, IDB<T>, new()
	{
		public T DB { get; set; }
		private IList<IDbObject<T>> AllObjects { get; set; }
		public IList<IDbObject<T>> FoundDbObjects { get; set; }

		public string SearchQuery { get; set; }

		public DatabaseManager() { }

		public DatabaseManager(string connectionString) : base()
		{
			DB = new T();
			DB.Connect(connectionString);
		}

		public DatabaseManager(string server, string database, string user = null, string password = null) : base()
		{
			DB = new T();
			DB.Connect(server, database, user, password);
		}

		public void SearchInDb(string query = "", EDbObjects searchIn = EDbObjects.All, Sort sort = Sort.asc)
		{
			if (DB == null || !DB.IsConnected) throw new Exception("There is no database connection!");

			SearchQuery = query;

			if (AllObjects == null)
				AllObjects = DB.GetObjects().DistinctBy(x => x.Name).ToList();

			FoundDbObjects = AllObjects.Where(o =>
						o is Table<T> && (searchIn.HasFlag(EDbObjects.Tables) | searchIn.HasFlag(EDbObjects.Columns))
					|| o is View<T> && searchIn.HasFlag(EDbObjects.Views)
					|| o is StoredProcedure<T> && searchIn.HasFlag(EDbObjects.StoredProcedures)
				).ToList();

			if (query != "")
			{
				var foundColumns = searchIn.HasFlag(EDbObjects.Columns) ? DB.SearchColumn(query) : null;
				var foundScripts = (searchIn.HasFlag(EDbObjects.StoredProcedures) | searchIn.HasFlag(EDbObjects.Views)) ? DB.SearchInScripts(query) : null;

				FoundDbObjects = FoundDbObjects.Where(o =>

						(!searchIn.HasFlag(EDbObjects.Columns) && o.Name.ToLowerInvariant().Contains(query.ToLowerInvariant()))

					|| (searchIn.HasFlag(EDbObjects.Columns) && foundColumns.Any(a => a == o.Name))

					|| ((searchIn.HasFlag(EDbObjects.StoredProcedures) | searchIn.HasFlag(EDbObjects.Views)) && foundScripts.Any(a => a == o.Name))

				).ToList();
			}

			if (sort != Sort.none)
			{
				FoundDbObjects = sort == Sort.asc ? 
					FoundDbObjects.OrderBy(o => o.Name).ToList() : 
					FoundDbObjects.OrderByDescending(o => o.Name).ToList();
			}
		}

		public void ClearCache()
		{
			AllObjects = null;
		}

		public void Dispose()
		{
			ClearCache();
			DB.Dispose();
		}
	}
}
