using DiabloIISaveLib.Huffman;
using DiabloIISaveLib.IO;
using Serilog;
using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text;

namespace DiabloIISaveLib.Types
{
	public class Ear : Item
	{
		public byte class_of_ear { get; set; }
		public byte level_of_ear { get; set; }
		public string? name_of_ear { get; set; }
	}

	public class Item
	{
		public enum ItemType
		{
			Armor = 0x01,
			Shield = 0x02, //treated the same as armor... only here to be able to parse nokkas jsons
			Weapon = 0x03,
			Other = 0x04,
		}

		public enum ItemQuality
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

		[Flags]
		public enum ItemFlags : uint
		{
			None = 0,
			NewItem = 0x00000001,
			Target = 0x00000002,
			Targeting = 0x00000004,
			Deleted = 0x00000008,
			Identified = 0x00000010,
			Quantity = 0x00000020,
			SwitchIn = 0x00000040,
			SwitchOut = 0x00000080,
			Broken = 0x00000100,
			Repaired = 0x00000200,
			Unk1 = 0x00000400,
			Socketed = 0x00000800,
			NoSell = 0x00001000,
			InStore = 0x00002000,
			NoEquip = 0x00004000,
			Named = 0x00008000,
			IsEar = 0x00010000,
			Startitem = 0x00020000,
			Unk2 = 0x00040000,
			Init = 0x00080000,
			Unk3 = 0x00100000,
			CompactSave = 0x00200000,
			Ethereal = 0x00400000,
			JustSaved = 0x00800000,
			Personalized = 0x01000000,
			LowQuality = 0x02000000,
			Runeword = 0x04000000,
			Item = 0x08000000,
			Unk4 = 0x10000000,  // New data block present (version > 99)
			Unk5 = 0x20000000,  // Modifies new data block format
		}

		public string name { get; private set; }
		public ItemFlags flags { get; private set; }
		public ushort version { get; private set; }
		public byte location_id { get; private set; }
		public byte equipped_id { get; private set; }
		public byte position_x { get; private set; }
		public byte position_y { get; private set; }
		public byte alt_position_id { get; private set; }
		public string? code { get; private set; }
		public string? category { get; private set; }
		public bool compact_item { get; private set; }
		public byte quest_difficulty { get; private set; }
		public byte used_sockets { get; private set; }
		public uint id { get; private set; }
		public byte level { get; private set; }
		public ItemQuality quality { get; private set; }
		public bool multiple_pictures { get; private set; }
		public byte picture_id { get; private set; }
		public bool class_specific { get; private set; }
		public ushort auto_affix_id { get; private set; }
		public uint file_index { get; private set; }
		public ushort rare_prefix_id { get; private set; }
		public ushort rare_suffix_id { get; private set; }
		public List<ushort> magic_prefix_ids { get; private set; } = new();
		public List<ushort> magic_suffix_ids { get; private set; } = new();
		public uint runeword_id { get; private set; }
		public byte runeword_statlist_index { get; private set; }
		public string? personalized_name { get; private set; }
		public byte[]? realm_data { get; private set; }
		public EItemType type { get; private set; } = EItemType.Undefined;
		public int? defense_rating { get; private set; }
		public int? max_durability { get; private set; }
		public int? current_durability { get; private set; }
		public bool stackable { get; private set; } = false;
		public int quantity { get; private set; }
		public byte total_nr_of_sockets { get; private set; }
		public byte set_item_mask { get; private set; } = 0;
		public List<List<ItemModifier>> modifiers { get; private set; } = new();
		public List<Item> socketed_items { get; private set; } = new();
		public bool yes_bit { get; private set; }
		public byte yes_bit_bits { get; private set; }
		public bool yes_102_103 { get; private set; }
		public byte yes_102_103_data { get; private set; }

		public static string GetCategory(string val)
		{
			if (Constants.itemTypes.TryGetValue(val, out var category))
			{
				_ = 3;
			}

			return "a";
		}

		public void Write(IBitWriter writer, int version)
		{
			int pos = writer.Position;
			WriteCompact(writer, version);

			if (!compact_item)
				WriteComplete(writer, version);

			WriteExtraData(writer, version);
			int old_pos = writer.Position;
			writer.Align();
			Log.Verbose($"Align writer from {old_pos} to {writer.Position}");
		}

