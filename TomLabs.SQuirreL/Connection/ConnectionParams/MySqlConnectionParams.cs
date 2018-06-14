namespace TomLabs.SQuirreL.Connection.ConnectionParams
{
	public class MySqlConnectionParams : IConnectionParams
	{
		public string Server { get; set; }
		public string Database { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		public string ConnectionString => throw new System.NotImplementedException();
	}
}