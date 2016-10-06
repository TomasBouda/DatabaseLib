using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Database.Lib.Handlers
{
	public class MySqlHandler : DBHandler, IDBHandler, IDisposable
	{
		public bool Connect(string connectionString)
		{
			throw new NotImplementedException();
		}

		public bool Connect(string server, string database, string user = null, string password = null)
		{
			throw new NotImplementedException();
		}

		public void Execute(string query)
		{
			throw new NotImplementedException();
		}

		public IDataReader ExecuteReader(string query)
		{
			throw new NotImplementedException();
		}

		public string ExecuteScalar(string query)
		{
			throw new NotImplementedException();
		}

		public int GetNumberOfRows(string table)
		{
			throw new NotImplementedException();
		}

		public string GetScriptFor(string objectName)
		{
			throw new NotImplementedException();
		}

		public IList<string> GetStoredProcedures()
		{
			throw new NotImplementedException();
		}

		public IList<string> GetTables()
		{
			throw new NotImplementedException();
		}

		public IList<string> GetViews()
		{
			throw new NotImplementedException();
		}
	}
}
