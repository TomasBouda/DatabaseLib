using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.DataProviders.ConnectionParams
{
	public enum EIntegratedSecurity
	{
		@true,
		@false
		//yes = @true,
		//no = @false,
		//sspi = @true
	}


	public class MSSQLConnectionParams : IConnectionParams<MSSQL>
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

		public EIntegratedSecurity SetIntegratedSecurity
		{
			set
			{
				if (value == EIntegratedSecurity.@true)
					_integratedSecurity = "true";
				else
					_integratedSecurity = "false";
			}
		}
	}
}
