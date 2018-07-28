using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using TomLabs.SQuirreL.Connection.ConnectionParams;
using TomLabs.SQuirreL.Data;
using TomLabs.SQuirreL.Misc;
using TomLabs.SQuirreL.Objects;

namespace TomLabs.SQuirreL.DataProviders
{
	public class MSSQL : DB, IDB
	{
		public MSSQL()
		{
		}

		public MSSQL(string server, string database, string user = null, string password = null)
		{
			Connect(server, database, user, password);
		}

		public void Connect(string server, string database, string user = null, string password = null, bool integratedSecurity = true)
		{
			string connectionString = $"Server={server};Database={database};Integrated security={(integratedSecurity ? "true" : "false")};"
				+ (user != null && password != null ? $"User id={user};Password={password};" : "");

			Connect(connectionString);
		}

		public void Connect<TConn>(TConn @params) where TConn : IConnectionParams
		{
			if (!(@params is MSSQLConnectionParams)) throw new Exception($"Wrong type of connection paremeters. Please provide {nameof(MSSQL)} connection parameters.");

			var connParams = @params as MSSQLConnectionParams;

			Connect(connParams.Server, connParams.Database, connParams.Username, connParams.Password, connParams.IntegratedSecurity);
		}

		public void Connect(string connectionString)
		{
			Connect<SqlConnection, SqlException>(connectionString, c => new SqlConnection(c));
		}

		public int Execute(string query)
		{
			using (var cmd = new SqlCommand(query, (SqlConnection)Connection))
				return cmd.ExecuteNonQuery();
		}

		public int ExecuteTransaction(string query)
		{
			int rows = 0;
			foreach (var q in SplitSqlStatements(query))
			{
				using (var cmd = new SqlCommand(q, (SqlConnection)Connection, (SqlTransaction)Transaction))
				{
					rows += cmd.ExecuteNonQuery();
				}
			}
			return rows;
		}

		public IEnumerable<string> SplitSqlStatements(string sqlScript)
		{
			// Split by "GO" statements
			var statements = Regex.Split(
					sqlScript,
					@"^\s*GO\s*\d*\s*($|\-\-.*$)",
					RegexOptions.Multiline |
					RegexOptions.IgnorePatternWhitespace |
					RegexOptions.IgnoreCase);

			// Remove empties, trim, and return
			return statements
				.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(x => x.Trim(' ', '\r', '\n'));
		}

		public string ExecuteScalar(string query)
		{
			using (var cmd = new SqlCommand(query, (SqlConnection)Connection))
				return cmd.ExecuteScalar()?.ToString();
		}

		public IDataReader ExecuteReader(string query)
		{
			using (var cmd = new SqlCommand(query, (SqlConnection)Connection))
				return cmd.ExecuteReader();
		}

		public DataSet ExecuteDataSet(string query)
		{
			using (SqlCommand sqlCommand = new SqlCommand(query, (SqlConnection)Connection))
			{
				using (DataSet ds = new DataSet())
				{
					SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
					sqlDataAdapter.SelectCommand = sqlCommand;
					sqlDataAdapter.Fill(ds);
					sqlDataAdapter.Dispose();

					return ds;
				}
			}
		}

		public IDbCommand CreateCommand(string query, CommandType type = CommandType.Text) // TODO
		{
			var cmd = new SqlCommand(query, (SqlConnection)Connection);
			cmd.CommandType = type;
			return cmd;
		}

		public int ExecuteCommand(SqlCommand cmd)
		{
			using (cmd)
				return cmd.ExecuteNonQuery();
		}

		public IList<RawDbObject> GetTables()
		{
			return GetCollection<SqlConnection>("Tables", new string[] { null, null, null, "BASE TABLE" });
		}

		public IList<RawDbObject> GetViews()
		{
			return GetCollection<SqlConnection>("Views");
		}

		public IList<RawDbObject> GetStoredProcedures()
		{
			return GetCollection<SqlConnection>("Procedures");
		}

