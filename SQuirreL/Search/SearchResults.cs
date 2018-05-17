using System.Collections.Generic;
using TomLabs.SQuirreL.Data;

namespace TomLabs.SQuirreL.Search
{
	public class SearchResults
	{
		public IList<IDbObject> FoundDbObjects { get; set; }
		public bool Error { get; set; }
		public string ErrorMessage { get; set; }
	}
}