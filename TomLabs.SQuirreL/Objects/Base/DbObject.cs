using System;
using System.Diagnostics;
using TomLabs.SQuirreL.DataProviders;

namespace TomLabs.SQuirreL.Data
{
	public abstract class DbObject<T> : IDbObject where T : class, IDB, new()
	{
		protected T DB { get; set; }

		public string Schema { get; protected set; }

		public string Name { get; protected set; }

		public int NumberOfRows { get; protected set; }

		private string _script = null;
		public string Script
		{
			get
			{
				if (_script == null)
				{
					LoadScript(DB);
				}

				return _script;
			}
			protected set { _script = value; }
		}

		protected readonly Func<string> GetScript;

		public bool IsLoaded { get; set; }

		public DbObject()
		{
		}

		public DbObject(string schema, string name, T db, Func<string> getScriptMethod)
		{
			Schema = schema;
			Name = name;
			DB = db;
			GetScript = getScriptMethod;
		}

		public virtual bool LoadScript()
		{
			return LoadScript(DB);
		}

		protected virtual bool LoadScript(T db)
		{
			try
			{
				Script = GetScript();
				IsLoaded = true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				IsLoaded = false;
			}

			return IsLoaded;
		}

		public override string ToString()
		{
			return Schema + "." + Name;
		}
	}
}