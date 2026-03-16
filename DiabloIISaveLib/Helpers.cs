using Serilog;

namespace DiabloIISaveLib
{
	public static class Helpers
	{
		public static ushort? ParseUshort(string? s)
		{
			if (ushort.TryParse(s, out ushort result))
				return result;
			return null;
		}
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

		public static void ThrowInvalidOperationException(string message)
		{
			var ex = new InvalidOperationException(message);
			Log.Error(ex, message);
			throw ex;
		}
	}
}
