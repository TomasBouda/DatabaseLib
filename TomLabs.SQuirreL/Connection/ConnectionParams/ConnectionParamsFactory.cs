using System;
using System.Collections.Generic;
using System.Text;

namespace TomLabs.SQuirreL.Connection.ConnectionParams
{
    public static class ConnectionParamsFactory
    {
		public static IConnectionParams CreateMssqlConnectionParams(string server, string database, string username = null, string password = null, bool integratedSecurity = true)
		{
			return new MSSQLConnectionParams(server, database, username, password, integratedSecurity);
		}
	}
}
