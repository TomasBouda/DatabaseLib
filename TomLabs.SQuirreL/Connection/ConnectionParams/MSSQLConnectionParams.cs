namespace TomLabs.SQuirreL.Connection.ConnectionParams
{
	public class MSSQLConnectionParams : IConnectionParams
	{
		public string ConnectionString => $"Server={Server};Database={Database};Integrated security={IntegratedSecurityString};"
											+ (Username != null && Password != null ? $"User id={Username};Password={Password};" : "");

		public string Server { get; set; }
		public string Database { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		private string IntegratedSecurityString { get; set; } = "false";
		public bool IntegratedSecurity
		{
			get => IntegratedSecurityString == "true";
			set
			{
				IntegratedSecurityString = value == true ? "true" : "false";
			}
		}

		public MSSQLConnectionParams()
		{

		}

		public MSSQLConnectionParams(string server, string database, string username = null, string password = null, bool integratedSecurity = true)
		{
			Server = server;
			Database = database;
			Username = username;
			Password = password;
			IntegratedSecurity = integratedSecurity;// TODO check username and password
		}

		public override string ToString()
		{
			return ConnectionString;
		}
	}
}