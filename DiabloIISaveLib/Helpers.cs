using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DiabloIISaveLib
{
	public static class Helpers
	{
		public static int? ParseInt(string? s)
		{
			if (int.TryParse(s, out int result))
				return result;
			return null;
		}

		public static bool? ParseBool(string? s)
		{
			int? val = ParseInt(s);
			if (val != null && (val == 0 || val == 1))
				return val != 0;

			if (bool.TryParse(s, out bool result))
				return result;
			return null;
		}
		public static Nullable<T> ToNullable<T>(this string s) where T : struct
		{
			Nullable<T> result = new Nullable<T>();
			try
			{
				if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
				{
					TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
					result = (T)conv.ConvertFrom(s);
				}
			}
			catch { }
			return result;
		}
	}
}
