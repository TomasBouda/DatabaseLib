using Database.Lib.Data;
using Database.Lib.DataProviders;
using Database.Lib.DataProviders.ConnectionParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Search
{
	public interface IDatabaseManager
	{
		string Name { get; set; }
		bool IsConnected { get; }
		string ConnectionString { get; }
		SearchResults Results { get; set; }
		string SearchQuery { get; set; }
		ConnectionResult Connect(string connectionString);
		ConnectionResult Connect(IConnectionParams connParams);
		SearchResults SearchInDb(string query = "", EDbObjects searchIn = EDbObjects.All, Sort sort = Sort.asc);
		bool IsTable(IDbObject dbObject);
		bool IsView(IDbObject dbObject);
		bool IsStoredProcedure(IDbObject dbObject);
		ITable Table(IDbObject dbObject);
		IView View(IDbObject dbObject);
		IStoredProcedure StoredProcedure(IDbObject dbObject);
		void ClearCache();
		void Dispose();
	}
}
