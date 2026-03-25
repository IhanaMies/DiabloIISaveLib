using Xunit.Sdk;
using static DiabloIISaveLib.Helpers;

namespace UnitTests
{
	public class HelpersTests
	{
		[Theory]
		[InlineData(null, null)]
		[InlineData("", null)]
		[InlineData("0", (ushort)0)]
		[InlineData("1", (ushort)1)]
		[InlineData("65,034", null)] //wrong culture
		[InlineData("65535", (ushort)65535)] //max ushort value
		[InlineData("-1", null)] //one underflow
		[InlineData("65536", null)] //one overflow
		public void Test_ParseUshort(string? s, ushort? expected)
		{
			Assert.Equal(expected, ParseUshort(s));
		}

		[Theory]
		[InlineData(null, null)]
		[InlineData("", null)]
		[InlineData("-1", -1)]
		[InlineData("0", 0)]
		[InlineData("1", 1)]
		[InlineData("-2,147,483,648", null)] //wrong culture
		[InlineData("2,147,483,647", null)] //wrong culture
		[InlineData("-2147483648", -2_147_483_648)] //min int value
		[InlineData("2147483647", 2_147_483_647)] //max int value
		[InlineData("-2147483649", null)] //one underflow
		[InlineData("2147483648", null)] //one overflow
		public void Test_ParseInt(string? s, int? expected)
		{
			Assert.Equal(expected, ParseInt(s));
		}

		[Theory]
		[InlineData(null, null)]
		[InlineData("", null)]
		[InlineData("-1", null)]
		[InlineData("0", false)]
		[InlineData("1", true)]
		[InlineData("false", false)]
		[InlineData("true", true)]
		[InlineData("False", false)]
		[InlineData("True", true)]
		public void Test_ParseBool(string? s, bool? expected)
		{
			Assert.Equal(expected, ParseBool(s));
		}

		[Theory]
		[InlineData("..//..//..//..//DiabloIISaveLib")]
		public void Test_DataPath(string root)
		{
			var a = Path.GetFullPath(root);
			Assert.True(Directory.Exists(root));
		}
	}
}