		public void WriteCompact(IBitWriter writer, int version)
		{
			writer.WriteUInt32((uint)flags);
			Log.Verbose($"Write flags. 32 bits. Position: {writer.Position}. {flags}");
			writer.WriteUInt16(this.version + 5, 3);
			Log.Verbose($"Write version. 3 bits. Position: {writer.Position}");
			writer.WriteByte(location_id, 3);
			Log.Verbose($"Write location_id. 3 bits. Position: {writer.Position}");
			writer.WriteByte(equipped_id, 4);
			Log.Verbose($"Write equipped_id. 4 bits. Position: {writer.Position}");
			writer.WriteByte(position_x, 4);
			Log.Verbose($"Write position_x. 4 bits. Position: {writer.Position}");
			writer.WriteByte(position_y, 4);
			Log.Verbose($"Write position_y. 4 bits. Position: {writer.Position}");
			writer.WriteByte(alt_position_id, 3);
			Log.Verbose($"Write alt_position_id. 3 bits. Position: {writer.Position}");

			if ((flags & ItemFlags.IsEar) != 0)
			{
				Ear ear = (Ear)this;
				writer.WriteByte(ear.class_of_ear, 3);
				Log.Verbose($"Write ear class. 3 bits. Position: {writer.Position}");
				writer.WriteByte(ear.level_of_ear, 3);
				Log.Verbose($"Write ear level. 3 bits. Position: {writer.Position}");
				writer.WriteString(ear.name_of_ear, 15);
				Log.Verbose($"Write ear name. 15 bits. Position: {writer.Position}");
			}
			else
			{
				var itemCode = code!.PadRight(4, ' ');
				Span<byte> codespan = stackalloc byte[itemCode.Length];
				Encoding.ASCII.GetBytes(itemCode, codespan);

				for (int i = 0; i < 4; i++)
				{
					var (bits, length) = Constants.item_code_tree!.GetEncodedBits((char)itemCode[i]);
					writer.WriteBits(bits, length);
				}
				Log.Verbose($"Write code. 32 bits. Position: {writer.Position}");

				int num_socket_bits = compact_item ? 1 : 3;
				if (code == "ques")
				{
					var questDiffStat = Constants.item_stat_costs["questitemdifficulty"];
					writer.WriteInt32((quest_difficulty + questDiffStat?.save_add ?? 0) >> questDiffStat.val_shift ?? 0, questDiffStat?.save_bits ?? 0);
					Log.Verbose($"Write quest difficulty. 32 bits. Position: {writer.Position}");
					num_socket_bits = 1;
				}

				writer.WriteByte(used_sockets, num_socket_bits);
				Log.Verbose($"Write used_sockets. {num_socket_bits} bits. Position: {writer.Position}");
			}
		}

