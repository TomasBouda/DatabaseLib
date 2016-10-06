using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Database.Lib.Handlers
{
	public class MSSQLHandler : DBHandler, IDBHandler, IDisposable
	{ 
		public MSSQLHandler() { }

		public MSSQLHandler(string server, string database, string user = null, string password = null)
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
			Connection = new SqlConnection(connectionString);

			try
			{
				Connection.Open();
				return true;
			}
			catch (SqlException ex) //TOMDO
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public void Execute(string query)
		{
			using(var cmd = new SqlCommand(query, (SqlConnection)Connection))
				cmd.ExecuteNonQuery();
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
			try
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
			catch (SqlException ex)	//TOMDO
			{
				Console.WriteLine(ex.Message);
				return -1;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return -1;
			}
		}

		public IList<string> GetTables()	// TODO
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
	}
}
