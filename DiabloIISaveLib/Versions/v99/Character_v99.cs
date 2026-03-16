using CommunityToolkit.HighPerformance.Buffers;
using DiabloIISaveLib.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace DiabloIISaveLib.Versions.v99
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

	public class Character_v99
	{
		//0x0000
		public Header_v99 header { get; set; }
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
		public Skill_v99[] AssignedSkills { get; set; }
		//0x0078
		public Skill_v99 left_skill { get; set; }
		//0x007c
		public Skill_v99 right_skill { get; set; }
		//0x0080
		public Skill_v99 left_swap_skill { get; set; }
		//0x0084
		public Skill_v99 right_swap_skill { get; set; }
		//0x0088 [char menu appearance]
		public Appearances_v99 appearances { get; set; }
		//0x00a8
		public Locations_v99 location { get; set; }
		//0x00ab
		public uint map_id { get; set; }
		//0x00b1
		public Mercenary_v99 mercenary { get; set; }
		public byte[]? unk_v105_1 { get; set; }
		//0x00bf
		public PreviewData_v99 preview_data { get; set; }
		public byte[]? unk_v105_2 { get; set; }
		//0x014f
		public QuestsSection_v99 quests { get; set; }
		//0x0279
		public WaypointsSection_v99 waypoints { get; set; }
		//0x02c9
		public NPCDialogSection_v99 npc_dialog { get; set; }
		//0x2fc
		public Attributes_v99 attributes { get; set; }
		public ClassSkills_v99 class_skills { get; set; }
		public ItemList_v99 item_list { get; set; }
		public CorpseList_v99 player_corpses { get; set; }
		public MercenaryItemList? mercenary_items { get; set; }
		public Golem_v99? golem { get; set; }
		private Character_v99(IBitReader reader)
		{
			header = Header_v99.Read(reader);

			string headerstr = reader.ReadString(2);

			active_weapon = reader.ReadUInt32();
			if (header.version >= 0x69)
			{

			}
			else if (header.version >= 0x62)
			{
				reader.AdvanceBits(0x10);
			}
			else
			{
				name = reader.ReadString(16);
			}
			status = (CharacterFlags)reader.ReadByte();
			progression = reader.ReadByte();
			active_weapon_set = reader.ReadBytes(2); // active_arms
			class_id = reader.ReadByte();
			unk0x0029 = reader.ReadBytes(2); // Stats, Skills
			level = reader.ReadByte();
			created = reader.ReadUInt32();
			last_played = reader.ReadUInt32();
			play_time = reader.ReadUInt32();
			AssignedSkills = new Skill_v99[16];
			for (int i = 0; i < AssignedSkills.Length; i++) AssignedSkills[i] = Skill_v99.Read(reader);
			left_skill = Skill_v99.Read(reader);
			right_skill = Skill_v99.Read(reader);
			left_swap_skill = Skill_v99.Read(reader);
			right_swap_skill = Skill_v99.Read(reader);
			appearances = Appearances_v99.Read(reader);
			location = Locations_v99.Read(reader);
			map_id = reader.ReadUInt32();
			mercenary = Mercenary_v99.Read(reader);

			if (header.version > 100)
				unk_v105_1 = reader.ReadBytes(45);

			preview_data = PreviewData_v99.Read(reader, header.version);

			if (header.version > 100)
				unk_v105_2 = reader.ReadBytes(36);

			quests = QuestsSection_v99.Read(reader);
			waypoints = WaypointsSection_v99.Read(reader);
			npc_dialog = NPCDialogSection_v99.Read(reader);
			attributes = Attributes_v99.Read(reader);

			class_skills = ClassSkills_v99.Read(reader, class_id);
			item_list = ItemList_v99.Read(reader, header.version);
			player_corpses = CorpseList_v99.Read(reader, header.version);

			// this expansion check is likely correct, but not for my use case
			//if (Status.IsExpansion)
			//{
			if (reader.Position != reader.Length)
				mercenary_items = MercenaryItemList.Read(reader, mercenary, header.version);

			if (reader.Position != reader.Length)
				golem = Golem_v99.Read(reader, header.version);
			//}
		}

		public void Write(IBitWriter writer)
		{
			header.Write(writer);
			writer.WriteUInt32(active_weapon);
			writer.WriteString(name, 16);
			writer.WriteByte((byte)status);
			writer.WriteByte(progression);
			//Unk0x0026
			writer.WriteBytes(active_weapon_set ?? new byte[2]);
			writer.WriteByte(class_id);
			//Unk0x0029
			writer.WriteBytes(unk0x0029 ?? stackalloc byte[] { 0x10, 0x1e });
			writer.WriteByte(level);
			writer.WriteUInt32(created);
			writer.WriteUInt32(last_played);
			writer.WriteUInt32(play_time);
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

		public static Character_v99 Read(ReadOnlySpan<byte> bytes)
		{
			using var reader = new BitReader(bytes);
			var d2s = new Character_v99(reader);
			int bytesToRead = bytes.Length * 8;
			int bytesNotRead = bytesToRead - reader.Position;
			Debug.Assert(bytesNotRead == 0);
			return d2s;
		}

		public static MemoryOwner<byte> WritePooled(Character_v99 character)
		{
			using var writer = new BitWriter();
			character.Write(writer);
			var bytes = writer.ToPooledArray();
			Header_v99.Fix(bytes.Span);
			return bytes;
		}

		public static byte[] Write(Character_v99 character)
		{
			using var writer = new BitWriter();
			character.Write(writer);
			byte[] bytes = writer.ToArray();
			Header_v99.Fix(bytes);
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
