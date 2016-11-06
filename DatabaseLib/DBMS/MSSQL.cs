using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Database.Lib.Data;
using Database.Lib.Misc;

namespace Database.Lib.DBMS
{
	public class MSSQL : DB, IDB<MSSQL>
	{ 
		public MSSQL() { }

		public MSSQL(string server, string database, string user = null, string password = null)
		{
			Connect(server, database, user, password);
		}

		public bool Connect(string server, string database, string user = null, string password = null)
		{
			string connectionString = $"Server={server};Integrated security=SSPI;database={database};" 
				+ (user != null && password != null ? $"User id={user};Password={password};" : "");

			return Connect(connectionString);			
		}

		public bool Connect(string connectionString)
		{
			return Connect<SqlConnection, SqlException>(connectionString, c => new SqlConnection(c));
		}

		public int Execute(string query)
		{
			using(var cmd = new SqlCommand(query, (SqlConnection)Connection))
				return cmd.ExecuteNonQuery();
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

		public int GetNumberOfRows(string table)
		{
			if (IsConnected)
			{
				int rows;
				if (int.TryParse(ExecuteScalar($"SELECT COUNT(*) FROM {table}"), out rows))
					return rows;
				else
					return -1;
			}
			else
				throw new Exception("Thre is no open connection!");
		}

		public IList<string> GetTables()
		{
			return GetCollection<SqlConnection>("Tables", new string[] { null, null, null, "BASE TABLE" });
		}

		public IList<string> GetViews()
		{
			return GetCollection<SqlConnection>("Views");
		}

		public IList<string> GetStoredProcedures()
		{
			return GetCollection<SqlConnection>("Procedures");
		}

		public IList<IDbObject<MSSQL>> GetAllObjects()
		{
			var allObjects = new List<IDbObject<MSSQL>>();

			allObjects.AddRange(GetTables().Select(x => new Table<MSSQL>(x, this)));
			allObjects.AddRange(GetViews().Select(x => new View<MSSQL>(x, this)));
			allObjects.AddRange(GetStoredProcedures().Select(x => new StoredProcedure<MSSQL>(x, this)));

			return allObjects;
		}

		public string GetScriptFor(string objectName)
		{
			using(SqlCommand sqlCommand = new SqlCommand("sys.sp_helptext", (SqlConnection)Connection))
			{
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.Parameters.AddWithValue("@objname", objectName);
				using (DataSet ds = new DataSet())
				{
					SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
					sqlDataAdapter.SelectCommand = sqlCommand;
					sqlDataAdapter.Fill(ds);
					sqlDataAdapter.Dispose();

					return ds.Tables[0].ToStringSingle();
				}
			}
		}

		public DataSet GetColumnsInfo(string tableName)
		{
			string script = @"SELECT 
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
				c.object_id = OBJECT_ID('{0}')";

			return ExecuteDataSet(string.Format(script, tableName));
		}

		public IList<string> SearchColumn(string columnName)
		{
			string script = @" SELECT DISTINCT c.name AS ColName, t.name AS TableName
				FROM sys.columns c
					JOIN sys.tables t ON c.object_id = t.object_id
				WHERE c.name LIKE '%{0}%'";

			var dataSet = ExecuteDataSet(string.Format(script, columnName));
			return dataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<string>("TableName")).ToList();
		}

		public IList<string> SearchInScripts(string query)
		{
			string script = @"SELECT DISTINCT
				   o.name AS Object_Name,
				   o.type_desc
			  FROM sys.sql_modules m
				   INNER JOIN
				   sys.objects o
					 ON m.object_id = o.object_id
			 WHERE m.definition Like '%{0}%';";

			var dataSet = ExecuteDataSet(string.Format(script, query));
			return dataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<string>("Object_Name")).ToList();
		}
	}
}
