using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TomLabs.SQuirreL.Data;
using TomLabs.SQuirreL.DataProviders;

namespace TomLabs.SQuirreL.Search
{
	public enum Sort
	{
		none,
		asc,
		desc
	}

	public class DatabaseManager<T> : IDatabaseManager, IDisposable
		where T : class, IDB, new()
	{
		private T DB { get; set; }

		public string Name { get; set; }

		public bool IsConnected
		{
			get
			{
				return DB.IsConnected;
			}
		}

		public string ConnectionString
		{
			get
			{
				return DB.Connection.ConnectionString;
			}
		}

		public IDbConnection Connection
		{
			get
			{
				return DB.Connection;
			}
		}

		private IList<IDbObject> AllObjects { get; set; }
		private DataSet Triggers { get; set; }
		public SearchResults Results { get; set; }

		public string SearchQuery { get; set; }

		public DatabaseManager()
		{
			DB = new T();
		}

		public DatabaseManager(string name) : this()
		{
			Name = name;
		}

		public ConnectionResult Connect(string connectionString)
		{
			return Connect(() => DB.Connect(connectionString));
		}

		public ConnectionResult Connect(IConnectionParams connParams)
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

		public SearchResults SearchInDb(string query = "", ObjectTypes searchIn = ObjectTypes.All, Sort sort = Sort.asc)
		{
			Results = new SearchResults();

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
						o is Table<T> && (searchIn.HasFlag(ObjectTypes.Tables) | searchIn.HasFlag(ObjectTypes.Columns))
					|| o is View<T> && searchIn.HasFlag(ObjectTypes.Views)
					|| o is StoredProcedure<T> && searchIn.HasFlag(ObjectTypes.StoredProcedures)
				).ToList();

			if (query != "")
			{
				var foundColumns = searchIn.HasFlag(ObjectTypes.Columns) ? DB.FindColumn(query) : null;
				var foundScripts = (searchIn.HasFlag(ObjectTypes.StoredProcedures) | searchIn.HasFlag(ObjectTypes.Views)) ? DB.FindInScripts(query) : null;

				Results.FoundDbObjects = Results.FoundDbObjects.Where(o =>

						(searchIn.HasFlag(ObjectTypes.Tables) && o.Name.ToLowerInvariant().Contains(query.ToLowerInvariant()))

					|| (searchIn.HasFlag(ObjectTypes.Columns) && foundColumns.Any(a => a == o.Name))

					//|| (searchIn.HasFlag(EDbObjects.Triggers) && o is Table<T> && ((Table<T>)o).Triggers.Any(t => t.Name.ToLowerInvariant().Contains(query.ToLowerInvariant())))

					|| ((searchIn.HasFlag(ObjectTypes.StoredProcedures) | searchIn.HasFlag(ObjectTypes.Views)) && foundScripts.Any(a => a == o.Name))

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

		public DataSet ExecuteDataSet(string query)
		{
			return DB.ExecuteDataSet(query);
		}

		public IDataReader ExecuteReader(string query)
		{
			return DB.ExecuteReader(query);
		}

		public int Execute(string query)
		{
			return DB.Execute(query);
		}

		public string ExecuteScalar(string query)
		{
			return DB.ExecuteScalar(query);
		}
		
	}
}