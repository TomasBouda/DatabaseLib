namespace TomLabs.SQuirreL.Connection.ConnectionParams
{
	public class MSSQLConnectionParams : IConnectionParams
	{
		public string ConnectionString => $"Server={Server};Database={Database};Integrated security={IntegratedSecurity};"
				+ (Username != null && Password != null ? $"User id={Username};Password={Password};" : "");

		public string Server { get; set; }
		public string Database { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		private string _integratedSecurity = "false";

		public string IntegratedSecurity
		{
			get
			{
				return _integratedSecurity;
			}
		}

		public bool SetIntegratedSecurity
		{
			set
			{
				if (value)
					_integratedSecurity = "true";
				else
					_integratedSecurity = "false";
			}
		}

		public MSSQLConnectionParams()
		{

		}

		public MSSQLConnectionParams(string server, string database, string username = null, string password = null)
		{
			Server = server;
			Database = database;
			Username = username;
			Password = password;
		}

		public override string ToString()
		{
			return ConnectionString;
		}
	}
}