using System;
using System.Collections.Generic;
using System.Text;
using TomLabs.SQuirreL.Data;

namespace TomLabs.SQuirreL.Objects.Base
{
    public interface IScript
    {
		List<IDbObject> Affects { get; }
	}
}