		public void WriteComplete(IBitWriter writer, int version)
		{
			writer.WriteUInt32(id);
			Log.Verbose($"Write id. 32 bits. Position: {writer.Position}");
			writer.WriteByte(level, 7);
			Log.Verbose($"Write level. 7 bits. Position: {writer.Position}");
			writer.WriteByte((byte)quality, 4);
			Log.Verbose($"Write quality. 4 bits. Position: {writer.Position}");
			writer.WriteBit(multiple_pictures);
			Log.Verbose($"Write multiple_pictures. 1 bit. Position: {writer.Position}");
			if (multiple_pictures)
			{
				writer.WriteByte(picture_id, 3);
				Log.Verbose($"Write picture_id. 3 bits. Position: {writer.Position}");
			}
			writer.WriteBit(class_specific);
			Log.Verbose($"Write class_specific. 1 bit. Position: {writer.Position}");
			if (class_specific)
			{
				writer.WriteUInt16(auto_affix_id, 11);
				Log.Verbose($"Write auto_affix_id. 11 bits. Position: {writer.Position}");
			}
			switch (quality)
			{
				case ItemQuality.Low:
				case ItemQuality.Superior:
				{
					writer.WriteByte((byte)file_index, 3);
					Log.Verbose($"Write file_index. 3 bits. Position: {writer.Position}");
					break;
				}
				case ItemQuality.Normal:
				{
					if (code == "scro" || code == "book") // item type 22 or 18
					{
						writer.WriteUInt16(magic_suffix_ids[0], 5);
						Log.Verbose($"Write scroll/book data. 5 bits. Position: {writer.Position}");
					}

					break;
				}
				case ItemQuality.Magic:
				{
					writer.WriteUInt16(magic_prefix_ids[0], 11);
					Log.Verbose($"Write magic_prefix_id. 11 bits. Position: {writer.Position}");
					writer.WriteUInt16(magic_suffix_ids[0], 11);
					Log.Verbose($"Write magic_suffix_id. 11 bits. Position: {writer.Position}");
					break;
				}
				case ItemQuality.Set:
				case ItemQuality.Unique:
				{
					writer.WriteUInt16(file_index, 12);
					Log.Verbose($"Write file_index. 12 bits. Position: {writer.Position}");
					break;
				}
				case ItemQuality.Rare:
				case ItemQuality.Crafted:
				{
					writer.WriteUInt16(rare_prefix_id, 8);
					Log.Verbose($"Write rare_prefix_id. 8 bits. Position: {writer.Position}");
					writer.WriteUInt16(rare_suffix_id, 8);
					Log.Verbose($"Write rare_suffix_id. 8 bits. Position: {writer.Position}");

					for (int i = 0; i < 3; i++)
					{
						bool has_prefix = magic_prefix_ids.Count > i;
						bool has_suffix = magic_suffix_ids.Count > i;
						writer.WriteBit(has_prefix);
						Log.Verbose($"Write has_prefix. 1 bit. Position: {writer.Position}");
						if (has_prefix)
						{
							writer.WriteUInt16(magic_prefix_ids[i], 11);
							Log.Verbose($"Write magic_prefix_id[{i}]. 11 bits. Position: {writer.Position}");
						}
						writer.WriteBit(has_suffix);
						Log.Verbose($"Write has_suffix. 1 bit. Position: {writer.Position}");
						if (has_suffix)
						{
							writer.WriteUInt16(magic_suffix_ids[i], 11);
							Log.Verbose($"Write magic_suffix_id[{i}]. 11 bits. Position: {writer.Position}");
						}
					}
					break;
				}
			}

			ushort set_bonuses = 0;

			if ((flags & ItemFlags.Runeword) != 0)
			{
				writer.WriteUInt32(runeword_id, 12);
				Log.Verbose($"Write runeword_id. 12 bits. Position: {writer.Position}");
				writer.WriteByte(runeword_statlist_index, 4);
				Log.Verbose($"Write runeword_statlist_index. 4 bits. Position: {writer.Position}");
				//learn wtf this bithack magic does
				set_bonuses |= (ushort)(1 << (runeword_statlist_index + 1));
			}

			if ((flags & ItemFlags.Personalized) != 0)
			{
				int old_position = writer.Position;
				WritePlayerName(writer, personalized_name!, 99);
				Log.Verbose($"Write personalized_name. {writer.Position - old_position} bits. Position: {writer.Position}");
			}

			bool has_realm_data = realm_data?.Length > 0;
			writer.WriteBit(has_realm_data);
			Log.Verbose($"Write has_realm_data. 1 bit. Position: {writer.Position}");

			if (has_realm_data)
			{
				writer.WriteBytes(realm_data, 16);
				Log.Verbose($"Write realm_data. 16 bits. Position: {writer.Position}");
			}

			if (type == EItemType.Armor)
			{
				var armorclass_data = Constants.item_stat_costs["armorclass"];
				writer.WriteUInt16(
					(ushort)(defense_rating! + armorclass_data.save_add!.Value),
					(ushort)armorclass_data.save_bits!.Value
				);
				Log.Verbose($"Write defense_rating. {(ushort)armorclass_data.save_bits!.Value} bits. Position: {writer.Position}");
			}
			if (type == EItemType.Armor || type == EItemType.Weapon)
			{
				var maxdurability_data = Constants.item_stat_costs["maxdurability"];
				var durability_data = Constants.item_stat_costs["durability"];

				writer.WriteUInt16(
					max_durability!.Value + maxdurability_data.save_add!.Value,
					maxdurability_data.save_bits!.Value
				);
				Log.Verbose($"Write max_durabilitity. {maxdurability_data.save_bits!.Value} bits. Position: {writer.Position}");

				if (max_durability > 0)
				{
					writer.WriteUInt16(
						current_durability!.Value + durability_data.save_add!.Value,
						durability_data.save_bits!.Value
					);
					Log.Verbose($"Write current_durability. {durability_data!.save_bits.Value} bits. Position: {writer.Position}");
				}
			}

			if (stackable)
			{
				writer.WriteUInt16(quantity, 9);
				Log.Verbose($"Write quantity. 9 bits. Position: {writer.Position}");
			}

			if ((flags & ItemFlags.Socketed) != 0)
			{
				int bits = Constants.item_stat_costs["item_numsockets"].save_bits ?? 0;
				writer.WriteByte(total_nr_of_sockets, bits);
				Log.Verbose($"Write total_nr_of_sockets. {bits} bits. Position: {writer.Position}");
			}

			if (quality == ItemQuality.Set)
			{
				writer.WriteByte(set_item_mask, 5);
				Log.Verbose($"Write set_item_mask. 5 bits. Position: {writer.Position}");
				set_bonuses |= set_item_mask;
			}

			WriteItemModifiers(writer, modifiers[0]);

			int idx = 1;
			while (set_bonuses > 0)
			{
				if ((set_bonuses & 0x1) != 0)
				{
					WriteItemModifiers(writer, modifiers[idx++]);
				}
				set_bonuses >>= 1;
			}

			int old_pos = writer.Position;
			writer.Align();
			Log.Verbose($"Align writer from {old_pos} to {writer.Position}");

			Log.Verbose("Read socketed_items");
			foreach (Item socketed_item in socketed_items)
			{
				socketed_item.Write(writer, version);
			}
		}
		//not sure wth this is supposed to present
		private void WriteExtraData(IBitWriter writer, int version)
		{
			if (version <= 99)
				return;

			if (version <= 101)
			{
				// Check hardcoded item code list, conditionally read 8 bits
				if (extra_data_itemcodes.Contains(code!))
				{
					writer.WriteBits(0, 8);
					Log.Verbose($"Write extra_data_itemcode. 8 bits. Position: {writer.Position}");
				}

				return;
			}

			if (version >= 104)
			{
				writer.WriteBit(yes_bit);
				Log.Verbose($"Write yes_bit. 1 bit. Position: {writer.Position} bits.. Bit..");
				if (yes_bit)
				{
					writer.WriteByte(yes_bit_bits, 8);
					Log.Verbose($"Write yes_bit_bits. 8 bits. Position: {writer.Position} bits.. Bit bit bit bit bit bit bit bit...");
				}

				return;
			}

			// version 102–103
			writer.WriteBit(yes_102_103);
			Log.Verbose($"Write yes_102_103. 1 bit. Position: {writer.Position}");
			if (yes_102_103)
			{
				writer.WriteByte(yes_102_103_data);
				Log.Verbose($"Write yes_102_103_data. 8 bit. Position: {writer.Position}");
			}
		}

