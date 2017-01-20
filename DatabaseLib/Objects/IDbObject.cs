using Database.Lib.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Data
{
	public interface IDbObject<T> where T : class, IDB<T>, new()
	{
		string Name { get; set; }

		bool Load(T db);
	}
}
