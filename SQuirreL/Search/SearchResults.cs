using TomLabs.OpenSource.SQuirreL.Data;
using TomLabs.OpenSource.SQuirreL.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomLabs.OpenSource.SQuirreL.Search
{
	public class SearchResults
	{
		public IList<IDbObject> FoundDbObjects { get; set; }
		public bool Error { get; set; }
		public string ErrorMessage { get; set; }
	}
}
