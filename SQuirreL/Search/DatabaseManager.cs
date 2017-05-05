using TomLabs.OpenSource.SQuirreL.Data;
using TomLabs.OpenSource.SQuirreL.DataProviders;
using TomLabs.OpenSource.SQuirreL.DataProviders.ConnectionParams;
using TomLabs.OpenSource.SQuirreL.Misc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomLabs.OpenSource.SQuirreL.Search
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

		public SearchResults SearchInDb(string query = "", EDbObjects searchIn = EDbObjects.All, Sort sort = Sort.asc)
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

		public IDbObject IDbObject(object dbObject)
		{
			return dbObject as IDbObject;
		}

		public bool IsTable(IDbObject dbObject)
		{
			return dbObject is Table<T>;
		}

		public bool IsView(IDbObject dbObject)
		{
			return dbObject is View<T>;
		}

		public bool IsStoredProcedure(IDbObject dbObject)
		{
			return dbObject is StoredProcedure<T>;
		}

		public ITable Table(IDbObject dbObject)
		{
			return (Table<T>)dbObject;
		}

		public IView View(IDbObject dbObject)
		{
			return (View<T>)dbObject;
		}

		public IStoredProcedure StoredProcedure(IDbObject dbObject)
		{
			return (StoredProcedure<T>)dbObject;
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

		public IDbCommand CreateCommand(string query, CommandType type = CommandType.Text)
		{
			return DB.CreateCommand(query, type);
		}
	}
}
