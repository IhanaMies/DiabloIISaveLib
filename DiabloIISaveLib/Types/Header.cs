using DiabloIISaveLib.IO;
using Serilog;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace DiabloIISaveLib.Types
{
	public class Header
	{
		//0x0000
		public uint? magic { get; set; }
		//0x0004
		public int version { get; set; }
		//0x0008
		public uint filesize { get; set; }
		//0x000c
		public uint checksum { get; set; }

		public void Write(IBitWriter writer)
		{
			writer.WriteUInt32(magic ?? 0xAA55AA55);
			writer.WriteUInt32((uint)version);
			writer.WriteUInt32(filesize);
			writer.WriteUInt32(checksum);
		}

		public static Header Read(IBitReader reader)
		{
			var header = new Header();
			header.magic = reader.ReadUInt32();
			Log.Verbose($"Read header.magic ({header.magic}). 32 bits. Position: {reader.Position}");
			header.version = (int)reader.ReadUInt32();
			Log.Verbose($"Read header.version ({header.version}). 32 bits. Position: {reader.Position}");
			header.filesize = reader.ReadUInt32();
			Log.Verbose($"Read header.filesize ({header.filesize}). 32 bits. Position: {reader.Position}");
			header.checksum = reader.ReadUInt32();
			Log.Verbose($"Read header.checksum ({header.checksum}). 32 bits. Position: {reader.Position}");

			string magicHex = ((int)(header.magic)).ToString("X");

			if (magicHex != "AA55AA55")
			{
				throw new InvalidOperationException($"Invalid D2S file identifier. Was: {magicHex} Expected: AA55AA55");
			}
			return header;
		}

		public static void Fix(Span<byte> bytes)
		{
			FixSize(bytes);
			FixChecksum(bytes);
		}

		private static void FixSize(Span<byte> bytes)
		{
			BinaryPrimitives.WriteInt32LittleEndian(bytes[0x8..0xC], bytes.Length);
		}

		private static void FixChecksum(Span<byte> bytes)
		{
			BinaryPrimitives.WriteInt32LittleEndian(bytes[0xC..0x10], 0);
			int checksum = 0;
			for (int i = 0; i < bytes.Length; i++)
			{
				checksum = bytes[i] + (checksum * 2) + (checksum < 0 ? 1 : 0);
			}
			BinaryPrimitives.WriteInt32LittleEndian(bytes[0xC..0x10], checksum);
		}
	}
}
