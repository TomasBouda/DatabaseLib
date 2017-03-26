using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Lib.Data;
using Database.Lib.DataProviders.ConnectionParams;
using Database.Lib.Search;
using MySql.Data.MySqlClient;
using Database.Lib.Misc;

namespace Database.Lib.DataProviders
{
	public class MySql : DB, IDB // TODO
	{
		private const string GetScriptForTableColumn = "Create Table";
		private const string GetScriptForViewolumn = "Create View";
		private const string GetScriptForStoredProcedureColumn = "Create Procedure";

		public void Connect(string server, string database, string user = null, string password = null)
		{
			string connectionString = $"Server={server};Database={database};Uid={user};Pwd ={password};";

			Connect(connectionString);
		}

		public void Connect<TConn>(TConn @params) where TConn : IConnectionParams
		{
			if (!(@params is MySqlConnectionParams)) throw new Exception($"Wrong type of connection paremeters. Please provide {nameof(MySql)} connection parameters.");

			var connParams = @params as MySqlConnectionParams;

			Connect(connParams.Server, connParams.Database, connParams.Username, connParams.Password);
		}

		public void Connect(string connectionString)
		{
			Connect<MySqlConnection, MySqlException>(connectionString, c => new MySqlConnection(c));
		}

		public int Execute(string query)
		{
			using (var cmd = new MySqlCommand(query, (MySqlConnection)Connection))
				return cmd.ExecuteNonQuery();
		}

		public DataSet ExecuteDataSet(string query)
		{
			using (MySqlCommand sqlCommand = new MySqlCommand(query, (MySqlConnection)Connection))
			{
				using (DataSet ds = new DataSet())
				{
					MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter();
					mySqlDataAdapter.SelectCommand = sqlCommand;
					mySqlDataAdapter.Fill(ds);
					mySqlDataAdapter.Dispose();

					return ds;
				}
			}
		}

		public IDataReader ExecuteReader(string query)
		{
			using (var cmd = new MySqlCommand(query, (MySqlConnection)Connection))
				return cmd.ExecuteReader();
		}

		public string ExecuteScalar(string query)
		{
			using (var cmd = new MySqlCommand(query, (MySqlConnection)Connection))
				return cmd.ExecuteScalar()?.ToString();
		}

		public DataSet GetColumnsInfo(string schema, string tableName)
		{
			string script = $"desc {schema}.{tableName}";

			return ExecuteDataSet(script);
		}

		public int GetNumberOfRows(string table)
		{
			throw new NotImplementedException();
		}

		public IList<IDbObject> GetObjects(EDbObjects including = EDbObjects.All)
		{
			var allObjects = new List<IDbObject>();

			if ((including & (EDbObjects.Tables | EDbObjects.Columns)) != 0)
				allObjects.AddRange(GetTables().Select(x => new Table<MySql>(x.Item1, x.Item2, this)));

			if ((including & EDbObjects.Views) != 0)
				allObjects.AddRange(GetViews().Select(x => new View<MySql>(x.Item1, x.Item2, this)));

			if ((including & EDbObjects.StoredProcedures) != 0)
				allObjects.AddRange(GetStoredProcedures().Select(x => new StoredProcedure<MySql>(x.Item1, x.Item2, this)));

			return allObjects;
		}

		public string GetScriptFor(string objectName, EDbObjects objType)
		{
			string script;
			switch (objType)
			{
				case EDbObjects.Tables:
				{
					script = $"show create table {objectName}";
					var ds = ExecuteDataSet(script);
					return ds.Tables[0]?.Rows[0]?.Field<string>(GetScriptForTableColumn);
				}
				case EDbObjects.Views:
				{
					script = $"show create view {objectName}";
					var ds = ExecuteDataSet(script);
					return ds.Tables[0]?.Rows[0]?.Field<string>(GetScriptForViewolumn);
					}
				case EDbObjects.StoredProcedures:
				{
					script = $"show create procedure {objectName}";
					var ds = ExecuteDataSet(script);
					return ds.Tables[0]?.Rows[0]?.Field<string>(GetScriptForStoredProcedureColumn);
					}

				default: return "";
			}
		}

		public IList<Tuple<string, string>> GetStoredProcedures()
		{
			return GetCollection<MySqlConnection>("Procedures");
		}

		public IList<Tuple<string, string>> GetTables()
		{
			return GetCollection<MySqlConnection>("Tables", new string[] { null, null, null, "BASE TABLE" });
		}

		public IList<string> GetTriggers(string tableName)
		{
			throw new NotImplementedException();
		}

		public IList<Tuple<string, string>> GetViews()
		{
			return GetCollection<MySqlConnection>("Views");
		}

		public IList<string> SearchColumn(string columnName)
		{
			throw new NotImplementedException();
		}

		public IList<string> SearchInScripts(string query)
		{
			throw new NotImplementedException();
		}

		public IDbCommand CreateCommand(string query, CommandType type = CommandType.Text)
		{
			throw new NotImplementedException();
		}
	}
}
