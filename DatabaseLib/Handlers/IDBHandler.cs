using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Handlers
{
    public interface IDBHandler
	{
		bool IsConnected { get; }
		IDbConnection Connection { get; }
		IDbTransaction Transaction { get; set; }

		bool Connect(string server, string database, string user = null, string password = null);

		bool Connect(string connectionString);

		bool Disconnect();

		void Execute(string query);

		string ExecuteScalar(string query);

		IDataReader ExecuteReader(string query);

		int GetNumberOfRows(string table);

		IList<string> GetTables();

		IList<string> GetViews();

		IList<string> GetStoredProcedures();

		string GetScriptFor(string objectName);
	}
}
