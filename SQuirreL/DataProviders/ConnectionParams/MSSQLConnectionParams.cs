namespace TomLabs.SQuirreL.DataProviders.ConnectionParams
{
	public class MSSQLConnectionParams : IConnectionParams
	{
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
	}
}