using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Search
{
	public class ConnectionResult
	{
		public bool Success { get; set; } = true;
		public Exception Exception { get; set; }
	}
}
