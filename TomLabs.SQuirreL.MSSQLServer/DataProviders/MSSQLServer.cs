using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Data.SqlClient;

namespace TomLabs.SQuirreL.DataProviders.Special
{
	public class MSSQLServer : DB   // TODO
	{
		public ServerConnection ServerConnection { get; set; }
		public Server Server { get; set; }

		public MSSQLServer()
		{
		}

		public MSSQLServer(string connectionString)
		{
			Connect(connectionString);
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

			ServerConnection = new ServerConnection(Connection as SqlConnection);
			Server = new Server(ServerConnection);
		}

		public int Execute(string query, params SqlParameter[] @params)
		{
			using (var cmd = Server.ConnectionContext.SqlConnectionObject.CreateCommand())
			{
				cmd.CommandText = query;
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

		public override void BeginTransaction()
		{
			ServerConnection.BeginTransaction();
		}

		public override void CommitTransaction()
		{
			ServerConnection.CommitTransaction();
		}

		public override void RollBackTransaction()
		{
			ServerConnection.RollBackTransaction();
		}
	}
}