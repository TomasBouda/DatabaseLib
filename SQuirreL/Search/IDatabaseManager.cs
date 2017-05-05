using TomLabs.OpenSource.SQuirreL.Data;
using TomLabs.OpenSource.SQuirreL.DataProviders;
using TomLabs.OpenSource.SQuirreL.DataProviders.ConnectionParams;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomLabs.OpenSource.SQuirreL.Search
{
	public interface IDatabaseManager
	{
		string Name { get; set; }
		bool IsConnected { get; }
		string ConnectionString { get; }
		IDbConnection Connection { get; }
		SearchResults Results { get; set; }
		string SearchQuery { get; set; }
		ConnectionResult Connect(string connectionString);
		ConnectionResult Connect(IConnectionParams connParams);
		SearchResults SearchInDb(string query = "", EDbObjects searchIn = EDbObjects.All, Sort sort = Sort.asc);
		int Execute(string query);
		string ExecuteScalar(string query);
		DataSet ExecuteDataSet(string query);
		IDataReader ExecuteReader(string query);
		IDbCommand CreateCommand(string query, CommandType type = CommandType.Text);

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
