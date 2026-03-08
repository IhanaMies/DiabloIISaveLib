using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static DiabloIISaveLib.Helpers;

namespace DiabloIISaveLib.Constants.v99
{
	public static class Data
	{
		public static Dictionary<string, ItemStatCost> itemStatCosts = new();
		public static Dictionary<string, ItemType> itemTypes = new();
		public static Dictionary<string, Armor> armors = new();
		public static Dictionary<string, Misc> miscs = new();
		public static Dictionary<string, Weapon> weapons = new();
		public static Dictionary<string, Unique> uniques = new();
		public static Dictionary<string, SetItem> setItems = new();

		public static void LoadData(string root)
		{
			ReadItemStatCosts(root);
			ReadItemTypes(root);
			ReadArmors(root);
			ReadMisc(root);
			ReadWeapons(root);
			ReadUniqueItems(root);
			ReadSetItems(root);
		}

		public static void ReadItemStatCosts(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\excel\\itemstatcost.txt");

			for (int i = 1; i < lines.Length; i++)
			{
				string[] values = lines[i].Split('\t');

				itemStatCosts.Add(values[0], new()
				{
					stat = values[0],
					id = ParseInt(values[1]),
					send_other = ParseBool(values[2]),
					signed = ParseBool(values[3]),
					send_bits = ParseInt(values[4]),
					send_param_bits = ParseInt(values[5]),
					update_anim_rate = ParseBool(values[6]),
					saved = ParseBool(values[7]),
					csv_signed = ParseBool(values[8]),
					csv_bits = ParseInt(values[9]),
					csv_param = ParseInt(values[10]),
					f_callback = ParseBool(values[11]),
					f_min = ParseBool(values[12]),
					min_accr = ParseInt(values[13]),
					encode = ParseInt(values[14]),
					add = ParseInt(values[15]),
					multiply = ParseInt(values[16]),
					val_shift = ParseInt(values[17]),
					save_bits_109 = ParseInt(values[18]),
					save_add_109 = ParseInt(values[19]),
					save_bits = ParseInt(values[20]),
					save_add = ParseInt(values[21]),
					save_param_bits = ParseInt(values[22]),
					keep_zero = ParseBool(values[23]),
					op = ParseInt(values[24]),
					op_param = ParseInt(values[25]),
					op_base = ParseInt(values[26]),
					op_stat1 = ParseInt(values[27]),
					op_stat2 = ParseInt(values[28]),
					op_stat3 = ParseInt(values[29]),
					direct = ParseBool(values[30]),
					maxstat = values[31],
					damage_related = ParseBool(values[32]),
					item_event1 = ParseInt(values[33]),
					item_event_func1 = ParseInt(values[34]),
					item_event2 = ParseInt(values[35]),
					item_event_func2 = ParseInt(values[36]),
					desc_priority = ParseInt(values[37]),
					desc_func = ParseInt(values[38]),
					desc_val = ParseInt(values[39]),
					desc_str_pos = values[40],
					desc_str_neg = values[41],
					desc_str2 = values[42],
					d_grp = ParseInt(values[43]),
					d_grp_func = ParseInt(values[44]),
					d_grp_val = ParseInt(values[45]),
					d_grp_str_pos = values[46],
					d_grp_str_neg = values[47],
					d_grp_str2 = values[48],
					stuff = ParseInt(values[49]),
					adv_display = ParseInt(values[50])
				});
			}
		}

