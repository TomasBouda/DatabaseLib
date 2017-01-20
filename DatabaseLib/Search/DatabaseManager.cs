using Database.Lib.Data;
using Database.Lib.DataProviders;
using Database.Lib.DataProviders.ConnectionParams;
using Database.Lib.Misc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Search
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
		//Triggers = 16,

		All = Tables | Views | StoredProcedures | Columns /*| Triggers*/
	}


	public class DatabaseManager<T> : IDisposable
		where T : class, IDB<T>, new()
	{
		public T DB { get; set; }
		private IList<IDbObject<T>> AllObjects { get; set; }
		private DataSet Triggers { get; set; }
		public SearchResults<T> Results { get; set; }

		public string SearchQuery { get; set; }

		public DatabaseManager()
		{
			DB = new T();
		}

		public ConnectionResult Connect(string connectionString)
		{
			return Connect(() => DB.Connect(connectionString));
		}

		public ConnectionResult Connect(IConnectionParams<T> connParams)
		{
			return Connect(() => DB.Connect(connParams));
		}

		private ConnectionResult Connect(Action connectMethod)
		{
			DB = new T();
			ConnectionResult connRes = new ConnectionResult();

			try
			{
				connectMethod();
				return connRes;
			}
			catch (Exception ex)
			{
				connRes.Success = false;
				connRes.Exception = ex;
				return connRes;
			}
		}

		public SearchResults<T> SearchInDb(string query = "", EDbObjects searchIn = EDbObjects.All, Sort sort = Sort.asc)
		{
			Results = new SearchResults<T>();

			if (DB == null || !DB.IsConnected)
			{
				Results.Error = true;
				Results.ErrorMessage = "There is no database connection!";
				return Results;
			}

			SearchQuery = query;

			if (AllObjects == null)
				AllObjects = DB.GetObjects().ToList();

			Results.FoundDbObjects = AllObjects.Where(o =>
						o is Table<T> && (searchIn.HasFlag(EDbObjects.Tables) | searchIn.HasFlag(EDbObjects.Columns))
					|| o is View<T> && searchIn.HasFlag(EDbObjects.Views)
					|| o is StoredProcedure<T> && searchIn.HasFlag(EDbObjects.StoredProcedures)
				).ToList();

			if (query != "")
			{
				var foundColumns = searchIn.HasFlag(EDbObjects.Columns) ? DB.SearchColumn(query) : null;
				var foundScripts = (searchIn.HasFlag(EDbObjects.StoredProcedures) | searchIn.HasFlag(EDbObjects.Views)) ? DB.SearchInScripts(query) : null;

				Results.FoundDbObjects = Results.FoundDbObjects.Where(o =>

						(searchIn.HasFlag(EDbObjects.Tables) && o.Name.ToLowerInvariant().Contains(query.ToLowerInvariant()))

					|| (searchIn.HasFlag(EDbObjects.Columns) && foundColumns.Any(a => a == o.Name))

					//|| (searchIn.HasFlag(EDbObjects.Triggers) && o is Table<T> && ((Table<T>)o).Triggers.Any(t => t.Name.ToLowerInvariant().Contains(query.ToLowerInvariant())))

					|| ((searchIn.HasFlag(EDbObjects.StoredProcedures) | searchIn.HasFlag(EDbObjects.Views)) && foundScripts.Any(a => a == o.Name))

				).ToList();
			}

			if (sort != Sort.none)
			{
				Results.FoundDbObjects = sort == Sort.asc ?
					Results.FoundDbObjects.OrderBy(o => o.Name).ToList() :
					Results.FoundDbObjects.OrderByDescending(o => o.Name).ToList();
			}

			return Results;
		}

		public void ClearCache()
		{
			AllObjects = null;
			Triggers = null;
		}

		public void Dispose()
		{
			ClearCache();
			DB.Dispose();
		}
	}
}
