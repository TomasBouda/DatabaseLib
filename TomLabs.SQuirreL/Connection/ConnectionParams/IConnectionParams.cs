namespace TomLabs.SQuirreL.Connection.ConnectionParams
{
	public interface IConnectionParams
	{
		string ConnectionString { get; }

		string Server { get; set; }
		string Database { get; set; }
		string Username { get; set; }
		string Password { get; set; }
	}
}