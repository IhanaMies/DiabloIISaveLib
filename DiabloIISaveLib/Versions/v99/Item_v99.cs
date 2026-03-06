using D2SLib;
using D2SLib.IO;
using DiabloIISaveLib.Constants.v99;
using DiabloIISaveLib.Huffman;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace DiabloIISaveLib.Versions.v99
{
	public class Item_v99
	{
		enum ItemType
		{
			Armor = 0x01,
			Shield = 0x02, //treated the same as armor... only here to be able to parse nokkas jsons
			Weapon = 0x03,
			Other = 0x04,
		}

		enum ItemQuality
		{
			None = 0x00,
			Low = 0x01,
			Normal = 0x02,
			Superior = 0x03,
			Magic = 0x04,
			Set = 0x05,
			Rare = 0x06,
			Unique = 0x07,
			Crafted = 0x08,
		}

		public static string GetCategory(string val)
		{
			if (Data.itemTypes.TryGetValue(val, out var category))
			{
				_ = 3;
			}

			return "a";
		}

		public static Item_v99 Read(BitReader reader)
		{
			var unknown_1 = reader.ReadBits(4);
			bool identified = reader.ReadBit();
			var unknown_2 = reader.ReadBits(6);
			bool socketed = reader.ReadBit();
			var unknown_3 = reader.ReadBits(1);
			bool newItem = reader.ReadBit();
			var unknown_4 = reader.ReadBits(2);
			bool isEar = reader.ReadBit();
			bool starterItem = reader.ReadBit();
			var unknown_5 = reader.ReadBits(3);
			bool simple_item = reader.ReadBit();
			bool ethereal = reader.ReadBit();
			var unknown_6 = reader.ReadBits(1);
			bool personalized = reader.ReadBit();
			var unknown_7 = reader.ReadBits(1);
			bool given_runeword = reader.ReadBit();
			var unknown_8 = reader.ReadBits(5);

			ushort version = reader.ReadUInt16(3);
			byte location_id = reader.ReadByte(3);
			byte equipped_id = reader.ReadByte(4);
			byte position_x = reader.ReadByte(4);
			byte position_y = reader.ReadByte(4);
			byte alt_position_id = reader.ReadByte(3);

			if (isEar)
			{
				byte class_of_ear = reader.ReadByte(3);
				byte level_of_ear = reader.ReadByte(7);
				string name_of_ear = reader.ReadString(15);
			}
			else
			{
				HuffmanTree tree = new HuffmanTree();
				tree.Build();

				Span<byte> codeBuffer = stackalloc byte[4];
				for (int i = 0; i < 4; i++)
				{
					codeBuffer[i] = tree.DecodeChar(reader);
				}
				string item_code = Encoding.ASCII.GetString(codeBuffer).Replace(" ", "");

				var item_category = GetCategory(item_code);

				int bits = simple_item ? 1:3;
				int number_of_sockets = reader.ReadByte(bits);

				if (!simple_item)
				{
					uint item_id = reader.ReadUInt32(32);
					byte item_level = reader.ReadByte(7);
					byte item_quality = reader.ReadByte(4);

					ItemQuality quality = (ItemQuality)item_quality;

					bool multiple_pictures = reader.ReadBit();
					if (multiple_pictures)
					{
						byte picture_id = reader.ReadByte(3);
					}
					bool class_specific = reader.ReadBit();
					if (class_specific)
					{
						ushort auto_affix_id = reader.ReadUInt16(11);
					}

					switch (quality)
					{
						case ItemQuality.Low:
						{
							byte low_quality_id = reader.ReadByte(3);
							break;
						}
						case ItemQuality.Normal:
						{
							// when a normal item has magic properties
							// (which is not possible in vanilla Diablo 2)
							// we end up with an extra 12 bits here whose
							// purpose we do not know
							// the bits seem to either be all 0s or all 1s
							BitArray? n_bits = reader.ReadBitArray(12);
							if (n_bits.HasAllSet() || n_bits.Not().HasAllSet())
							{
								BitArray? item_normal_12_bits = n_bits;
							}
							else
							{
								BitArray? item_normal_12_bits = null;
								reader.AdvanceBits(-12);
							}

							break;
						}
						case ItemQuality.Superior:
						{
							byte file_index = reader.ReadByte(3);
							break;
						}
						case ItemQuality.Magic:
						{
							ushort magic_prefix = reader.ReadUInt16(11);
							if (magic_prefix > 0)
							{
								//get constant data for magic_prefix
							}

							ushort magic_suffix = reader.ReadUInt16(11);
							if (magic_suffix > 0)
							{
								//get constant data for magic_suffix
							}
							break;
						}
						case ItemQuality.Set:
						{
							ushort set_id = reader.ReadUInt16(12);
							break;
						}
						case ItemQuality.Unique:
						{
							ushort unique_id = reader.ReadUInt16(12);
							break;
						}
						case ItemQuality.Rare:
						case ItemQuality.Crafted:
						{
							ushort rare_name_id = reader.ReadByte(8);
							ushort rare_name_id2 = reader.ReadByte(8);

							List<ushort?> magical_name_ids = new();
							for (int i = 0; i < 6; i++)
							{
								if (reader.ReadBit())
								{
									magical_name_ids.Add(reader.ReadUInt16());
								}
								else
								{
									magical_name_ids.Add(null);
								}
							}
							break;
						}
					}

					if (given_runeword)
					{
						ushort runeword_id = reader.ReadUInt16(12);
						byte runeword_extra = reader.ReadByte(4);
					}

					if (personalized)
					{
						StringBuilder sb = new();
						for (int i = 0; i < 16; i++)
						{
							char c = (char)reader.ReadByte(8);
							//for versions <= 0x60 read 7 bytes
							sb.Append((char)reader.ReadByte(8));
						}
						string personalized_name = sb.ToString();
					}

					if (item_code == "tbk" || item_code == "ibk")
					{
						byte tome_unknown_data = reader.ReadByte(5);
					}

					byte timestamp = reader.ReadByte(1);

					if (true /*item.type*/)
					{

					}
				}
			}

			return new Item_v99();
		}
	}
}
