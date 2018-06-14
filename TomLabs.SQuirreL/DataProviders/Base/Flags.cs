using System;

namespace TomLabs.SQuirreL.DataProviders
{
	[Flags]
	public enum ObjectTypes : byte
	{
		None = 0,
		Tables = 1,
		Columns = 2,
		Views = 4,
		StoredProcedures = 8,
		//Triggers = 16,

		All = Tables | Views | StoredProcedures | Columns /*| Triggers*/
	}

	public enum Sort
	{
		None,
		Asc,
		Desc
	}
}