using System.Data;
using TomLabs.SQuirreL.Data;
using TomLabs.SQuirreL.DataProviders;
using TomLabs.SQuirreL.DataProviders.ConnectionParams;

namespace TomLabs.SQuirreL.Search
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

		SearchResults SearchInDb(string query = "", ObjectTypes searchIn = ObjectTypes.All, Sort sort = Sort.asc);

		int Execute(string query);

		string ExecuteScalar(string query);

		DataSet ExecuteDataSet(string query);

		IDataReader ExecuteReader(string query);

		IDbCommand CreateCommand(string query, CommandType type = CommandType.Text);

		void ClearCache();

		void Dispose();
	}
}