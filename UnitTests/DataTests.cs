using DiabloIISaveLib;
using DiabloIISaveLib.Data;
using DiabloIISaveLib.Versions;
using Serilog;
using Xunit.Abstractions;
using Xunit.Sdk;
using static DiabloIISaveLib.Helpers;

namespace UnitTests
{
	public class DataTests : IDisposable
	{
		string temp_path = "..//..//..//..//DiabloIISaveLib//Save//v99//temp.d2x";

		public DataTests(ITestOutputHelper output)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.Async(a => a.TestOutput(output))
				.WriteTo.Async(a => a.File("log.txt"))
				.CreateLogger();

			Constants.LoadData("..//..//..//..//DiabloIISaveLib");
		}

		[Fact]
		public void Test_ReadAndWriteAtmaStash_ThreeItemStash()
		{
			Log.Verbose("Test_ReadAndWriteAtmaStash_ThreeItemStash");
			string three_item_stash = "..//..//..//..//DiabloIISaveLib//Save//v99//TestStash_ThreeItems.d2x";

			try
			{
				var stash = new AtmaStash_v99(three_item_stash);
				Assert.NotNull(stash);
				Assert.Equal((uint)3687750839, stash.checkSum);
				Assert.Equal(3, stash.item_count);

				stash.Write(temp_path);
				var tempstash = new AtmaStash_v99(temp_path);
				Assert.NotNull(tempstash);
				Assert.Equal((uint)3687750839, stash.checkSum);
				Assert.Equal(3, tempstash.item_count);

				Assert.True(AreEqual(three_item_stash, temp_path));
			}
			finally
			{
				if (File.Exists(temp_path))
					File.Delete(temp_path);
			}
		}

		[Fact]
		public void Test_ReadAndWrite_AtmaStash_ThreeArmors()
		{
			Log.Verbose("Test_ReadAndWrite_AtmaStash_ThreeArmors");
			string three_armors_stash = "..//..//..//..//DiabloIISaveLib//Save//v99//TestStash_ThreeArmors.d2x";

			try
			{
				var stash = new AtmaStash_v99(three_armors_stash);
				Assert.NotNull(stash);
				Assert.Equal((uint)1993806350, stash.checkSum);
				Assert.Equal(3, stash.item_count);

				Item_v99 second_item = stash.item_list.items[1];
				Assert.Equal(61, second_item.current_durability);
				Assert.Equal(61, ItemHelpers.GetTrueMaxDurability(second_item));
				Assert.Equal(391, ItemHelpers.GetTrueDefenseRating(second_item));

				Item_v99 first_item = stash.item_list.items[0];
				Assert.Equal(44, first_item.current_durability);
				Assert.Equal(100, ItemHelpers.GetTrueMaxDurability(first_item));
				Assert.Equal(267, ItemHelpers.GetTrueDefenseRating(first_item));

				Item_v99 third_item = stash.item_list.items[2];
				Assert.Equal(16, third_item.current_durability);
				Assert.Equal(24, ItemHelpers.GetTrueMaxDurability(third_item));
				Assert.Equal(53, ItemHelpers.GetTrueDefenseRating(third_item));

				stash.Write(temp_path);
				Assert.True(AreEqual(three_armors_stash, temp_path));
				var tempstash = new AtmaStash_v99(temp_path);
				Assert.NotNull(tempstash);
				Assert.Equal((uint)1993806350, stash.checkSum);
				Assert.Equal(3, tempstash.item_count);

			}
			finally
			{
				if (File.Exists(temp_path))
					File.Delete(temp_path);
			}
		}

		[Fact]
		public void Test_ReadAndWrite_AtmaStash_EmptyStash()
		{
			Log.Verbose("Test_ReadAndWrite_AtmaStash_EmptyStash");
			string empty_stash = "..//..//..//..//DiabloIISaveLib//Save//v99//EmptyStash.d2x";

			try
			{
				var stash = new AtmaStash_v99(empty_stash);
				Assert.NotNull(stash);
				Assert.Equal((uint)120928, stash.checkSum);
				Assert.Equal(0, stash.item_count);

				stash.Write(temp_path);
				Assert.True(AreEqual(empty_stash, temp_path));
				var tempstash = new AtmaStash_v99(temp_path);
				Assert.Equal((uint)120928, stash.checkSum);
				Assert.NotNull(tempstash);
				Assert.Equal(0, tempstash.item_count);
			}
			finally
			{
				if (File.Exists(temp_path))
					File.Delete(temp_path);
			}
		}

		[Fact]
		public async Task Test_ReadAndWrite_AtmaStash_BigStash()
		{
			await Task.Delay(5000);
			Log.Verbose("Test_ReadAndWrite_AtmaStash_BigStash");
			string big_stash = "..//..//..//..//DiabloIISaveLib//Save//v99//TestStash.d2x";

			try
			{
				var stash = new AtmaStash_v99(big_stash);
				
				Assert.NotNull(stash);
				Assert.Equal(2788771406, stash.checkSum);
				Assert.Equal(957, stash.item_count);

				stash.Write(temp_path);
				var tempstash = new AtmaStash_v99(temp_path);
				Assert.NotNull(tempstash);
				Assert.Equal(2788771406, tempstash.checkSum);
				Assert.Equal(957, tempstash.item_count);

				Assert.True(AreEqual(big_stash, temp_path));
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		[Fact]
		public void Test_ReadCharacter()
		{
			Log.Verbose("Test_ReadAndWrite_AtmaStash_EmptyStash");
			string character_path = "..//..//..//..//DiabloIISaveLib//Save//v99//Yennefer.d2s";

			try
			{
				var character = new Character_v99(character_path);
				Assert.NotNull(character);
				Assert.Equal((uint)120928, character.header.checksum);
				//Assert.Equal(0, stash.item_count);

				//stash.Write(temp_path);
				//Assert.True(AreEqual(empty_stash, temp_path));
				//var tempstash = new AtmaStash_v99(temp_path);
				//Assert.Equal((uint)120928, stash.checkSum);
				//Assert.NotNull(tempstash);
				//Assert.Equal(0, tempstash.item_count);
			}
			finally
			{
				if (File.Exists(temp_path))
					File.Delete(temp_path);
			}
		}

		public void Dispose()
		{
			if (File.Exists(temp_path))
				File.Delete(temp_path);
		}

		private bool AreEqual(string path1, string path2)
		{
			if (!File.Exists(path1))
				throw new FileNotFoundException($"File not found. Path: {path1}");
			if (!File.Exists(path2))
				throw new FileNotFoundException($"File not found. Path: {path2}");

			byte[] bytes1 = File.ReadAllBytes(path1);
			byte[] bytes2 = File.ReadAllBytes(path2);

			for (int i = 0; i < Math.Max(bytes1.Length, bytes2.Length); i++)
			{
				if (bytes1[i] != bytes2[i])
				{
					Log.Verbose($"Path 1: {path1}. Data length: {bytes1.Length}");
					Log.Verbose($"Path 2: {path2}. Data length: {bytes2.Length}");
				}

				Assert.Equal(bytes1[i], bytes2[i]);
			}

			Assert.Equal(bytes1.Length, bytes2.Length);
			Assert.Equal(bytes1, bytes2);

			return true;
		}
	}
}
