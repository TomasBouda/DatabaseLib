using Database.Lib.Data;
using Database.Lib.DBMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.DBMS
{
    public interface IDB<T> : IDisposable where T : class, IDB<T>, new()
	{
		bool IsConnected { get; }
		IDbConnection Connection { get; }
		IDbTransaction Transaction { get; set; }

		bool Connect(string server, string database, string user = null, string password = null);

		bool Connect(string connectionString);

		bool Disconnect();

		int Execute(string query);

		string ExecuteScalar(string query);

		IDataReader ExecuteReader(string query);

		DataSet ExecuteDataSet(string query);

		int GetNumberOfRows(string table);

		IList<string> GetTables();

		IList<string> GetViews();

		IList<string> GetStoredProcedures();

		IList<IDbObject<T>> GetAllObjects();

		string GetScriptFor(string objectName);

		DataSet GetColumnsInfo(string tableName);

		IList<string> SearchColumn(string columnName);

		IList<string> SearchInScripts(string query);
	}
}
