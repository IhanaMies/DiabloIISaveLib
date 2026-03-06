using D2SLib.IO;
using System.Buffers.Binary;
using System.Text;

namespace DiabloIISaveLib.Versions.v99
{
	public class AtmaStash_v99
	{
		public string header { get; }
		public ushort itemCount { get; }
		public ushort version { get; }
		public uint checkSum { get; }
		public List<Item_v99> items { get; set; }

		public AtmaStash_v99(string path)
		{
			items = new List<Item_v99>();

			using MemoryStream ms = new MemoryStream();
			using Stream s = File.Open(path, FileMode.Open);
			s.CopyTo(ms);
			s.Dispose();
			s.Close();

			using BinaryReader reader = new(ms);
			ms.Position = 0;
			header = Encoding.Default.GetString(reader.ReadBytes(3));

			if (header != "D2X")
			{
				throw new InvalidOperationException($"Invalid Atma Stash header. Expected 'D2X'. Was '{header}'");
			}

			itemCount = reader.ReadUInt16();
			version = reader.ReadUInt16();
			checkSum = reader.ReadUInt32();

			string hexPosition = $"0x{reader.BaseStream.Position:X2}";

			var bitReader = new BitReader(ms.ToArray());
			bitReader.Seek((int)reader.BaseStream.Position);

			//read item list
			for (int i = 0; i < itemCount; i++)
			{
				items.Add(Item_v99.Read(bitReader));
			}
		}

		static ushort ReverseBits(ushort n)
		{
			ushort ans = 0;

			// traversing bits from right to left
			while (n > 0)
			{
				// bitwise left shift by 1
				ans <<= 1;

				// if current bit is '1'
				if ((n & 1) == 1)
					ans |= 1;

				// bitwise right shift by 1
				n >>= 1;
			}

			// required number
			return ans;
		}
	}
}