		public void WriteItemModifiers(IBitWriter writer, List<ItemModifier> modifiers)
		{
			for (int i = 0; i < modifiers.Count; i++)
			{
				var mod = modifiers[i];
				writer.WriteUInt16(mod.id!.Value, 9);
				Log.Verbose($"Write mod.id. 9 bits. Position: {writer.Position}");
				modifiers[i].Write(writer);
				if (mod.id is magicmindam or item_maxdamage_percent or firemindam or lightmindam)
				{
					modifiers[++i].Write(writer);
				}
				else if (mod.id is coldmindam or poisonmindam)
				{
					modifiers[++i].Write(writer);
					modifiers[++i].Write(writer);
				}
			}

			writer.WriteUInt16(0x1ff, 9);
			Log.Verbose($"Write mod.ender (511). 9 bits. Position: {writer.Position}");
		}

		private static void WritePlayerName(IBitWriter writer, string name, uint version)
		{
			var nameChars = name.AsSpan().TrimEnd('\0');
			Span<byte> bytes = stackalloc byte[30];

			// Find max characters that fit
			int charCount = nameChars.Length;
			while (charCount > 0 && Encoding.UTF8.GetByteCount(nameChars[..charCount]) > bytes.Length)
			{
				charCount--;
			}

			var trimmedChars = nameChars[..charCount];

			int byteCount = Encoding.UTF8.GetBytes(trimmedChars, bytes);
			bytes = bytes[..byteCount];
			int numBits = version > 97 ? 8 : 7;
			for (int i = 0; i < bytes.Length; i++)
			{
				writer.WriteByte(bytes[i], numBits);
			}
			writer.WriteByte(0, numBits);
		}

		private static Item ReadCompact(IBitReader reader, int version)
		{
			Item item = new();
			item.flags = (ItemFlags)reader.ReadUInt32();
			Log.Verbose($"Read flags. 32 bits. Position: {reader.Position}. {item.flags}");

			if (version > 96)
			{
				item.version = (ushort)(reader.ReadUInt16(3) + 99);
				Log.Verbose($"Read version ({item.version}). 3 bits. Position: {reader.Position}");
			}

			item.location_id = reader.ReadByte(3);
			Log.Verbose($"Read location_id. 3 bits. Position: {reader.Position}");
			item.equipped_id = reader.ReadByte(4);
			Log.Verbose($"Read equipped_id. 4 bits. Position: {reader.Position}");
			item.position_x = reader.ReadByte(4);
			Log.Verbose($"Read position_x. 4 bits. Position: {reader.Position}");
			item.position_y = reader.ReadByte(4);
			Log.Verbose($"Read position_y. 4 bits. Position: {reader.Position}");
			item.alt_position_id = reader.ReadByte(3);
			Log.Verbose($"Read alt_position_id. 3 bits. Position: {reader.Position}");

			if ((item.flags & ItemFlags.IsEar) != 0)
			{
				Ear ear = (Ear)item;
				ear.class_of_ear = reader.ReadByte(3);
				Log.Verbose($"Read ear class. 3 bits. Position: {reader.Position}");
				ear.level_of_ear = reader.ReadByte(7);
				Log.Verbose($"Read ear level. 7 bits. Position: {reader.Position}");
				int old_pos = reader.Position;
				ear.name_of_ear = reader.ReadString(15);
				Log.Verbose($"Read ear name. {reader.Position - old_pos} bits. Position: {reader.Position}");

				return ear;
			}
			else
			{
				int old_pos = reader.Position;
				item.code = GetItemCode(reader);
				Log.Verbose($"Read code. {reader.Position - old_pos} bits. Position: {reader.Position}");
				item.category = GetCategory(item.code);
				item.compact_item = (item.flags & ItemFlags.CompactSave) != 0;
				int num_socket_bits = item.compact_item ? 1 : 3;

				if (item.code == "ques")
				{
					var a = Constants.item_stat_costs["questitemdifficulty"];
					item.quest_difficulty = (byte)((reader.ReadInt32(a.save_bits ?? 0) - a.save_add ?? 0) << a.val_shift ?? 0);
					Log.Verbose($"Read quest_difficulty. {a.save_bits ?? 0} bits. Position: {reader.Position}");

					if (item.quest_difficulty > 2)
						throw new Exception("item.QuestDifficulty > 2");

					num_socket_bits = 1;
				}

				item.used_sockets = reader.ReadByte(num_socket_bits);
				Log.Verbose($"Read used_sockets. {num_socket_bits} bits. Position: {reader.Position}");
			}

			return item;
		}