		public static void ReadItemTypes(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\excel\\itemtypes.txt");

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
					Repair = ParseBool(values[4]),
					Body = ParseBool(values[5]),
					BodyLoc1 = values[6],
					BodyLoc2 = values[7],
					Shoots = values[8],
					Quiver = values[9],
					Throwable = ParseBool(values[10]),
					Reload = ParseBool(values[11]),
					ReEquip = ParseBool(values[12]),
					AutoStack = ParseBool(values[13]),
					Magic = ParseBool(values[14]),
					Rare = ParseBool(values[15]),
					Normal = ParseBool(values[16]),
					Beltable = ParseBool(values[17]),
					MaxSockets1 = ParseInt(values[18]),
					MaxSocketsLevelThreshold1 = ParseInt(values[19]),
					MaxSockets2 = ParseInt(values[20]),
					MaxSocketsLevelThreshold2 = ParseInt(values[21]),
					MaxSockets3 = ParseInt(values[22]),
					TreasureClass = ParseBool(values[23]),
					Rarity = ParseInt(values[24]),
					StaffMods = values[25],
					Class = values[26],
					VarInvGfx = ParseInt(values[27]),
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

		// Implementation for reading armors.txt
		public static void ReadArmors(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\excel\\armors.txt");

			for (int i = 1; i < lines.Length; i++)
			{
				var values = lines[i].Split('\t');
				var key = values[0];
				if (string.IsNullOrWhiteSpace(key)) continue;

				armors.Add(key, new Armor
				{
					name = key,
					version = ParseInt(values[1]),
					compactsave = ParseBool(values[2]),
					rarity = ParseInt(values[3]),
					spawnable = ParseBool(values[4]),
					minac = ParseInt(values[5]),
					maxac = ParseInt(values[6]),
					speed = ParseInt(values[7]),
					reqstr = ParseInt(values[8]),
					reqdex = ParseInt(values[9]),
					block = ParseInt(values[10]),
					durability = ParseInt(values[11]),
					nodurability = ParseInt(values[12]),
					level = ParseInt(values[13]),
					ShowLevel = ParseInt(values[14]),
					levelreq = ParseInt(values[15]),
					cost = ParseInt(values[16]),
					gamble_cost = ParseInt(values[17]),
					code = values[18],
					namestr = values[19],
					magic_lvl = ParseInt(values[20]),
					auto_prefix = ParseInt(values[21]),
					alternategfx = values[22],
					normcode = values[23],
					ubercode = values[24],
					ultracode = values[25],
					component = ParseInt(values[26]),
					invwidth = ParseInt(values[27]),
					invheight = ParseInt(values[28]),
					hasinv = ParseInt(values[29]),
					gemsockets = ParseInt(values[30]),
					gemapplytype = ParseInt(values[31]),
					flippyfile = values[32],
					invfile = values[33],
					uniqueinvfile = values[34],
					setinvfile = values[35],
					rArm = ParseInt(values[36]),
					lArm = ParseInt(values[37]),
					Torso = ParseInt(values[38]),
					Legs = ParseInt(values[39]),
					rSPad = ParseInt(values[40]),
					lSPad = ParseInt(values[41]),
					useable = ParseBool(values[42]),
					stackable = ParseBool(values[43]),
					minstack = ParseInt(values[44]),
					maxstack = ParseInt(values[45]),
					spawnstack = ParseInt(values[46]),
					Transmogrify = ParseBool(values[47]),
					TMogType = values[48],
					TMogMin = ParseInt(values[49]),
					TMogMax = ParseInt(values[50]),
					type = values[51],
					type2 = values[52],
					dropsound = values[53],
					dropsfxframe = ParseInt(values[54]),
					usesound = values[55],
					unique = ParseBool(values[56]),
					transparent = ParseBool(values[57]),
					transtbl = ParseInt(values[58]),
					quivered = ParseBool(values[59]),
					lightradius = ParseInt(values[60]),
					belt = ParseInt(values[61]),
					quest = values[62],
					questdiffcheck = values[63],
					missiletype = ParseBool(values[64]),
					durwarning = ParseInt(values[65]),
					qntwarning = ParseInt(values[66]),
					mindam = ParseInt(values[67]),
					maxdam = ParseInt(values[68]),
					StrBonus = ParseInt(values[69]),
					DexBonus = ParseInt(values[70]),
					gemoffset = ParseInt(values[71]),
					bitfield1 = ParseInt(values[72]),
					CharsiMin = ParseInt(values[73]),
					CharsiMax = ParseInt(values[74]),
					CharsiMagicMin = ParseInt(values[75]),
					CharsiMagicMax = ParseInt(values[76]),
					CharsiMagicLvl = ParseInt(values[77]),
					GheedMin = ParseInt(values[78]),
					GheedMax = ParseInt(values[79]),
					GheedMagicMin = ParseInt(values[80]),
					GheedMagicMax = ParseInt(values[81]),
					GheedMagicLvl = ParseInt(values[82]),
					AkaraMin = ParseInt(values[83]),
					AkaraMax = ParseInt(values[84]),
					AkaraMagicMin = ParseInt(values[85]),
					AkaraMagicMax = ParseInt(values[86]),
					AkaraMagicLvl = ParseInt(values[87]),
					FaraMin = ParseInt(values[88]),
					FaraMax = ParseInt(values[89]),
					FaraMagicMin = ParseInt(values[90]),
					FaraMagicMax = ParseInt(values[91]),
					FaraMagicLvl = ParseInt(values[92]),
					LysanderMin = ParseInt(values[93]),
					LysanderMax = ParseInt(values[94]),
					LysanderMagicMin = ParseInt(values[95]),
					LysanderMagicMax = ParseInt(values[96]),
					LysanderMagicLvl = ParseInt(values[97]),
					DrognanMin = ParseInt(values[98]),
					DrognanMax = ParseInt(values[99]),
					DrognanMagicMin = ParseInt(values[100]),
					DrognanMagicMax = ParseInt(values[101]),
					DrognanMagicLvl = ParseInt(values[102]),
					HratliMin = ParseInt(values[103]),
					HratliMax = ParseInt(values[104]),
					HratliMagicMin = ParseInt(values[105]),
					HratliMagicMax = ParseInt(values[106]),
					HratliMagicLvl = ParseInt(values[107]),
					AlkorMin = ParseInt(values[108]),
					AlkorMax = ParseInt(values[109]),
					AlkorMagicMin = ParseInt(values[110]),
					AlkorMagicMax = ParseInt(values[111]),
					AlkorMagicLvl = ParseInt(values[112]),
					OrmusMin = ParseInt(values[113]),
					OrmusMax = ParseInt(values[114]),
					OrmusMagicMin = ParseInt(values[115]),
					OrmusMagicMax = ParseInt(values[116]),
					OrmusMagicLvl = ParseInt(values[117]),
					ElzixMin = ParseInt(values[118]),
					ElzixMax = ParseInt(values[119]),
					ElzixMagicMin = ParseInt(values[120]),
					ElzixMagicMax = ParseInt(values[121]),
					ElzixMagicLvl = ParseInt(values[122]),
					AshearaMin = ParseInt(values[123]),
					AshearaMax = ParseInt(values[124]),
					AshearaMagicMin = ParseInt(values[125]),
					AshearaMagicMax = ParseInt(values[126]),
					AshearaMagicLvl = ParseInt(values[127]),
					CainMin = ParseInt(values[128]),
					CainMax = ParseInt(values[129]),
					CainMagicMin = ParseInt(values[130]),
					CainMagicMax = ParseInt(values[131]),
					CainMagicLvl = ParseInt(values[132]),
					HalbuMin = ParseInt(values[133]),
					HalbuMax = ParseInt(values[134]),
					HalbuMagicMin = ParseInt(values[135]),
					HalbuMagicMax = ParseInt(values[136]),
					HalbuMagicLvl = ParseInt(values[137]),
					JamellaMin = ParseInt(values[138]),
					JamellaMax = ParseInt(values[139]),
					JamellaMagicMin = ParseInt(values[140]),
					JamellaMagicMax = ParseInt(values[141]),
					JamellaMagicLvl = ParseInt(values[142]),
					LarzukMin = ParseInt(values[143]),
					LarzukMax = ParseInt(values[144]),
					LarzukMagicMin = ParseInt(values[145]),
					LarzukMagicMax = ParseInt(values[146]),
					LarzukMagicLvl = ParseInt(values[147]),
					MalahMin = ParseInt(values[148]),
					MalahMax = ParseInt(values[149]),
					MalahMagicMin = ParseInt(values[150]),
					MalahMagicMax = ParseInt(values[151]),
					MalahMagicLvl = ParseInt(values[152]),
					AnyaMin = ParseInt(values[153]),
					AnyaMax = ParseInt(values[154]),
					AnyaMagicMin = ParseInt(values[155]),
					AnyaMagicMax = ParseInt(values[156]),
					AnyaMagicLvl = ParseInt(values[157]),
					Transform = ParseInt(values[158]),
					InvTrans = ParseInt(values[159]),
					SkipName = ParseBool(values[160]),
					NightmareUpgrade = values[161],
					HellUpgrade = values[162],
					Nameable = ParseBool(values[163]),
					PermStoreItem = ParseBool(values[164]),
					diablocloneweight = ParseInt(values[165])
				});
			}
		}

		// Implementation for reading misc.txt
		public static void ReadMisc(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\excel\\misc.txt");

			for (int i = 1; i < lines.Length; i++)
			{
				var values = lines[i].Split('\t');
				var key = values[0];
				if (string.IsNullOrWhiteSpace(key)) continue;

				miscs.Add(key, new Misc
				{
					name = key,
					compactsave = ParseBool(values[1]),
					version = ParseInt(values[2]),
					level = ParseInt(values[3]),
					ShowLevel = ParseInt(values[4]),
					levelreq = ParseInt(values[5]),
					reqstr = ParseInt(values[6]),
					reqdex = ParseInt(values[7]),
					rarity = ParseInt(values[8]),
					spawnable = ParseBool(values[9]),
					speed = ParseInt(values[10]),
					nodurability = ParseInt(values[11]),
					cost = ParseInt(values[12]),
					gamble_cost = ParseInt(values[13]),
					code = values[14],
					alternategfx = values[15],
					namestr = values[16],
					component = ParseInt(values[17]),
					invwidth = ParseInt(values[18]),
					invheight = ParseInt(values[19]),
					hasinv = ParseInt(values[20]),
					gemsockets = ParseInt(values[21]),
					gemapplytype = ParseInt(values[22]),
					flippyfile = values[23],
					invfile = values[24],
					uniqueinvfile = values[25],
					Transmogrify = ParseBool(values[26]),
					TMogType = values[27],
					TMogMin = ParseInt(values[28]),
					TMogMax = ParseInt(values[29]),
					useable = ParseBool(values[30]),
					type = values[31],
					type2 = values[32],
					dropsound = values[33],
					dropsfxframe = ParseInt(values[34]),
					usesound = values[35],
					unique = ParseBool(values[36]),
					transparent = ParseBool(values[37]),
					transtbl = ParseInt(values[38]),
					quivered = ParseBool(values[39]),
					lightradius = ParseInt(values[40]),
					belt = ParseInt(values[41]),
					autobelt = ParseBool(values[42]),
					stackable = ParseBool(values[43]),
					minstack = ParseInt(values[44]),
					maxstack = ParseInt(values[45]),
					spawnstack = ParseInt(values[46]),
					quest = values[47],
					questdiffcheck = values[48],
					missiletype = ParseBool(values[49]),
					spellicon = ParseInt(values[50]),
					pSpell = ParseInt(values[51]),
					state = values[52],
					cstate1 = values[53],
					cstate2 = values[54],
					len = ParseInt(values[55]),
					stat1 = values[56],
					calc1 = ParseInt(values[57]),
					stat2 = values[58],
					calc2 = ParseInt(values[59]),
					stat3 = values[60],
					calc3 = ParseInt(values[61]),
					spelldesc = ParseInt(values[62]),
					spelldescstr = values[63],
					spelldescstr2 = values[64],
					spelldesccalc = ParseInt(values[65]),
					spelldesccolor = ParseInt(values[66]),
					durwarning = ParseInt(values[67]),
					qntwarning = ParseInt(values[68]),
					mindam = ParseInt(values[69]),
					maxdam = ParseInt(values[70]),
					StrBonus = ParseInt(values[71]),
					DexBonus = ParseInt(values[72]),
					gemoffset = ParseInt(values[73]),
					bitfield1 = ParseInt(values[74]),
					CharsiMin = ParseInt(values[75]),
					CharsiMax = ParseInt(values[76]),
					CharsiMagicMin = ParseInt(values[77]),
					CharsiMagicMax = ParseInt(values[78]),
					CharsiMagicLvl = ParseInt(values[79]),
					GheedMin = ParseInt(values[80]),
					GheedMax = ParseInt(values[81]),
					GheedMagicMin = ParseInt(values[82]),
					GheedMagicMax = ParseInt(values[83]),
					GheedMagicLvl = ParseInt(values[84]),
					AkaraMin = ParseInt(values[85]),
					AkaraMax = ParseInt(values[86]),
					AkaraMagicMin = ParseInt(values[87]),
					AkaraMagicMax = ParseInt(values[88]),
					AkaraMagicLvl = ParseInt(values[89]),
					FaraMin = ParseInt(values[90]),
					FaraMax = ParseInt(values[91]),
					FaraMagicMin = ParseInt(values[92]),
					FaraMagicMax = ParseInt(values[93]),
					FaraMagicLvl = ParseInt(values[94]),
					LysanderMin = ParseInt(values[95]),
					LysanderMax = ParseInt(values[96]),
					LysanderMagicMin = ParseInt(values[97]),
					LysanderMagicMax = ParseInt(values[98]),
					LysanderMagicLvl = ParseInt(values[99]),
					DrognanMin = ParseInt(values[100]),
					DrognanMax = ParseInt(values[101]),
					DrognanMagicMin = ParseInt(values[102]),
					DrognanMagicMax = ParseInt(values[103]),
					DrognanMagicLvl = ParseInt(values[104]),
					HratliMin = ParseInt(values[105]),
					HratliMax = ParseInt(values[106]),
					HratliMagicMin = ParseInt(values[107]),
					HratliMagicMax = ParseInt(values[108]),
					HratliMagicLvl = ParseInt(values[109]),
					AlkorMin = ParseInt(values[110]),
					AlkorMax = ParseInt(values[111]),
					AlkorMagicMin = ParseInt(values[112]),
					AlkorMagicMax = ParseInt(values[113]),
					AlkorMagicLvl = ParseInt(values[114]),
					OrmusMin = ParseInt(values[115]),
					OrmusMax = ParseInt(values[116]),
					OrmusMagicMin = ParseInt(values[117]),
					OrmusMagicMax = ParseInt(values[118]),
					OrmusMagicLvl = ParseInt(values[119]),
					ElzixMin = ParseInt(values[120]),
					ElzixMax = ParseInt(values[121]),
					ElzixMagicMin = ParseInt(values[122]),
					ElzixMagicMax = ParseInt(values[123]),
					ElzixMagicLvl = ParseInt(values[124]),
					AshearaMin = ParseInt(values[125]),
					AshearaMax = ParseInt(values[126]),
					AshearaMagicMin = ParseInt(values[127]),
					AshearaMagicMax = ParseInt(values[128]),
					AshearaMagicLvl = ParseInt(values[129]),
					CainMin = ParseInt(values[130]),
					CainMax = ParseInt(values[131]),
					CainMagicMin = ParseInt(values[132]),
					CainMagicMax = ParseInt(values[133]),
					CainMagicLvl = ParseInt(values[134]),
					HalbuMin = ParseInt(values[135]),
					HalbuMax = ParseInt(values[136]),
					HalbuMagicMin = ParseInt(values[137]),
					HalbuMagicMax = ParseInt(values[138]),
					HalbuMagicLvl = ParseInt(values[139]),
					JamellaMin = ParseInt(values[140]),
					JamellaMax = ParseInt(values[141]),
					JamellaMagicMin = ParseInt(values[142]),
					JamellaMagicMax = ParseInt(values[143]),
					JamellaMagicLvl = ParseInt(values[144]),
					LarzukMin = ParseInt(values[145]),
					LarzukMax = ParseInt(values[146]),
					LarzukMagicMin = ParseInt(values[147]),
					LarzukMagicMax = ParseInt(values[148]),
					LarzukMagicLvl = ParseInt(values[149]),
					MalahMin = ParseInt(values[150]),
					MalahMax = ParseInt(values[151]),
					MalahMagicMin = ParseInt(values[152]),
					MalahMagicMax = ParseInt(values[153]),
					MalahMagicLvl = ParseInt(values[154]),
					AnyaMin = ParseInt(values[155]),
					AnyaMax = ParseInt(values[156]),
					AnyaMagicMin = ParseInt(values[157]),
					AnyaMagicMax = ParseInt(values[158]),
					AnyaMagicLvl = ParseInt(values[159]),
					Transform = ParseInt(values[160]),
					InvTrans = ParseInt(values[161]),
					SkipName = ParseBool(values[162]),
					NightmareUpgrade = values[163],
					HellUpgrade = values[164],
					Nameable = ParseBool(values[165]),
					PermStoreItem = ParseBool(values[166]),
					diablocloneweight = ParseInt(values[167])
					// if files include more fields, extend indexes accordingly
				});
			}
		}

		// Implementation for reading weapons.txt
		public static void ReadWeapons(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\excel\\weapons.txt");

			for (int i = 1; i < lines.Length; i++)
			{
				var values = lines[i].Split('\t');
				var key = values[0];
				if (string.IsNullOrWhiteSpace(key)) continue;

				weapons.Add(key, new Weapon
				{
					name = key,
					type = values[1],
					type2 = values[2],
					code = values[3],
					alternategfx = values[4],
					namestr = values[5],
					version = ParseInt(values[6]),
					compactsave = ParseBool(values[7]),
					rarity = ParseInt(values[8]),
					spawnable = ParseBool(values[9]),
					Transmogrify = ParseBool(values[10]),
					TMogType = values[11],
					TMogMin = ParseInt(values[12]),
					TMogMax = ParseInt(values[13]),
					mindam = ParseInt(values[14]),
					maxdam = ParseInt(values[15]),
					one_or_two_handed = ParseBool(values[16]),
					two_handed = ParseBool(values[17]),
					two_handed_min_dam = ParseInt(values[18]),
					two_handed_max_dam = ParseInt(values[19]),
					min_mis_dam = ParseInt(values[20]),
					max_mis_dam = ParseInt(values[21]),
					rangeadder = ParseInt(values[22]),
					speed = ParseInt(values[23]),
					StrBonus = ParseInt(values[24]),
					DexBonus = ParseInt(values[25]),
					reqstr = ParseInt(values[26]),
					reqdex = ParseInt(values[27]),
					durability = ParseInt(values[28]),
					nodurability = ParseInt(values[29]),
					level = ParseInt(values[30]),
					ShowLevel = ParseInt(values[31]),
					levelreq = ParseInt(values[32]),
					cost = ParseInt(values[33]),
					gamble_cost = ParseInt(values[34]),
					magic_lvl = ParseInt(values[35]),
					auto_prefix = ParseInt(values[36]),
					normcode = values[37],
					ubercode = values[38],
					ultracode = values[39],
					wclass = values[40],
					two_handed_class = values[41],
					component = ParseInt(values[42]),
					hitclass = values[43],
					invwidth = ParseInt(values[44]),
					invheight = ParseInt(values[45]),
					stackable = ParseBool(values[46]),
					minstack = ParseInt(values[47]),
					maxstack = ParseInt(values[48]),
					spawnstack = ParseInt(values[49]),
					flippyfile = values[50],
					invfile = values[51],
					uniqueinvfile = values[52],
					setinvfile = values[53],
					hasinv = ParseInt(values[54]),
					gemsockets = ParseInt(values[55]),
					gemapplytype = ParseInt(values[56]),
					comment = values[57],
					useable = ParseBool(values[58]),
					dropsound = values[59],
					dropsfxframe = ParseInt(values[60]),
					usesound = values[61],
					unique = ParseBool(values[62]),
					transparent = ParseBool(values[63]),
					transtbl = ParseInt(values[64]),
					quivered = ParseBool(values[65]),
					lightradius = ParseInt(values[66]),
					belt = ParseInt(values[67]),
					quest = values[68],
					questdiffcheck = values[69],
					missiletype = ParseBool(values[70]),
					durwarning = ParseInt(values[71]),
					qntwarning = ParseInt(values[72]),
					gemoffset = ParseInt(values[73]),
					bitfield1 = ParseInt(values[74]),
					CharsiMin = ParseInt(values[75]),
					CharsiMax = ParseInt(values[76]),
					CharsiMagicMin = ParseInt(values[77]),
					CharsiMagicMax = ParseInt(values[78]),
					CharsiMagicLvl = ParseInt(values[79]),
					GheedMin = ParseInt(values[80]),
					GheedMax = ParseInt(values[81]),
					GheedMagicMin = ParseInt(values[82]),
					GheedMagicMax = ParseInt(values[83]),
					GheedMagicLvl = ParseInt(values[84]),
					AkaraMin = ParseInt(values[85]),
					AkaraMax = ParseInt(values[86]),
					AkaraMagicMin = ParseInt(values[87]),
					AkaraMagicMax = ParseInt(values[88]),
					AkaraMagicLvl = ParseInt(values[89]),
					FaraMin = ParseInt(values[90]),
					FaraMax = ParseInt(values[91]),
					FaraMagicMin = ParseInt(values[92]),
					FaraMagicMax = ParseInt(values[93]),
					FaraMagicLvl = ParseInt(values[94]),
					LysanderMin = ParseInt(values[95]),
					LysanderMax = ParseInt(values[96]),
					LysanderMagicMin = ParseInt(values[97]),
					LysanderMagicMax = ParseInt(values[98]),
					LysanderMagicLvl = ParseInt(values[99]),
					DrognanMin = ParseInt(values[100]),
					DrognanMax = ParseInt(values[101]),
					DrognanMagicMin = ParseInt(values[102]),
					DrognanMagicMax = ParseInt(values[103]),
					DrognanMagicLvl = ParseInt(values[104]),
					HratliMin = ParseInt(values[105]),
					HratliMax = ParseInt(values[106]),
					HratliMagicMin = ParseInt(values[107]),
					HratliMagicMax = ParseInt(values[108]),
					HratliMagicLvl = ParseInt(values[109]),
					AlkorMin = ParseInt(values[110]),
					AlkorMax = ParseInt(values[111]),
					AlkorMagicMin = ParseInt(values[112]),
					AlkorMagicMax = ParseInt(values[113]),
					AlkorMagicLvl = ParseInt(values[114]),
					OrmusMin = ParseInt(values[115]),
					OrmusMax = ParseInt(values[116]),
					OrmusMagicMin = ParseInt(values[117]),
					OrmusMagicMax = ParseInt(values[118]),
					OrmusMagicLvl = ParseInt(values[119]),
					ElzixMin = ParseInt(values[120]),
					ElzixMax = ParseInt(values[121]),
					ElzixMagicMin = ParseInt(values[122]),
					ElzixMagicMax = ParseInt(values[123]),
					ElzixMagicLvl = ParseInt(values[124]),
					AshearaMin = ParseInt(values[125]),
					AshearaMax = ParseInt(values[126]),
					AshearaMagicMin = ParseInt(values[127]),
					AshearaMagicMax = ParseInt(values[128]),
					AshearaMagicLvl = ParseInt(values[129]),
					CainMin = ParseInt(values[130]),
					CainMax = ParseInt(values[131]),
					CainMagicMin = ParseInt(values[132]),
					CainMagicMax = ParseInt(values[133]),
					CainMagicLvl = ParseInt(values[134]),
					HalbuMin = ParseInt(values[135]),
					HalbuMax = ParseInt(values[136]),
					HalbuMagicMin = ParseInt(values[137]),
					HalbuMagicMax = ParseInt(values[138]),
					HalbuMagicLvl = ParseInt(values[139]),
					JamellaMin = ParseInt(values[140]),
					JamellaMax = ParseInt(values[141]),
					JamellaMagicMin = ParseInt(values[142]),
					JamellaMagicMax = ParseInt(values[143]),
					JamellaMagicLvl = ParseInt(values[144]),
					LarzukMin = ParseInt(values[145]),
					LarzukMax = ParseInt(values[146]),
					LarzukMagicMin = ParseInt(values[147]),
					LarzukMagicMax = ParseInt(values[148]),
					LarzukMagicLvl = ParseInt(values[149]),
					MalahMin = ParseInt(values[150]),
					MalahMax = ParseInt(values[151]),
					MalahMagicMin = ParseInt(values[152]),
					MalahMagicMax = ParseInt(values[153]),
					MalahMagicLvl = ParseInt(values[154]),
					AnyaMin = ParseInt(values[155]),
					AnyaMax = ParseInt(values[156]),
					AnyaMagicMin = ParseInt(values[157]),
					AnyaMagicMax = ParseInt(values[158]),
					AnyaMagicLvl = ParseInt(values[159]),
					Transform = ParseInt(values[160]),
					InvTrans = ParseInt(values[161]),
					SkipName = ParseBool(values[162]),
					NightmareUpgrade = values[163],
					HellUpgrade = values[164],
					Nameable = ParseBool(values[165]),
					PermStoreItem = ParseBool(values[166]),
					diablocloneweight = ParseInt(values[167])
				});
			}
		}

		// Implementation for reading uniques.txt
		public static void ReadUniqueItems(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\excel\\uniqueitems.txt");

			for (int i = 1; i < lines.Length; i++)
			{
				var values = lines[i].Split('\t');
				var key = values[0];
				if (string.IsNullOrWhiteSpace(key)) continue;

				uniques.Add(key, new Unique
				{
					ID = ParseInt(values[0]),
					version = ParseInt(values[1]),
					enabled = ParseBool(values[2]),
					firstLadderSeason = ParseInt(values[3]),
					lastLadderSeason = ParseInt(values[4]),
					rarity = ParseInt(values[5]),
					nolimit = values[6],
					lvl = ParseInt(values[7]),
					lvl_req = ParseInt(values[8]),
					code = values[9],
					ItemName = values[10],
					carry1 = ParseInt(values[11]),
					cost_mult = ParseInt(values[12]),
					cost_add = ParseInt(values[13]),
					chrtransform = values[14],
					invtransform = values[15],
					flippyfile = values[16],
					invfile = values[17],
					dropsound = values[18],
					dropsfxframe = ParseInt(values[19]),
					usesound = values[20],
					prop1 = values[21],
					par1 = ParseInt(values[22]),
					min1 = ParseInt(values[23]),
					max1 = ParseInt(values[24]),
					prop2 = values[25],
					par2 = values[26],
					min2 = ParseInt(values[27]),
					max2 = ParseInt(values[28]),
					prop3 = values[29],
					par3 = values[30],
					min3 = ParseInt(values[31]),
					max3 = ParseInt(values[32]),
					prop4 = values[33],
					par4 = values[34],
					min4 = ParseInt(values[35]),
					max4 = ParseInt(values[36]),
					prop5 = values[37],
					par5 = values[38],
					min5 = ParseInt(values[39]),
					max5 = ParseInt(values[40]),
					prop6 = values[41],
					par6 = values[42],
					min6 = ParseInt(values[43]),
					max6 = ParseInt(values[44]),
					prop7 = values[45],
					par7 = values[46],
					min7 = ParseInt(values[47]),
					max7 = ParseInt(values[48]),
					prop8 = values[49],
					par8 = values[50],
					min8 = ParseInt(values[51]),
					max8 = ParseInt(values[52]),
					prop9 = values[53],
					par9 = values[54],
					min9 = ParseInt(values[55]),
					max9 = ParseInt(values[56]),
					prop10 = values[57],
					par10 = values[58],
					min10 = ParseInt(values[59]),
					max10 = ParseInt(values[60]),
					prop11 = values[61],
					par11 = values[62],
					min11 = ParseInt(values[63]),
					max11 = ParseInt(values[64]),
					prop12 = values[65],
					par12 = ParseInt(values[66]),
					min12 = ParseInt(values[67]),
					max12 = ParseInt(values[68]),
					diablocloneweight = ParseInt(values[69])
				});
			}
		}

		// Implementation for reading sets.txt
		public static void ReadSetItems(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\excel\\setitems.txt");

			for (int i = 1; i < lines.Length; i++)
			{
				var values = lines[i].Split('\t');
				var key = values[0];
				if (string.IsNullOrWhiteSpace(key)) continue;

				setItems.Add(key, new SetItem
				{
					index = key,
					ID = values[1],
					set = values[2],
					item = values[3],
					ItemName = values[4],
					rarity = ParseInt(values[5]),
					lvl = ParseInt(values[6]),
					lvl_req = ParseInt(values[7]),
					chrtransform = values[8],
					invtransform = values[9],
					invfile = values[10],
					flippyfile = values[11],
					dropsound = values[12],
					dropsfxframe = values[13],
					usesound = values[14],
					cost_mult = ParseInt(values[15]),
					cost_add = ParseInt(values[16]),
					add_func = ParseInt(values[17]),
					prop1 = values[18],
					par1 = ParseInt(values[19]),
					min1 = ParseInt(values[20]),
					max1 = ParseInt(values[21]),
					prop2 = values[22],
					par2 = ParseInt(values[23]),
					min2 = ParseInt(values[24]),
					max2 = ParseInt(values[25]),
					prop3 = values[26],
					par3 = ParseInt(values[27]),
					min3 = ParseInt(values[28]),
					max3 = ParseInt(values[29]),
					prop4 = values[30],
					par4 = ParseInt(values[31]),
					min4 = ParseInt(values[32]),
					max4 = ParseInt(values[33]),
					prop5 = values[34],
					par5 = ParseInt(values[35]),
					min5 = ParseInt(values[36]),
					max5 = ParseInt(values[37]),
					prop6 = values[38],
					par6 = ParseInt(values[39]),
					min6 = ParseInt(values[40]),
					max6 = ParseInt(values[41]),
					prop7 = values[42],
					par7 = ParseInt(values[43]),
					min7 = ParseInt(values[44]),
					max7 = ParseInt(values[45]),
					prop8 = values[46],
					par8 = ParseInt(values[47]),
					min8 = ParseInt(values[48]),
					max8 = ParseInt(values[49]),
					prop9 = values[50],
					par9 = ParseInt(values[51]),
					min9 = ParseInt(values[52]),
					max9 = ParseInt(values[53]),
					aprop1a = values[54],
					apar1a = ParseInt(values[55]),
					amin1a = ParseInt(values[56]),
					amax1a = ParseInt(values[57]),
					aprop1b = values[58],
					apar1b = ParseInt(values[59]),
					amin1b = ParseInt(values[60]),
					amax1b = ParseInt(values[61]),
					aprop2a = values[62],
					apar2a = ParseInt(values[63]),
					amin2a = ParseInt(values[64]),
					amax2a = ParseInt(values[65]),
					aprop2b = values[66],
					apar2b = ParseInt(values[67]),
					amin2b = ParseInt(values[68]),
					amax2b = ParseInt(values[69]),
					aprop3a = values[70],
					apar3a = ParseInt(values[71]),
					amin3a = ParseInt(values[72]),
					amax3a = ParseInt(values[73]),
					aprop3b = values[74],
					apar3b = ParseInt(values[75]),
					amin3b = ParseInt(values[76]),
					amax3b = ParseInt(values[77]),
					aprop4a = values[78],
					apar4a = ParseInt(values[79]),
					amin4a = ParseInt(values[80]),
					amax4a = ParseInt(values[81]),
					aprop4b = values[82],
					apar4b = ParseInt(values[83]),
					amin4b = ParseInt(values[84]),
					amax4b = ParseInt(values[85]),
					aprop5a = values[86],
					apar5a = ParseInt(values[87]),
					amin5a = ParseInt(values[88]),
					amax5a = ParseInt(values[89]),
					aprop5b = values[90],
					apar5b = ParseInt(values[91]),
					amin5b = ParseInt(values[92]),
					amax5b = ParseInt(values[93]),
					diablocloneweight = ParseInt(values[94])
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
		public int? diablocloneweight { get; set; }
	}

	public class Misc
	{
		public string? name { get; set; }
		public bool? compactsave { get; set; }
		public int? version { get; set; }
		public int? level { get; set; }
		public int? ShowLevel { get; set; }
		public int? levelreq { get; set; }
		public int? reqstr { get; set; }
		public int? reqdex { get; set; }
		public int? rarity { get; set; }
		public bool? spawnable { get; set; }
		public int? speed { get; set; }
		public int? nodurability { get; set; }
		public int? cost { get; set; }
		/// <summary>
		/// Defines the gambling gold cost of the item on the Gambling UI. Only functions for rings and amulets.
		/// </summary>
		public int? gamble_cost { get; set; }
		public string? code { get; set; }
		public string? alternategfx { get; set; }
		public string? namestr { get; set; }
		public int? component { get; set; }
		public int? invwidth { get; set; }
		public int? invheight { get; set; }
		public int? hasinv { get; set; }
		public int? gemsockets { get; set; }
		public int? gemapplytype { get; set; }
		public string? flippyfile { get; set; }
		public string? invfile { get; set; }
		public string? uniqueinvfile { get; set; }
		public bool? Transmogrify { get; set; }
		public string? TMogType { get; set; }
		public int? TMogMin { get; set; }
		public int? TMogMax { get; set; }
		public bool? useable { get; set; }
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
		public bool? autobelt { get; set; }
		public bool? stackable { get; set; }
		public int? minstack { get; set; }
		public int? maxstack { get; set; }
		public int? spawnstack { get; set; }
		public string? quest { get; set; }
		public string? questdiffcheck { get; set; }
		public bool? missiletype { get; set; }
		public int? spellicon { get; set; }
		public int? pSpell { get; set; }
		public string? state { get; set; }
		public string? cstate1 { get; set; }
		public string? cstate2 { get; set; }
		public int? len { get; set; }
		public string? stat1 { get; set; }
		public int? calc1 { get; set; }
		public string? stat2 { get; set; }
		public int? calc2 { get; set; }
		public string? stat3 { get; set; }
		public int? calc3 { get; set; }
		public int? spelldesc { get; set; }
		public string? spelldescstr { get; set; }
		public string? spelldescstr2 { get; set; }
		public int? spelldesccalc { get; set; }
		public int? spelldesccolor { get; set; }
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
		public int? diablocloneweight { get; set; }
	}

	public class Weapon
	{
		public string? name { get; set; }
		public string? type { get; set; }
		public string? type2 { get; set; }
		public string? code { get; set; }
		public string? alternategfx { get; set; }
		public string? namestr { get; set; }
		public int? version { get; set; }
		public bool? compactsave { get; set; }
		public int? rarity { get; set; }
		public bool? spawnable { get; set; }
		public bool? Transmogrify { get; set; }
		public string? TMogType { get; set; }
		public int? TMogMin { get; set; }
		public int? TMogMax { get; set; }
		public int? mindam { get; set; }
		public int? maxdam { get; set; }
		public bool? one_or_two_handed { get; set; }
		public bool? two_handed { get; set; }

		public int? two_handed_min_dam { get; set; }
		public int? two_handed_max_dam { get; set; }
		public int? min_mis_dam { get; set; }
		public int? max_mis_dam { get; set; }
		public int? rangeadder { get; set; }
		public int? speed { get; set; }
		public int? StrBonus { get; set; }
		public int? DexBonus { get; set; }

		public int? reqstr { get; set; }
		public int? reqdex { get; set; }
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
		public int? magic_lvl { get; set; }
		public int? auto_prefix { get; set; }
		public string? normcode { get; set; }
		public string? ubercode { get; set; }
		public string? ultracode { get; set; }
		public string? wclass { get; set; }
		public string? two_handed_class { get; set; }
		public int? component { get; set; }
		public string? hitclass { get; set; }
		public int? invwidth { get; set; }
		public int? invheight { get; set; }
		public bool? stackable { get; set; }
		public int? minstack { get; set; }
		public int? maxstack { get; set; }
		public int? spawnstack { get; set; }
		public string? flippyfile { get; set; }
		public string? invfile { get; set; }
		public string? uniqueinvfile { get; set; }
		public string? setinvfile { get; set; }
		public int? hasinv { get; set; }
		public int? gemsockets { get; set; }
		public int? gemapplytype { get; set; }
		public string? comment { get; set; }
		public bool? useable { get; set; }
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
		public int? diablocloneweight { get; set; }
	}

	public class Unique
	{
		public int? ID { get; set; }
		public int? version { get; set; }
		public bool? enabled { get; set; }
		public int? firstLadderSeason { get; set; }
		public int? lastLadderSeason { get; set; }
		public int? rarity { get; set; }
		public string? nolimit { get; set; }
		public int? lvl { get; set; }
		public int? lvl_req { get; set; }
		public string? code { get; set; }
		public string? ItemName { get; set; }
		public int? carry1 { get; set; }
		public int? cost_mult { get; set; }
		public int? cost_add { get; set; }
		public string? chrtransform { get; set; }
		public string? invtransform { get; set; }
		public string? flippyfile { get; set; }
		public string? invfile { get; set; }
		public string? dropsound { get; set; }
		public int? dropsfxframe { get; set; }
		public string? usesound { get; set; }
		public string? prop1 { get; set; }
		public int? par1 { get; set; }
		public int? min1 { get; set; }
		public int? max1 { get; set; }
		public string? prop2 { get; set; }
		public string? par2 { get; set; }
		public int? min2 { get; set; }
		public int? max2 { get; set; }
		public string? prop3 { get; set; }
		public string? par3 { get; set; }
		public int? min3 { get; set; }
		public int? max3 { get; set; }
		public string? prop4 { get; set; }
		public string? par4 { get; set; }
		public int? min4 { get; set; }
		public int? max4 { get; set; }
		public string? prop5 { get; set; }
		public string? par5 { get; set; }
		public int? min5 { get; set; }
		public int? max5 { get; set; }
		public string? prop6 { get; set; }
		public string? par6 { get; set; }
		public int? min6 { get; set; }
		public int? max6 { get; set; }
		public string? prop7 { get; set; }
		public string? par7 { get; set; }
		public int? min7 { get; set; }
		public int? max7 { get; set; }
		public string? prop8 { get; set; }
		public string? par8 { get; set; }
		public int? min8 { get; set; }
		public int? max8 { get; set; }
		public string? prop9 { get; set; }
		public string? par9 { get; set; }
		public int? min9 { get; set; }
		public int? max9 { get; set; }
		public string? prop10 { get; set; }
		public string? par10 { get; set; }
		public int? min10 { get; set; }
		public int? max10 { get; set; }
		public string? prop11 { get; set; }
		public string? par11 { get; set; }
		public int? min11 { get; set; }
		public int? max11 { get; set; }
		public string? prop12 { get; set; }
		public int? par12 { get; set; }
		public int? min12 { get; set; }
		public int? max12 { get; set; }
		public int? diablocloneweight { get; set; }
	}

	public class SetItem
	{
		public string? index { get; set; }
		public string? ID { get; set; }
		public string? set { get; set; }
		public string? item { get; set; }
		public string? ItemName { get; set; }
		public int? rarity { get; set; }
		public int? lvl { get; set; }
		public int? lvl_req { get; set; }
		public string? chrtransform { get; set; }
		public string? invtransform { get; set; }
		public string? invfile { get; set; }
		public string? flippyfile { get; set; }
		public string? dropsound { get; set; }
		public string? dropsfxframe { get; set; }
		public string? usesound { get; set; }
		public int? cost_mult { get; set; }
		public int? cost_add { get; set; }
		public int? add_func { get; set; }
		public string? prop1 { get; set; }
		public int? par1 { get; set; }
		public int? min1 { get; set; }
		public int? max1 { get; set; }
		public string? prop2 { get; set; }
		public int? par2 { get; set; }
		public int? min2 { get; set; }
		public int? max2 { get; set; }
		public string? prop3 { get; set; }
		public int? par3 { get; set; }
		public int? min3 { get; set; }
		public int? max3 { get; set; }
		public string? prop4 { get; set; }
		public int? par4 { get; set; }
		public int? min4 { get; set; }
		public int? max4 { get; set; }
		public string? prop5 { get; set; }
		public int? par5 { get; set; }
		public int? min5 { get; set; }
		public int? max5 { get; set; }
		public string? prop6 { get; set; }
		public int? par6 { get; set; }
		public int? min6 { get; set; }
		public int? max6 { get; set; }
		public string? prop7 { get; set; }
		public int? par7 { get; set; }
		public int? min7 { get; set; }
		public int? max7 { get; set; }
		public string? prop8 { get; set; }
		public int? par8 { get; set; }
		public int? min8 { get; set; }
		public int? max8 { get; set; }
		public string? prop9 { get; set; }
		public int? par9 { get; set; }
		public int? min9 { get; set; }
		public int? max9 { get; set; }
		public string? aprop1a { get; set; }
		public int? apar1a { get; set; }
		public int? amin1a { get; set; }
		public int? amax1a { get; set; }
		public string? aprop1b { get; set; }
		public int? apar1b { get; set; }
		public int? amin1b { get; set; }
		public int? amax1b { get; set; }
		public string? aprop2a { get; set; }
		public int? apar2a { get; set; }
		public int? amin2a { get; set; }
		public int? amax2a { get; set; }
		public string? aprop2b { get; set; }
		public int? apar2b { get; set; }
		public int? amin2b { get; set; }
		public int? amax2b { get; set; }
		public string? aprop3a { get; set; }
		public int? apar3a { get; set; }
		public int? amin3a { get; set; }
		public int? amax3a { get; set; }
		public string? aprop3b { get; set; }
		public int? apar3b { get; set; }
		public int? amin3b { get; set; }
		public int? amax3b { get; set; }
		public string? aprop4a { get; set; }
		public int? apar4a { get; set; }
		public int? amin4a { get; set; }
		public int? amax4a { get; set; }
		public string? aprop4b { get; set; }
		public int? apar4b { get; set; }
		public int? amin4b { get; set; }
		public int? amax4b { get; set; }
		public string? aprop5a { get; set; }
		public int? apar5a { get; set; }
		public int? amin5a { get; set; }
		public int? amax5a { get; set; }
		public string? aprop5b { get; set; }
		public int? apar5b { get; set; }
		public int? amin5b { get; set; }
		public int? amax5b { get; set; }
		public int? diablocloneweight { get; set; }
	}
}
