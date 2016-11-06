using Database.Lib.Data;
using Database.Lib.DBMS;
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

	public class DatabaseManager<T> : IDisposable
		where T : class, IDB<T>, new()
	{
		public T DB { get; set; }
		private IList<IDbObject<T>> AllObjects { get; set; }
		public IList<IDbObject<T>> DbObjects { get; set; }

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

		public void SearchInDb(string query = "", Sort sort = Sort.asc)
		{
			if (DB == null || !DB.IsConnected) return;

			SearchQuery = query;

			if (AllObjects == null)
				AllObjects = DB.GetAllObjects();

			DbObjects = AllObjects.ToList();

			if (query != "")
			{
				var foundColumns = DB.SearchColumn(query);
				var foundScripts = DB.SearchInScripts(query);

				DbObjects = DbObjects.Where(o =>
					o.Name.ToLowerInvariant().Contains(query.ToLowerInvariant())
					|| foundColumns.Any(a => a == o.Name)
					|| foundScripts.Any(a => a == o.Name)
				).ToList();
			}
			if (sort != Sort.none)
			{
				DbObjects = sort == Sort.asc ? 
					DbObjects.OrderBy(o => o.Name).ToList() : 
					DbObjects.OrderByDescending(o => o.Name).ToList();
			}
		}

		public void Dispose()
		{
			DB.Dispose();
		}
	}
}