		private static Item ReadComplete(IBitReader reader, Item item)
		{
			item.id = reader.ReadUInt32(32);
			Log.Verbose($"Read id. 32 bits. Position: {reader.Position}");
			string fingerprint = "0x"+item.id.ToString("X");
			item.level = reader.ReadByte(7);
			Log.Verbose($"Read level. 7 bits. Position: {reader.Position}");
			item.quality = (ItemQuality)reader.ReadByte(4);
			Log.Verbose($"Read quality. 4 bits. Position: {reader.Position}");

			item.multiple_pictures = reader.ReadBit();
			Log.Verbose($"Read multiple_pictures. 1 bit. Position: {reader.Position}");
			if (item.multiple_pictures)
			{
				item.picture_id = reader.ReadByte(3);
				Log.Verbose($"Read picture_id. 3 bits. Position: {reader.Position}");
			}
			item.class_specific = reader.ReadBit();
			Log.Verbose($"Read class_specific. 1 bit. Position: {reader.Position}");
			if (item.class_specific)
			{
				item.auto_affix_id = reader.ReadUInt16(11);
				Log.Verbose($"Read auto_affix_id. 11 bits. Position: {reader.Position}");
			}
			switch (item.quality)
			{
				case ItemQuality.Low:
				case ItemQuality.Superior:
				{
					item.file_index = reader.ReadByte(3);
					Log.Verbose($"Read file_index. 3 bits. Position: {reader.Position}");
					break;
				}
				case ItemQuality.Normal:
				{
					if (item.code == "scro" || item.code == "book") // item type 22 or 18
					{
						item.magic_suffix_ids.Add(reader.ReadUInt16(5));
						Log.Verbose($"Read scroll/book data. 5 bits. Position: {reader.Position}");
					}

					break;
				}
				case ItemQuality.Magic:
				{
					item.magic_prefix_ids.Add(reader.ReadUInt16(11));
					Log.Verbose($"Read magic_prefix_id. 11 bits. Position: {reader.Position}");
					item.magic_suffix_ids.Add(reader.ReadUInt16(11));
					Log.Verbose($"Read magic_suffix_id. 11 bits. Position: {reader.Position}");
					break;
				}
				case ItemQuality.Set:
				case ItemQuality.Unique:
				{
					item.file_index= reader.ReadUInt16(12);
					Log.Verbose($"Read file_index. 12 bits. Position: {reader.Position}");
					break;
				}
				case ItemQuality.Rare:
				case ItemQuality.Crafted:
				{
					item.rare_prefix_id = reader.ReadByte(8);
					Log.Verbose($"Read rare_prefix_id. 8 bits. Position: {reader.Position}");
					item.rare_suffix_id = reader.ReadByte(8);
					Log.Verbose($"Read rare_suffix_id. 8 bits. Position: {reader.Position}");

					for (int i = 0; i < 3; i++)
					{
						bool has_prefix = reader.ReadBit();
						Log.Verbose($"Read has_prefix. 1 bit. Position: {reader.Position}");
						if (has_prefix)
						{
							item.magic_prefix_ids.Add(reader.ReadUInt16(11));
							Log.Verbose($"Read magic_prefix_id[{i}]. 11 bits. Position: {reader.Position}");
						}

						bool has_suffix = reader.ReadBit();
						Log.Verbose($"Read has_suffix. 1 bit. Position: {reader.Position}");
						if (has_suffix)
						{
							item.magic_suffix_ids.Add(reader.ReadUInt16(11));
							Log.Verbose($"Read magic_suffix_id[{i}]. 11 bits. Position: {reader.Position}");
						}
					}
					break;
				}
			}

			ushort set_bonuses = 0;

			if ((item.flags & ItemFlags.Runeword) != 0)
			{
				item.runeword_id = reader.ReadUInt16(12);
				Log.Verbose($"Read runeword_id. 12 bits. Position: {reader.Position}");
				item.runeword_statlist_index = reader.ReadByte(4);
				Log.Verbose($"Read runeword_statlist_index. 4 bits. Position: {reader.Position}");
				//learn wtf this bithack magic does
				set_bonuses |= (ushort)(1 << (item.runeword_statlist_index + 1));
			}

			if ((item.flags & ItemFlags.Personalized) != 0)
			{
				int old_pos = reader.Position;
				item.personalized_name = GetPersonalizedName(reader);
				Log.Verbose($"Read personalized_name. {reader.Position - old_pos} bits. Position: {reader.Position}");
			}

			bool has_realm_data = reader.ReadBit();
			Log.Verbose($"Read has_realm_data. 1 bit. Position: {reader.Position}");
			if (has_realm_data)
			{
				item.realm_data = reader.ReadBytes(16);
				Log.Verbose($"Read realm_data. 16 bits. Position: {reader.Position}");
			}

			//if (code == "tbk" || code == "ibk")
			//{
			//	byte tome_unknown_data = reader.ReadByte(5);
			//}
			string code = item.code!;

			if (Constants.armors.ContainsKey(code))
			{
				//is amror
				item.type = EItemType.Armor;
			}
			else if (Constants.weapons.ContainsKey(code))
			{
				//is wepun
				item.type = EItemType.Weapon;
			}
			else if (Constants.miscs.ContainsKey(code))
			{
				//is else
				item.type = EItemType.Other;
			}

			item.stackable = Constants.IsStackable(item.code!);

			if (item.type == EItemType.Armor)
			{
				int bits_to_read = Constants.item_stat_costs["armorclass"].save_bits!.Value;
				item.defense_rating = reader.ReadUInt16(bits_to_read)
				- Constants.item_stat_costs["armorclass"].save_add!.Value;
				Log.Verbose($"Read defense_rating. {bits_to_read} bits. Position: {reader.Position}");
			}

			if (item.type == EItemType.Armor || item.type == EItemType.Weapon)
			{
				int bits_to_read = Constants.item_stat_costs["maxdurability"].save_bits!.Value;
				item.max_durability = reader.ReadUInt16(bits_to_read)
				- Constants.item_stat_costs["maxdurability"].save_add!.Value;
				Log.Verbose($"Read max_durability. {bits_to_read} bits. Position: {reader.Position}");

				if (item.max_durability > 0)
				{
					bits_to_read = Constants.item_stat_costs["durability"].save_bits!.Value;
					item.current_durability = reader.ReadUInt16(bits_to_read)
					- Constants.item_stat_costs["durability"].save_add!.Value;
					Log.Verbose($"Read durability. {bits_to_read} bits. Position: {reader.Position}");
				}
			}

			if (item.stackable)
			{
				item.quantity = reader.ReadUInt16(9);
				Log.Verbose($"Read quantity. 9 bits. Position: {reader.Position}");
			}

			if ((item.flags & ItemFlags.Socketed) != 0)
			{
				int bits_to_read = Constants.item_stat_costs["item_numsockets"].save_bits ?? 0;
				item.total_nr_of_sockets = reader.ReadByte(bits_to_read);
				Log.Verbose($"Read total_nr_of_sockets. {bits_to_read} bits. Position: {reader.Position}");
			}

			if (item.quality == ItemQuality.Set)
			{
				item.set_item_mask = reader.ReadByte(5);
				Log.Verbose($"Read set_item_mask. 5 bits. Position: {reader.Position}");
				set_bonuses |= item.set_item_mask;
			}

			item.modifiers.Add(ReadItemStats(reader));

			while (set_bonuses > 0)
			{
				if ((set_bonuses & 0x1) != 0)
				{
					item.modifiers.Add(ReadItemStats(reader));
				}
				set_bonuses >>= 1;
			}

			if (item.version > 99)
			{
				if ((item.flags & ItemFlags.Unk4) != 0)
				{
					reader.ReadBits(16);
					Log.Verbose($"Read ItemFlags.Unk4. 16 bits. Position: {reader.Position}");

					int count;

					if ((item.flags & ItemFlags.Unk5) == 0)
					{
						reader.ReadBits(32);
						Log.Verbose($"Read ItemFlags.Unk5. 32 bits. Position: {reader.Position}");
						count = reader.ReadInt32(4);
						Log.Verbose($"Read ItemFlags.Unk5 count. 4 bits. Position: {reader.Position}");
						if (count < 0) count = 0;
						if (count > 8) count = 8;
					}
					else
					{
						count = 1;
					}

					for (int i = 0; i < count; i++)
					{
						uint lo = reader.ReadUInt32();
						Log.Verbose($"Read lo. 32 bits. Position: {reader.Position}");
						uint hi = reader.ReadUInt32();
						Log.Verbose($"Read hi. 32 bits. Position: {reader.Position}");
					}
				}
			}

			return item;
		}

