using Database.Lib.Data;
using Database.Lib.DataProviders;
using Database.Lib.DataProviders.ConnectionParams;
using Database.Lib.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.DataProviders
{
    public interface IDB : IDisposable
	{
		bool IsConnected { get; }
		IDbConnection Connection { get; }
		IDbTransaction Transaction { get; }

		void Connect(string connectionString);

		void Connect<TConn>(TConn @params) where TConn : IConnectionParams;

		bool Disconnect();

		int Execute(string query);

		string ExecuteScalar(string query);

		IDataReader ExecuteReader(string query);

		DataSet ExecuteDataSet(string query);

		int GetNumberOfRows(string table);

		IList<Tuple<string, string>> GetTables();

		IList<Tuple<string, string>> GetViews();

		IList<Tuple<string, string>> GetStoredProcedures();

		IList<string> GetTriggers(string tableName);

		IList<IDbObject> GetObjects(EDbObjects including = EDbObjects.All);

		string GetScriptFor(string objectName);

		DataSet GetColumnsInfo(string schema, string tableName);

		IList<string> SearchColumn(string columnName);

		IList<string> SearchInScripts(string query);
	}
}
