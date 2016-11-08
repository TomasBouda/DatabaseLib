using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Lib.Data;
using MySql.Data.MySqlClient;

namespace Database.Lib.DBMS
{
	public class MySql : DB, IDB<MySql>, IDisposable
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

		public DataSet ExecuteDataSet(string query)
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

		public DataSet GetColumnsInfo(string tableName)
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

		public IList<string> SearchColumn(string columnName)
		{
			throw new NotImplementedException();
		}

		public IList<string> SearchInScripts(string query)
		{
			throw new NotImplementedException();
		}

		int IDB<MySql>.Execute(string query)
		{
			throw new NotImplementedException();
		}

		public IList<IDbObject<MySql>> GetObjects(EDbObjects including = EDbObjects.All)
		{
			throw new NotImplementedException();
		}
	}
}