		private static readonly HashSet<string> extra_data_itemcodes = new HashSet<string>
	{
		"rvs ", "rvl ",
        // Amethyst
        "gcv ", "gfv ", "gsv ", "gzv ", "gpv ",
        // Topaz
        "gcy ", "gfy ", "gsy ", "gly ", "gpy ",
        // Sapphire
        "gcb ", "gfb ", "gsb ", "glb ", "gpb ",
        // Emerald
        "gcg ", "gfg ", "gsg ", "glg ", "gpg ",
        // Ruby
        "gcr ", "gfr ", "gsr ", "glr ", "gpr ",
        // Diamond
        "gcw ", "gfw ", "gsw ", "glw ", "gpw ",
        // Skulls
        "skc ", "skf ", "sku ", "skl ", "skz ",
        // Runes
        "r01 ", "r02 ", "r03 ", "r04 ", "r05 ",
		"r06 ", "r07 ", "r08 ", "r09 ", "r10 ",
		"r11 ", "r12 ", "r13 ", "r14 ", "r15 ",
		"r16 ", "r17 ", "r18 ", "r19 ", "r20 ",
		"r21 ", "r22 ", "r23 ", "r24 ", "r25 ",
		"r26 ", "r27 ", "r28 ", "r29 ", "r30 ",
		"r31 ", "r32 ", "r33 ",
        // Pandemonium Keys
        "pk1 ", "pk2 ", "pk3 ",
        // Uber organs / special items
        "dhn ", "bey ", "mbr ", "toa ", "tes ",
		"ceh ", "bet ", "fed ",
	};

