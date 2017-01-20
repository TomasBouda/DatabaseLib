using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.DataProviders.ConnectionParams
{
	public interface IConnectionParams<T> where T : class, IDB<T>, new()
	{

	}
}
