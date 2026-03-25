using DiabloIISaveLib.IO;
using Serilog;
using System.Buffers.Binary;
using System.Reflection.PortableExecutable;
using System.Text;

namespace DiabloIISaveLib.Data
{
	public class AtmaStash_v99
	{
		public string header { get; }
		public ushort item_count { get; }
		public ushort version { get; }
		public uint checkSum { get; }
		public ItemList_v99 item_list { get; set; }

		public AtmaStash_v99(string path)
		{
			if (!Path.Exists(path))
				throw new FileNotFoundException($"Atma stash does not exist ({path})");

			byte[] bytes = File.ReadAllBytes(path);
			using var reader = new BitReader(bytes);

			header = Encoding.Default.GetString(reader.ReadBytes(3));
			Log.Verbose($"Read header ({header}). 24 bits. Position: {reader.Position}");

			if (header != "D2X")
			{
				throw new InvalidOperationException($"Invalid Atma Stash header. Expected 'D2X'. Was '{header}'");
			}

			item_count = reader.ReadUInt16();
			Log.Verbose($"Read item_count ({item_count}). 16 bits. Position: {reader.Position}");
			version = reader.ReadUInt16();
			Log.Verbose($"Read version ({version}). 16 bits. Position: {reader.Position}");
			checkSum = reader.ReadUInt32();
			Log.Verbose($"Read check_sum ({checkSum}). 32 bits. Position: {reader.Position}");
			byte[] checksum_bytes = BitConverter.GetBytes(checkSum);
			Log.Verbose($"check_sum bytes [{checksum_bytes[0]}, {checksum_bytes[1]}, {checksum_bytes[2]}, {checksum_bytes[3]}]. 32 bits. Position: {reader.Position}");

			item_list = ItemList_v99.Read(reader, version, item_count);
		}

		public bool Write(string path)
		{
			try
			{
				using var writer = new BitWriter();
				writer.WriteString("D2X", 3);
				Log.Verbose($"Write header. 24 bits. Position: {writer.Position}");

				writer.WriteUInt16((ushort)item_list.items.Count);
				Log.Verbose($"Write item_count. 16 bits. Position: {writer.Position}");

				writer.WriteUInt16(version);
				Log.Verbose($"Write version. 16 bits. Position: {writer.Position}");

				writer.WriteUInt32(0, 32);
				Log.Verbose($"Write empty check_sum. 32 bits. Position: {writer.Position}");

				int item_list_start_position = writer.Position;

				item_list.Write(writer, version, false);
				ulong check_sum = CheckSum(writer.ToArray());

				writer.SeekBits(item_list_start_position - 32);
				writer.WriteUInt64(check_sum, CHECKSUM_BYTE_LENGTH*8);
				Log.Verbose($"Write check_sum ({check_sum}). 32 bits. Position: {writer.Position}");

				byte[] bytes = writer.ToArray();

				File.WriteAllBytes(path, bytes);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Failed to save Atma Stash v99 {path}", path);
			}

			return true;
		}

		public const int CHECKSUM_BYTE_OFFSET_START = 7; //9 for 105 (RotW)
		public const int CHECKSUM_BYTE_LENGTH = 4;

		private ulong calculateAtmaCheckSum_GoMuleVersio(byte[] bytes)
		{
			using BitReader r = new(bytes);

			long lCheckSum = 0;

			for (int i = 0; i < bytes.Length; i++)
			{
				long lByte = r.ReadInt64(8);
				if (i >= CHECKSUM_BYTE_OFFSET_START && i < (CHECKSUM_BYTE_OFFSET_START + CHECKSUM_BYTE_LENGTH))
				{
					lByte = 0;
				}

				long upshift = lCheckSum << 33 >>> 32;
				long add = lByte + ((lCheckSum >>> 31) == 1 ? 1 : 0);
				lCheckSum = upshift + add;
			}

			return (ulong)lCheckSum;
		}

		private uint CheckSum(byte[] bytes)
		{
			using BitReader r = new(bytes);
			uint checksum = 0;
			for (int i = 0; i < bytes.Length; i++)
			{
				checksum = (checksum << 1) | (checksum >>> 31);
				checksum += r.ReadUInt32(8);
			}
			return checksum;
		}
	}
}