		//not sure wth this is supposed to present
		private static void ReadExtraData(IBitReader reader, Item item, int version)
		{
			if (version <= 99)
				return;

			if (version <= 101)
			{
				// Check hardcoded item code list, conditionally read 8 bits
				if (extra_data_itemcodes.Contains(item.code!))
				{
					reader.ReadBits(8);
					Log.Verbose($"Read extra_data_itemcode. 8 bits. Position: {reader.Position}");
				}

				return;
			}

			if (version >= 104)
			{
				bool yes = reader.ReadBit();
				item.yes_bit = yes;

				Log.Verbose($"Read yes_bit. 1 bit. Position: {reader.Position} bits.. Bit..");
				if (yes)
				{
					item.yes_bit_bits = reader.ReadByte();
					Log.Verbose($"Read yes_bit_bits. 8 bits. Position: {reader.Position} bits.. Bit bit bit bit bit bit bit bit...");
				}

				return;
			}

			// version 102–103
			item.yes_102_103 = reader.ReadBit();
			Log.Verbose($"Read yes_102_103. 1 bit. Position: {reader.Position}");
			if (item.yes_102_103)
			{
				item.yes_102_103_data = reader.ReadByte();
				Log.Verbose($"Read yes_102_103_bits. 8 bit. Position: {reader.Position}");
			}
		}

		private static string GetPersonalizedName(IBitReader reader)
		{
			StringBuilder sb = new();
			for (int i = 0; i < 16; i++)
			{
				char c = (char)reader.ReadByte(8);
				//for versions <= 0x60 read 7 bytes
				sb.Append((char)reader.ReadByte(8));
			}
			return sb.ToString();
		}

		private static string GetItemCode(IBitReader reader)
		{
			HuffmanTree tree = new HuffmanTree();
			tree.Build();

			Span<byte> codeBuffer = stackalloc byte[4];
			for (int i = 0; i <= 3; i++)
			{
				codeBuffer[i] = tree.DecodeChar(reader);
			}
			return Encoding.ASCII.GetString(codeBuffer).Replace(" ", "");
		}

		public static Item Read(IBitReader reader, int version)
		{
			Item? item = null;

			try
			{
				int pos = reader.Position;
				item = ReadCompact(reader, version);

				if (!item.compact_item)
				{
					item = ReadComplete(reader, item);
				}

				ReadExtraData(reader, item, version);
				int old_pos = reader.Position;
				reader.Align();
				Log.Verbose($"Align reader from {old_pos} to {reader.Position}");

				Log.Verbose("Read socketed_items");
				for (int i = 0; i < item.used_sockets; i++)
				{
					item.socketed_items.Add(Read(reader, version));
				}			
			}
			catch (Exception ex)
			{
				Log.Error(ex, $"Error reading item. {item!.id}");
			}

			return item;
		}

		public class ItemModifier
		{
			public ushort? id { get; set; }
			public string stat { get; set; } = string.Empty;
			public int? skill_tab { get; set; }
			public int? skill_id { get; set; }
			public int? skill_level { get; set; }
			public int? max_charges { get; set; }
			public int? param { get; set; }
			public int value { get; set; }

			public static ItemModifier Read(IBitReader reader, ushort id)
			{
				var itemStat = new ItemModifier();
				var property = Constants.item_stat_costs.Values.FirstOrDefault(x => (ushort)x.id! == id);
				if (property == null)
				{
					throw new Exception($"No ItemStatCost record found for id: {id} at bit {reader.Position - 9}");
				}
				itemStat.id = id;
				itemStat.stat = property.stat!;
				int? saveParamBitCount = property.save_param_bits;
				int? encode = property.encode;
				if (saveParamBitCount != null && saveParamBitCount != 0)
				{
					int saveParam = reader.ReadInt32(saveParamBitCount.Value);
					Log.Verbose($"  Read mod.save_param. {saveParamBitCount} bits. Position: {reader.Position}");
					//todo is there a better way to identify skill tab stats.
					switch (property.desc_func)
					{
						case 14: //+[value] to [skilltab] Skill Levels ([class] Only) : stat id 188
							itemStat.skill_tab = saveParam & 0x7;
							itemStat.skill_level = (saveParam >> 3) & 0x1fff;
							break;
						default:
							break;
					}
					switch (encode)
					{
						case 2: //chance to cast skill
						case 3: //skill charges
							itemStat.skill_level = saveParam & 0x3f;
							itemStat.skill_id = (saveParam >> 6) & 0x3ff;
							break;
						case 1:
						case 4: //by times
						default:
							itemStat.param = saveParam;
							break;
					}
				}

				int saveBits = reader.ReadInt32(property.save_bits ?? 0);
				Log.Verbose($"  Read mod.save_bits. {property.save_bits ?? 0} bits. Position: {reader.Position}");
				int? save_add = property.save_add;
				saveBits -= save_add ?? 0;
				switch (encode)
				{
					case 3: //skill charges
						itemStat.max_charges = (saveBits >> 8) & 0xff;
						itemStat.value = saveBits & 0xff;
						break;
					default:
						itemStat.value = saveBits;
						break;
				}
				return itemStat;
			}

