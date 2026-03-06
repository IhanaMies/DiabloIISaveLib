using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DiabloIISaveLib.Constants.v105
{
	public static class Data
	{
		public static Dictionary<string, ItemStatCost> itemStatCosts = new();
		public static Dictionary<string, ItemType> itemTypes = new();

		public static void ReadItemStatCosts(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\excel\\itemstatcost.txt");

			for (int i = 1; i < lines.Length; i++)
			{
				string[] values = lines[i].Split('\t');

				itemStatCosts.Add(values[0], new()
				{
					stat = values[0],
					id = int.TryParse(values[1], out int id) ? id : null,
					send_other = bool.TryParse(values[2], out bool send_other) ? send_other : null,
					signed = bool.TryParse(values[3], out bool signed) ? signed : null,
					send_bits = int.TryParse(values[4], out int send_bits) ? send_bits : null,
					send_param_bits = int.TryParse(values[5], out int send_param_bits) ? send_param_bits : null,
					update_anim_rate = bool.TryParse(values[6], out bool update_anim_rate) ? update_anim_rate : null,
					saved = bool.TryParse(values[7], out bool saved) ? saved : null,
					csv_signed = bool.TryParse(values[8], out bool csv_signed) ? csv_signed : null,
					csv_bits = int.TryParse(values[9], out int csv_bits) ? csv_bits : null,
					csv_param = int.TryParse(values[10], out int csv_param) ? csv_param : null,
					f_callback = bool.TryParse(values[11], out bool f_callback) ? f_callback : null,
					f_min = bool.TryParse(values[12], out bool f_min) ? f_min : null,
					min_accr = int.TryParse(values[13], out int min_accr) ? min_accr : null,
					encode = int.TryParse(values[14], out int encode) ? encode : null,
					add = int.TryParse(values[15], out int add) ? add : null,
					multiply = int.TryParse(values[16], out int multiply) ? multiply : null,
					val_shift = int.TryParse(values[17], out int val_shift) ? val_shift : null,
					save_bits_109 = int.TryParse(values[18], out int save_bits_109) ? save_bits_109 : null,
					save_add_109 = int.TryParse(values[19], out int save_add_109) ? save_add_109 : null,
					save_bits = int.TryParse(values[20], out int save_bits) ? save_bits : null,
					save_add = int.TryParse(values[21], out int save_add) ? save_add : null,
					save_param_bits = int.TryParse(values[22], out int save_param_bits) ? save_param_bits : null,
					keep_zero = bool.TryParse(values[23], out bool keep_zero) ? keep_zero : null,
					op = int.TryParse(values[24], out int op) ? op : null,
					op_param = int.TryParse(values[25], out int op_param) ? op_param : null,
					op_base = int.TryParse(values[26], out int op_base) ? op_base : null,
					op_stat1 = int.TryParse(values[27], out int op_stat1) ? op_stat1 : null,
					op_stat2 = int.TryParse(values[28], out int op_stat2) ? op_stat2 : null,
					op_stat3 = int.TryParse(values[29], out int op_stat3) ? op_stat3 : null,
					direct = bool.TryParse(values[30], out bool direct) ? direct : null,
					maxstat = values[31],
					damage_related = bool.TryParse(values[32], out bool damage_related) ? damage_related : null,
					item_event1 = int.TryParse(values[33], out int item_event1) ? item_event1 : null,
					item_event_func1 = int.TryParse(values[34], out int item_event_func1) ? item_event_func1 : null,
					item_event2 = int.TryParse(values[35], out int item_event2) ? item_event2 : null,
					item_event_func2 = int.TryParse(values[36], out int item_event_func2) ? item_event_func2 : null,
					desc_priority = int.TryParse(values[37], out int desc_priority) ? desc_priority : null,
					desc_func = int.TryParse(values[38], out int desc_func) ? desc_func : null,
					desc_val = int.TryParse(values[39], out int desc_val) ? desc_val : null,
					desc_str_pos = values[40],
					desc_str_neg = values[41],
					desc_str2 = values[42],
					d_grp = int.TryParse(values[43], out int d_grp) ? d_grp : null,
					d_grp_func = int.TryParse(values[44], out int d_grp_func) ? d_grp_func : null,
					d_grp_val = int.TryParse(values[45], out int d_grp_val) ? d_grp_val : null,
					d_grp_str_pos = values[46],
					d_grp_str_neg = values[47],
					d_grp_str2 = values[48],
					stuff = int.TryParse(values[49], out int stuff) ? stuff : null,
					adv_display = int.TryParse(values[50], out int adv_display) ? adv_display : null
				});
			}
		}

		public static void ReadItemTypes(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\excel\\itemtypes.txt");
			string[] headers = lines[0].Split('\t');
			PropertyInfo[] properties = typeof(ItemType).GetProperties();

			for (int i = 0; i < headers.Length; i++)
			{
				if (properties[i].Name != headers[i])
				{
					throw new InvalidOperationException($"Table header ({headers[i]}) does not match with itemtype declaration ({properties[i]})");
				}
			}

			for (int i = 1; i < lines.Length; i++)
			{
				string[] values = lines[i].Split('\t');

				if (string.IsNullOrWhiteSpace(values[1])) continue;

				itemTypes.Add(values[0], new()
				{
					Type = values[0],
					Code = values[1],
					Equiv1 = values[2],
					Equiv2 = values[3],
					Repair = bool.TryParse(values[4], out bool repair) ? repair : null,
					Body = bool.TryParse(values[5], out bool body) ? body : null,
					BodyLoc1 = values[6],
					BodyLoc2 = values[7],
					Shoots = values[8],
					Quiver = values[9],
					Throwable = bool.TryParse(values[10], out bool throwable) ? throwable : null,
					Reload = bool.TryParse(values[11], out bool reload) ? reload : null,
					ReEquip = bool.TryParse(values[12], out bool reEquip) ? reEquip : null,
					AutoStack = bool.TryParse(values[13], out bool autoStack) ? autoStack : null,
					Magic = bool.TryParse(values[14], out bool magic) ? magic : null,
					Rare = bool.TryParse(values[15], out bool rare) ? rare : null,
					Normal = bool.TryParse(values[16], out bool normal) ? normal : null,
					Beltable = bool.TryParse(values[17], out bool beltable) ? beltable : null,
					MaxSockets1 = int.TryParse(values[18], out int maxSockets1) ? maxSockets1 : null,
					MaxSocketsLevelThreshold1 = int.TryParse(values[19], out int maxSocketsLevelThreshold1) ? maxSocketsLevelThreshold1 : null,
					MaxSockets2 = int.TryParse(values[20], out int maxSockets2) ? maxSockets2 : null,
					MaxSocketsLevelThreshold2 = int.TryParse(values[21], out int maxSocketsLevelThreshold2) ? maxSocketsLevelThreshold2 : null,
					MaxSockets3 = int.TryParse(values[22], out int maxSockets3) ? maxSockets3 : null,
					TreasureClass = bool.TryParse(values[23], out bool treasureClass) ? treasureClass : null,
					Rarity = int.TryParse(values[24], out int rarity) ? rarity : null,
					StaffMods = values[25],
					Class = values[26],
					VarInvGfx = int.TryParse(values[27], out int varInvGfx) ? varInvGfx : null,
					InvGfx1 = values[28],
					InvGfx2 = values[29],
					InvGfx3 = values[30],
					InvGfx4 = values[31],
					InvGfx5 = values[32],
					InvGfx6 = values[33],
					StorePage = values[34]
				});
			}
		}
	}
	
	public class ItemStatCost
	{
		public string? stat { get; set; }
		public int? id { get; set; }
		public bool? send_other { get; set; }
		public bool? signed { get; set; }
		public int? send_bits { get; set; }
		public int? send_param_bits { get; set; }
		public bool? update_anim_rate { get; set; }
		public bool? saved { get; set; }
		public bool? csv_signed { get; set; }
		public int? csv_bits { get; set; }
		public int? csv_param { get; set; }
		public bool? f_callback { get; set; }
		public bool? f_min { get; set; }
		public int? min_accr { get; set; }
		public int? encode { get; set; }
		public int? add { get; set; }
		public int? multiply { get; set; }
		public int? val_shift { get; set; }
		public int? save_bits_109 { get; set; }
		public int? save_add_109 { get; set; }
		public int? save_bits { get; set; }
		public int? save_add { get; set; }
		public int? save_param_bits { get; set; }
		public bool? keep_zero { get; set; }
		public int? op { get; set; }
		public int? op_param { get; set; }
		public int? op_base { get; set; }
		public int? op_stat1 { get; set; }
		public int? op_stat2 { get; set; }
		public int? op_stat3 { get; set; }
		public bool? direct { get; set; }
		public string? maxstat { get; set; }
		public bool? damage_related { get; set; }
		public int? item_event1 { get; set; }
		public int? item_event_func1 { get; set; }
		public int? item_event2 { get; set; }
		public int? item_event_func2 { get; set; }
		public int? desc_priority { get; set; }
		public int? desc_func { get; set; }
		public int? desc_val { get; set; }
		public string? desc_str_pos { get; set; }
		public string? desc_str_neg { get; set; }
		public string? desc_str2 { get; set; }
		public int? d_grp { get; set; }
		public int? d_grp_func { get; set; }
		public int? d_grp_val { get; set; }
		public string? d_grp_str_pos { get; set; }
		public string? d_grp_str_neg { get; set; }
		public string? d_grp_str2 { get; set; }
		public int? stuff { get; set; }
		public int? adv_display { get; set; }
	}

	public class ItemType
	{
		public string? Type { get; set; }
		public string? Code { get; set; }
		public string? Equiv1 { get; set; }
		public string? Equiv2 { get; set; }
		public bool? Repair { get; set; }
		public bool? Body { get; set; }
		public string? BodyLoc1 { get; set; }
		public string? BodyLoc2 { get; set; }
		public string? Shoots { get; set; }
		public string? Quiver { get; set; }
		public bool? Throwable { get; set; }
		public bool? Reload { get; set; }
		public bool? ReEquip { get; set; }
		public bool? AutoStack { get; set; }
		public bool? Magic { get; set; }
		public bool? Rare { get; set; }
		public bool? Normal { get; set; }
		public bool? Beltable { get; set; }
		public int? MaxSockets1 { get; set; }
		public int? MaxSocketsLevelThreshold1 { get; set; }
		public int? MaxSockets2 { get; set; }
		public int? MaxSocketsLevelThreshold2 { get; set; }
		public int? MaxSockets3 { get; set; }
		public bool? TreasureClass { get; set; }
		public int? Rarity { get; set; }
		public string? StaffMods { get; set; }
		public string? Class { get; set; }
		public int? VarInvGfx { get; set; }
		public string? InvGfx1 { get; set; }
		public string? InvGfx2 { get; set; }
		public string? InvGfx3 { get; set; }
		public string? InvGfx4 { get; set; }
		public string? InvGfx5 { get; set; }
		public string? InvGfx6 { get; set; }
		public string? StorePage { get; set; }
	}

	public class Armor
	{
		public string? name { get; set; }
		public int? version { get; set; }
		public bool? compactsave { get; set; }
		public int? rarity { get; set; }
		public bool? spawnable { get; set; }
		public string? DropConditionCalc { get; set; }
		public int? minac { get; set; }
		public int? maxac { get; set; }
		public int? speed { get; set; }
		public int? reqstr { get; set; }
		public int? reqdex { get; set; }
		public int? block { get; set; }
		public int? durability { get; set; }
		public int? nodurability { get; set; }
		public int? level { get; set; }
		public int? ShowLevel { get; set; }
		public int? levelreq { get; set; }
		public int? cost { get; set; }
		/// <summary>
		/// Defines the gambling gold cost of the item on the Gambling UI. Only functions for rings and amulets.
		/// </summary>
		public int? gamble_cost { get; set; }
		public string? code { get; set; }
		public string? namestr { get; set; }
		public int? magic_lvl { get; set; }
		public int? auto_prefix { get; set; }
		public string? alternategfx { get; set; }
		public string? normcode { get; set; }
		public string? ubercode { get; set; }
		public string? ultracode { get; set; }
		public int? component { get; set; }
		public int? invwidth { get; set; }
		public int? invheight { get; set; }
		public int? hasinv { get; set; }
		public int? gemsockets { get; set; }
		public int? gemapplytype { get; set; }
		public string? flippyfile { get; set; }
		public string? invfile { get; set; }
		public string? uniqueinvfile { get; set; }
		public string? setinvfile { get; set; }
		public int? rArm { get; set; }
		public int? lArm { get; set; }
		public int? Torso { get; set; }
		public int? Legs { get; set; }
		public int? rSPad { get; set; }
		public int? lSPad { get; set; }
		public bool? useable { get; set; }
		public bool? stackable { get; set; }
		public int? minstack { get; set; }
		public int? maxstack { get; set; }
		public int? spawnstack { get; set; }
		public bool? Transmogrify { get; set; }
		public string? TMogType { get; set; }
		public int? TMogMin { get; set; }
		public int? TMogMax { get; set; }
		public string? type { get; set; }
		public string? type2 { get; set; }
		public string? dropsound { get; set; }
		public int? dropsfxframe { get; set; }
		public string? usesound { get; set; }
		public bool? unique { get; set; }
		public bool? transparent { get; set; }
		public int? transtbl { get; set; }
		public bool? quivered { get; set; }
		public int? lightradius { get; set; }
		public int? belt { get; set; }
		public string? quest { get; set; }
		public string? questdiffcheck { get; set; }
		public bool? missiletype { get; set; }
		public int? durwarning { get; set; }
		public int? qntwarning { get; set; }
		public int? mindam { get; set; }
		public int? maxdam { get; set; }
		public int? StrBonus { get; set; }
		public int? DexBonus { get; set; }
		public int? gemoffset { get; set; }
		public int? bitfield1 { get; set; }
		public int? CharsiMin { get; set; }
		public int? CharsiMax { get; set; }
		public int? CharsiMagicMin { get; set; }
		public int? CharsiMagicMax { get; set; }
		public int? CharsiMagicLvl { get; set; }
		public int? GheedMin { get; set; }
		public int? GheedMax { get; set; }
		public int? GheedMagicMin { get; set; }
		public int? GheedMagicMax { get; set; }
		public int? GheedMagicLvl { get; set; }
		public int? AkaraMin { get; set; }
		public int? AkaraMax { get; set; }
		public int? AkaraMagicMin { get; set; }
		public int? AkaraMagicMax { get; set; }
		public int? AkaraMagicLvl { get; set; }
		public int? FaraMin { get; set; }
		public int? FaraMax { get; set; }
		public int? FaraMagicMin { get; set; }
		public int? FaraMagicMax { get; set; }
		public int? FaraMagicLvl { get; set; }
		public int? LysanderMin { get; set; }
		public int? LysanderMax { get; set; }
		public int? LysanderMagicMin { get; set; }
		public int? LysanderMagicMax { get; set; }
		public int? LysanderMagicLvl { get; set; }
		public int? DrognanMin { get; set; }
		public int? DrognanMax { get; set; }
		public int? DrognanMagicMin { get; set; }
		public int? DrognanMagicMax { get; set; }
		public int? DrognanMagicLvl { get; set; }
		public int? HratliMin { get; set; }
		public int? HratliMax { get; set; }
		public int? HratliMagicMin { get; set; }
		public int? HratliMagicMax { get; set; }
		public int? HratliMagicLvl { get; set; }
		public int? AlkorMin { get; set; }
		public int? AlkorMax { get; set; }
		public int? AlkorMagicMin { get; set; }
		public int? AlkorMagicMax { get; set; }
		public int? AlkorMagicLvl { get; set; }
		public int? OrmusMin { get; set; }
		public int? OrmusMax { get; set; }
		public int? OrmusMagicMin { get; set; }
		public int? OrmusMagicMax { get; set; }
		public int? OrmusMagicLvl { get; set; }
		public int? ElzixMin { get; set; }
		public int? ElzixMax { get; set; }
		public int? ElzixMagicMin { get; set; }
		public int? ElzixMagicMax { get; set; }
		public int? ElzixMagicLvl { get; set; }
		public int? AshearaMin { get; set; }
		public int? AshearaMax { get; set; }
		public int? AshearaMagicMin { get; set; }
		public int? AshearaMagicMax { get; set; }
		public int? AshearaMagicLvl { get; set; }
		public int? CainMin { get; set; }
		public int? CainMax { get; set; }
		public int? CainMagicMin { get; set; }
		public int? CainMagicMax { get; set; }
		public int? CainMagicLvl { get; set; }
		public int? HalbuMin { get; set; }
		public int? HalbuMax { get; set; }
		public int? HalbuMagicMin { get; set; }
		public int? HalbuMagicMax { get; set; }
		public int? HalbuMagicLvl { get; set; }
		public int? JamellaMin { get; set; }
		public int? JamellaMax { get; set; }
		public int? JamellaMagicMin { get; set; }
		public int? JamellaMagicMax { get; set; }
		public int? JamellaMagicLvl { get; set; }
		public int? LarzukMin { get; set; }
		public int? LarzukMax { get; set; }
		public int? LarzukMagicMin { get; set; }
		public int? LarzukMagicMax { get; set; }
		public int? LarzukMagicLvl { get; set; }
		public int? MalahMin { get; set; }
		public int? MalahMax { get; set; }
		public int? MalahMagicMin { get; set; }
		public int? MalahMagicMax { get; set; }
		public int? MalahMagicLvl { get; set; }
		public int? AnyaMin { get; set; }
		public int? AnyaMax { get; set; }
		public int? AnyaMagicMin { get; set; }
		public int? AnyaMagicMax { get; set; }
		public int? AnyaMagicLvl { get; set; }
		public int? Transform { get; set; }
		public int? InvTrans { get; set; }
		public bool? SkipName { get; set; }
		public string? NightmareUpgrade { get; set; }
		public string? HellUpgrade { get; set; }
		public bool? Nameable { get; set; }
		public bool? PermStoreItem { get; set; }
		public string? UICatOverride { get; set; }
		public int? diablocloneweight { get; set; }
	}
}
