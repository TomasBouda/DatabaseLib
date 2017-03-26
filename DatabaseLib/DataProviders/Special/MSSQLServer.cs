using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Lib.Data;
using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace Database.Lib.DataProviders.Special
{
	public class MSSQLServer : DB	// TODO
	{
		public ServerConnection ServerConnection { get; set; }
		public Server Server { get; set; }

		public MSSQLServer() { }

		public MSSQLServer(SqlConnection connection)
		{
			Connection = connection;
			ServerConnection = new ServerConnection(connection);
			Server = new Server(ServerConnection);
		}

		public MSSQLServer(string server, string database, string user = null, string password = null)
		{
			Connect(server, database, user, password);
		}

		public void Connect(string server, string database, string user = null, string password = null)
		{
			string connectionString = $"Server={server};Integrated security=SSPI;database={database};"
				+ (user != null && password != null ? $"User id={user};Password={password};" : "");

			Connect(connectionString);
		}

		public void Connect(string connectionString)
		{
			Connect<SqlConnection, SqlException>(connectionString, c => new SqlConnection(c));
		}

		public int Execute(string query, params SqlParameter[] @params)
		{
			using (var cmd = Server.ConnectionContext.SqlConnectionObject.CreateCommand())
			{
				cmd.CommandText = query;//"INSERT INTO dbo.UpdateLog (Name) VALUES (@KitName)"
				cmd.CommandType = CommandType.Text;
				CommandAddParams(cmd, @params);

				return cmd.ExecuteNonQuery();
			}
		}

		public int ExecuteNonQuery(string sqlCommand)
		{
			return ServerConnection.ExecuteNonQuery(sqlCommand);
		}

		public SqlCommand CreateCommand()
		{
			return Server.ConnectionContext.SqlConnectionObject.CreateCommand();
		}

		public int ExecuteCommand(SqlCommand cmd)
		{
			using (cmd)
				return cmd.ExecuteNonQuery();
		}

		public SqlCommand CommandAddParams(SqlCommand cmd, params SqlParameter[] @params)
		{
			foreach (var param in @params)
			{
				cmd.Parameters.Add(param);
			}
			return cmd;
		}
	}
}
