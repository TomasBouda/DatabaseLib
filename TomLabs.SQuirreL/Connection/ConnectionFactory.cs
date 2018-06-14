using System;
using System.Collections.Generic;
using System.Text;
using TomLabs.SQuirreL.Connection.ConnectionParams;

namespace TomLabs.SQuirreL.Connection
{
	public static class ConnectionFactory
	{
		public static MSSQLConnection CreateMssqlConnection(string server, string database, string username = null, string password = null)
		{
			return new MSSQLConnection(ConnectionParamsFactory.CreateMssqlConnectionParams(server, database, username, password));
		}
	}
}
