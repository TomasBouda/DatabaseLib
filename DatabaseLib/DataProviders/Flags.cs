using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.DataProviders
{
	class Flags { }

	[Flags]
	public enum EDbObjects : int
	{
		None = 0,
		Tables = 1,
		Columns = 2,
		Views = 4,
		StoredProcedures = 8,
		//Triggers = 16,

		All = Tables | Views | StoredProcedures | Columns /*| Triggers*/
	}
}