			public void Write(IBitWriter writer)
			{
				var property = Constants.item_stat_costs.Values.FirstOrDefault(x => (ushort)x.id! == id);

				if (property == null)
				{
					string message = $"No ItemStatCost record found for id: {id}";
					Log.Error(message);
					throw new InvalidOperationException(message);
				}

				int save_param_bit_count = property.save_param_bits ?? 0;
				int? encode = property.encode;

				if (save_param_bit_count != 0)
				{
					if (param.HasValue)
					{
						writer.WriteInt32(param.Value, save_param_bit_count);
						Log.Verbose($"  Write mod.param. 0 bits. Position: {writer.Position}");
					}
					else
					{
						int save_param_bits = 0;

						switch (property.desc_func)
						{
							case 14:
								save_param_bits |= (skill_tab ?? 0 & 0x7);
								save_param_bits |= ((skill_level ?? 0 & 0x1fff) << 3);
								break;
							default:
								break;
						}
						switch (encode)
						{
							case 2: //chance to cast skill
							case 3: //skill charges
								save_param_bits |= (skill_level ?? 0 & 0x3f);
								save_param_bits |= ((skill_id ?? 0 & 0x3ff) << 6);
								break;
							case 4: //by times
							case 1:
							default:
								break;
						}
						if (param.HasValue)
						{
							save_param_bits = param.Value;
						}
						writer.WriteInt32(save_param_bits, save_param_bit_count);
						Log.Verbose($"  Write mod.save_param_bits. {save_param_bit_count} bits. Position: {writer.Position}");
					}
				}
				int save_bits = value;
				save_bits += property.save_add ?? 0;
				switch (encode)
				{
					case 3: //skill charges
						save_bits &= 0xff;
						save_bits |= ((max_charges ?? 0 & 0xff) << 8);
						break;
					default:
						break;
				}
				writer.WriteInt32(save_bits, property.save_bits ?? 0);
				Log.Verbose($"  Write mod.save_bits. {property.save_bits ?? 0} bits. Position: {writer.Position}");
			}
		}

		private const ushort magicmindam = 52;
		private const ushort item_maxdamage_percent = 17;
		private const ushort firemindam = 48;
		private const ushort lightmindam = 50;
		private const ushort coldmindam = 54;
		private const ushort poisonmindam = 57;

		public static List<ItemModifier> ReadItemStats(IBitReader reader)
		{
			ushort id = reader.ReadUInt16(9);
			Log.Verbose($"Read mod.id. 9 bits. Position: {reader.Position}");
			List<ItemModifier> item_stats = new();

			while (id != 0x1ff)
			{
				item_stats.Add(ItemModifier.Read(reader, id));

				if (id is magicmindam or item_maxdamage_percent or firemindam or lightmindam)
				{
					item_stats.Add(ItemModifier.Read(reader, (ushort)(id + 1)));
				}
				else if (id is coldmindam or poisonmindam)
				{
					item_stats.Add(ItemModifier.Read(reader, (ushort)(id + 1)));
					item_stats.Add(ItemModifier.Read(reader, (ushort)(id + 2)));
				}

				id = reader.ReadUInt16(9);
				Log.Verbose($"Read id. 9 bits. Position: {reader.Position}");
			}

			return item_stats;
		}
	}

	public enum EItemType
	{
		Undefined = 0x00,
		Armor = 0x01,
		Shield = 0x02, //treated the same as armor... only here to be able to parse nokkas jsons
		Weapon = 0x03,
		Other = 0x04
	}

	public sealed class ItemList
	{
		private ItemList(ushort header, ushort count)
		{
			this.header = header;
			items = new List<Item>(count);
		}

		public ushort? header { get; set; }
		public List<Item> items { get; }

		public void Write(IBitWriter writer, int version, bool write_item_list_header = true)
		{
			ushort itemcount = (ushort)items.Count;
			if (write_item_list_header)
			{
				writer.WriteUInt16(header ?? 99);
				Log.Verbose($"Write header. 16 bits. Position: {writer.Position}");
				writer.WriteUInt16(itemcount);
				Log.Verbose($"Write item_count. 16 bits. Position: {writer.Position}");
			}
			for (int i = 0; i < itemcount; i++)
			{
				int pos = writer.Position;
				Log.Verbose($"Write item number {i}");
				items[i].Write(writer, version);
			}
		}

		public static ItemList Read(IBitReader reader, int version)
		{
			ushort header = reader.ReadUInt16();
			Log.Verbose($"Read header. 16 bits. Position: {reader.Position}");
			ushort items = reader.ReadUInt16();
			Log.Verbose($"Read item_count. 16 bits. Position: {reader.Position}");
			var itemList = new ItemList(
				header: header,
				count: items
			);

			for (int i = 0; i < items; i++)
			{
				itemList.items.Add(Item.Read(reader, version));
			}

			return itemList;
		}

		public static ItemList Read(IBitReader reader, int version, ushort items)
		{
			var itemList = new ItemList(
				header: 99,
				count: items
			);

			for (int i = 0; i < items; i++)
			{
				int pos = reader.Position;
				Log.Verbose($"Read item number {i}");
				itemList.items.Add(Item.Read(reader, version));
			}

			return itemList;
		}
	}
}