		public IList<string> GetTriggers(string tableName)
		{
			string script = $@"SELECT
				sysobjects.name AS trigger_name
				,USER_NAME(sysobjects.uid) AS trigger_owner
				,s.name AS table_schema
				,OBJECT_NAME(parent_obj) AS table_name
				,OBJECTPROPERTY( id, 'ExecIsUpdateTrigger') AS isupdate
				,OBJECTPROPERTY( id, 'ExecIsDeleteTrigger') AS isdelete
				,OBJECTPROPERTY( id, 'ExecIsInsertTrigger') AS isinsert
				,OBJECTPROPERTY( id, 'ExecIsAfterTrigger') AS isafter
				,OBJECTPROPERTY( id, 'ExecIsInsteadOfTrigger') AS isinsteadof
				,OBJECTPROPERTY(id, 'ExecIsTriggerDisabled') AS [disabled]
			FROM sysobjects
			INNER JOIN sysusers
				ON sysobjects.uid = sysusers.uid
			INNER JOIN sys.tables t
				ON sysobjects.parent_obj = t.object_id
			INNER JOIN sys.schemas s
				ON t.schema_id = s.schema_id
			WHERE sysobjects.type = 'TR' and OBJECT_NAME(parent_obj) = '{tableName}'";

			var dataSet = ExecuteDataSet(script);
			return dataSet.ColumnToList("trigger_name");
		}

		public IList<IDbObject> GetObjects(ObjectTypes including = ObjectTypes.All)
		{
			var allObjects = new List<IDbObject>();

			if ((including & (ObjectTypes.Tables | ObjectTypes.Columns)) != 0)
				allObjects.AddRange(GetTables().Select(x => new Table<MSSQL>(x.Schema, x.Name, this)));

			if ((including & ObjectTypes.Views) != 0)
				allObjects.AddRange(GetViews().Select(x => new View<MSSQL>(x.Schema, x.Name, this)));

			if ((including & ObjectTypes.StoredProcedures) != 0)
				allObjects.AddRange(GetStoredProcedures().Select(x => new StoredProcedure<MSSQL>(x.Schema, x.Name, this)));

			return allObjects;
		}

		public string GetScriptFor(string objectName, ObjectTypes objType = ObjectTypes.None)
		{
			using (SqlCommand sqlCommand = new SqlCommand("sys.sp_helptext", (SqlConnection)Connection))
			{
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.Parameters.AddWithValue("@objname", objectName);
				using (DataSet ds = new DataSet())
				{
					using(SqlDataAdapter sqlDataAdapter = new SqlDataAdapter())
					{
						sqlDataAdapter.SelectCommand = sqlCommand;
						sqlDataAdapter.Fill(ds);

						return ds.Tables[0].ToStringSingle();
					}
				}
			}
		}

		public DataSet GetColumnsInfo(string schema, string tableName)
		{
			string script =
			$@"SELECT DISTINCT
				c.name 'Column Name',
				t.Name 'Data type',
				c.max_length 'Max Length',
				c.precision ,
				c.scale ,
				c.is_nullable,
				ISNULL(i.is_primary_key, 0) 'Primary Key'
			FROM
				sys.columns c
			INNER JOIN
				sys.types t ON c.user_type_id = t.user_type_id
			LEFT OUTER JOIN
				sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
			LEFT OUTER JOIN
				sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
			WHERE
				c.object_id = OBJECT_ID('{schema}.{tableName}')";

			return ExecuteDataSet(script);
		}

		public IList<RawDbObject> FindColumn(string columnName)
		{
			string script =
				$@"SELECT c.name AS ColName, SCHEMA_NAME(t.schema_id) AS SchemaName , t.name AS TableName
				FROM sys.columns c
					JOIN sys.tables t ON c.object_id = t.object_id
				WHERE c.name LIKE '%{columnName}%'";// TODO wildcards

			var dataSet = ExecuteDataSet(script);
			return dataSet.ToRawObject();
		}

		public IList<RawDbObject> FindInScripts(string query)
		{
			string script =
			$@"SELECT DISTINCT
				   SCHEMA_NAME(o.schema_id) AS SchemaName,
				   o.name AS Object_Name,
				   o.type_desc
			  FROM sys.sql_modules m
				   INNER JOIN
				   sys.objects o
					 ON m.object_id = o.object_id
			 WHERE m.definition Like '%{query}%';";

			var dataSet = ExecuteDataSet(script);
			return dataSet.ToRawObject();
		}
	}
}