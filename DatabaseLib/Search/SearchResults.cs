using Database.Lib.Data;
using Database.Lib.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Search
{
	public class SearchResults<T> where T : class, IDB<T>, new()
	{
		public IList<IDbObject<T>> FoundDbObjects { get; set; }
		public bool Error { get; set; }
		public string ErrorMessage { get; set; }
	}
}
