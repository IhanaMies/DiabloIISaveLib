using CommunityToolkit.HighPerformance.Buffers;
using DiabloIISaveLib.IO;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace DiabloIISaveLib.Types
{

	[Flags]
	public enum CharacterFlags : byte
	{
		None = 0x0,
		Newbie = 0x1,
		Error = 0x2,
		Hardcore = 0x4,
		Dead = 0x8,
		SaveProcess = 0x10,
		Expansion = 0x20,
		Ladder = 0x40,
		NeedsRenaming = 0x80
	}

	public class Character
	{
		//0x0000
		public Header header { get; set; }
		//0x0010
		public uint active_weapon { get; set; }
		//0x0014 sizeof(16)
		public string? name { get; set; }
		//0x0024
		public CharacterFlags status { get; set; }
		//0x0025
		public byte progression { get; set; }
		//0x0026 [unk = 0x0, 0x0]
		[JsonIgnore]
		public byte[]? active_weapon_set { get; set; }
		//0x0028
		public byte class_id { get; set; }
		//0x0029 [unk = 0x10, 0x1E]
		[JsonIgnore]
		public byte[]? unk0x0029 { get; set; }
		//0x002b
		public byte level { get; set; }
		//0x002c
		public uint created { get; set; }
		//0x0030
		public uint last_played { get; set; }
		//0x0034 [unk = 0xff, 0xff, 0xff, 0xff]
		public uint play_time { get; set; }
		//0x0038
		public Skill[] AssignedSkills { get; set; }
		//0x0078
		public Skill left_skill { get; set; }
		//0x007c
		public Skill right_skill { get; set; }
		//0x0080
		public Skill left_swap_skill { get; set; }
		//0x0084
		public Skill right_swap_skill { get; set; }
		//0x0088 [char menu appearance]
		public Appearances appearances { get; set; }
		//0x00a8
		public Locations location { get; set; }
		//0x00ab
		public uint map_id { get; set; }
		//0x00b1
		public Mercenary mercenary { get; set; }
		public byte[]? unk_v105_1 { get; set; }
		//0x00bf
		public PreviewData preview_data { get; set; }
		public byte[]? unk_v105_2 { get; set; }
		//0x014f
		public QuestsSection quests { get; set; }
		//0x0279
		public WaypointsSection waypoints { get; set; }
		//0x02c9
		public NPCDialogSection npc_dialog { get; set; }
		//0x2fc
		public Attributes attributes { get; set; }
		public ClassSkills class_skills { get; set; }
		public ItemList item_list { get; set; }
		public CorpseList player_corpses { get; set; }
		public MercenaryItemList? mercenary_items { get; set; }
		public Golem? golem { get; set; }
		//public Character()
		//{

		//}
		public Character(string path)
		{
			if (!File.Exists(path))
			{
				throw new InvalidOperationException($"Character save does not exist. {path}");
			}

			using BitReader reader = new(File.ReadAllBytes(path));
			header = Header.Read(reader);
			string headerstr = reader.ReadString(2);
			Log.Verbose($"Read headerstr ({headerstr}). 16 bits. Position: {reader.Position}");

			active_weapon = reader.ReadUInt32();
			Log.Verbose($"Read active_weapon ({active_weapon}). 32 bits. Position: {reader.Position}");
			if (header.version >= 0x69)
			{

			}
			else if (header.version >= 0x62)
			{
				reader.AdvanceBits(0x10);
				Log.Verbose($"Skip 10 bits. Position: {reader.Position}");
			}
			else
			{
				name = reader.ReadString(16);
				Log.Verbose($"Read name ({name}). {16*8} bits. Position: {reader.Position}");
			}
			status = (CharacterFlags)reader.ReadByte();
			Log.Verbose($"Read status ({status}). 8 bits. Position: {reader.Position}");
			progression = reader.ReadByte();
			Log.Verbose($"Read progression ({progression}). 8 bits. Position: {reader.Position}");
			active_weapon_set = reader.ReadBytes(2); // active_arms
			Log.Verbose($"Read active_weapon_set ({active_weapon_set}). 16 bits. Position: {reader.Position}");
			class_id = reader.ReadByte();
			Log.Verbose($"Read class_id ({class_id}). 8 bits. Position: {reader.Position}");
			unk0x0029 = reader.ReadBytes(2); // Stats, Skills
			Log.Verbose($"Read unk0x0029 ({unk0x0029}). 16 bits. Position: {reader.Position}");
			level = reader.ReadByte();
			Log.Verbose($"Read level ({level}). 8 bits. Position: {reader.Position}");
			created = reader.ReadUInt32();
			Log.Verbose($"Read created ({created}). 32 bits. Position: {reader.Position}");
			last_played = reader.ReadUInt32();
			Log.Verbose($"Read last_played ({last_played}). 32 bits. Position: {reader.Position}");
			play_time = reader.ReadUInt32();
			Log.Verbose($"Read play_time ({play_time}). 32 bits. Position: {reader.Position}");
			AssignedSkills = new Skill[16];
			for (int i = 0; i < AssignedSkills.Length; i++) AssignedSkills[i] = Skill.Read(reader);
			left_skill = Skill.Read(reader);
			right_skill = Skill.Read(reader);
			left_swap_skill = Skill.Read(reader);
			right_swap_skill = Skill.Read(reader);
			appearances = Appearances.Read(reader);
			location = Locations.Read(reader);
			map_id = reader.ReadUInt32();
			mercenary = Mercenary.Read(reader);

			if (header.version > 100)
			{
				unk_v105_1 = reader.ReadBytes(45);
				Log.Verbose($"Read unk_v105_1 ({unk_v105_1}). {45*8} bits. Position: {reader.Position}");
			}

			preview_data = PreviewData.Read(reader, header.version);

			if (header.version > 100)
			{
				unk_v105_2 = reader.ReadBytes(36);
				Log.Verbose($"Read unk_v105_2 ({unk_v105_2}). {36*8} bits. Position: {reader.Position}");
			}

			quests = QuestsSection.Read(reader);
			waypoints = WaypointsSection.Read(reader);
			npc_dialog = NPCDialogSection.Read(reader);
			attributes = Attributes.Read(reader);

			class_skills = ClassSkills.Read(reader, class_id);
			item_list = ItemList.Read(reader, header.version);
			player_corpses = CorpseList.Read(reader, header.version);

			// this expansion check is likely correct, but not for my use case
			//if (Status.IsExpansion)
			//{
			if (reader.Position != reader.Length)
			{
				mercenary_items = MercenaryItemList.Read(reader, mercenary, header.version);
			}

			if (reader.Position != reader.Length)
			{
				golem = Golem.Read(reader, header.version);
			}
			//}
		}

		public void Write(IBitWriter writer)
		{
			header.Write(writer);
			Log.Verbose($"Write header ({header}). 128 bits. Position: {writer.Position}");
			writer.WriteUInt32(active_weapon);
			Log.Verbose($"Write active_weapon ({active_weapon}). 32 bits. Position: {writer.Position}");
			int pos = writer.Position;
			writer.WriteString(name, 16);
			Log.Verbose($"Write name ({name}). {writer.Position - pos} bits. Position: {writer.Position}");
			writer.WriteByte((byte)status);
			Log.Verbose($"Write status ({status}). 8 bits. Position: {writer.Position}");
			writer.WriteByte(progression);
			Log.Verbose($"Write progression ({progression}). 8 bits. Position: {writer.Position}");
			//Unk0x0026
			writer.WriteBytes(active_weapon_set ?? new byte[2]);
			Log.Verbose($"Write active_weapon_set ({active_weapon_set}). 16 bits. Position: {writer.Position}");
			writer.WriteByte(class_id);
			Log.Verbose($"Write class_id ({class_id}). 8 bits. Position: {writer.Position}");
			//Unk0x0029
			pos = writer.Position;
			writer.WriteBytes(unk0x0029 ?? stackalloc byte[] { 0x10, 0x1e });
			Log.Verbose($"Write unk0x0029 ({unk0x0029}). {writer.Position - pos} bits. Position: {writer.Position}");
			writer.WriteByte(level);
			Log.Verbose($"Write level ({level}). 8 bits. Position: {writer.Position}");
			writer.WriteUInt32(created);
			Log.Verbose($"Write created ({created}). 32 bits. Position: {writer.Position}");
			writer.WriteUInt32(last_played);
			Log.Verbose($"Write last_played ({last_played}). 32 bits. Position: {writer.Position}");
			writer.WriteUInt32(play_time);
			Log.Verbose($"Write play_time ({play_time}). 32 bits. Position: {writer.Position}");
			for (int i = 0; i < 16; i++)
			{
				AssignedSkills[i].Write(writer);
			}
			left_skill.Write(writer);
			right_skill.Write(writer);
			left_swap_skill.Write(writer);
			right_swap_skill.Write(writer);
			appearances.Write(writer);
			location.Write(writer);
			writer.WriteUInt32(map_id);
			mercenary.Write(writer);
			preview_data.Write(writer, header.version);
			quests.Write(writer);
			waypoints.Write(writer);
			npc_dialog.Write(writer);
			attributes.Write(writer);
			class_skills.Write(writer);
			item_list.Write(writer, header.version);
			player_corpses.Write(writer, header.version);
			// this expansion check is likely correct, but not for my use case
			//if (Status.IsExpansion)
			//{
			mercenary_items?.Write(writer, mercenary, header.version);
			golem?.Write(writer, header.version);
			//}
		}

		public static MemoryOwner<byte> WritePooled(Character character)
		{
			using var writer = new BitWriter();
			character.Write(writer);
			var bytes = writer.ToPooledArray();
			Header.Fix(bytes.Span);
			return bytes;
		}

		public static byte[] Write(Character character)
		{
			using var writer = new BitWriter();
			character.Write(writer);
			byte[] bytes = writer.ToArray();
			Header.Fix(bytes);
			return bytes;
		}

		public void Dispose()
		{
			quests.Dispose();
			player_corpses.Dispose();
			mercenary_items?.Dispose();
		}
	}
}
