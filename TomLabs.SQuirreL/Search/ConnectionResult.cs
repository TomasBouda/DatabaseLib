using System;

namespace TomLabs.SQuirreL.Search
{
	public class ConnectionResult
	{
		public bool Success { get; set; } = true;
		public Exception Exception { get; set; }
	}
}