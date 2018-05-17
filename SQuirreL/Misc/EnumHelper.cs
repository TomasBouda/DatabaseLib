using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TomLabs.SQuirreL.Misc
{
	public static class EnumHelper<T> where T : struct
	{
		public static string GetEnumDescription(string value)
		{
			Type type = typeof(T);
			var name = Enum.GetNames(type).Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();

			if (name == null)
			{
				return string.Empty;
			}
			var field = type.GetField(name);
			var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
			return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
		}

		public static Dictionary<int, string> ToDictionary()
		{
			if (!typeof(T).IsEnum)
				throw new ArgumentException("Type T must be an enum.");

			return Enum.GetValues(typeof(T))
			   .Cast<T>()
			   .ToDictionary(t => (int)Convert.ChangeType(t, t.GetType()), t => t.ToString());
		}
	}
}