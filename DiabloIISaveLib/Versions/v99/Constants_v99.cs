using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DiabloIISaveLib.Huffman;
using Serilog;
using static DiabloIISaveLib.Helpers;

namespace DiabloIISaveLib
{
	public static class Constants
	{
		public enum EDataLoadingStatus
		{
			NotLoaded,
			Loading,
			Loaded
		}

		public static EDataLoadingStatus dataLoadingStatus { get; private set; } = EDataLoadingStatus.NotLoaded;

		public static HuffmanTree? item_code_tree
		{
			get => field ??= HuffmanTree.BuildNew();
			set => field = value;
		}
		public static Dictionary<string, ItemStatCost> item_stat_costs
		{
			get
			{
				if (dataLoadingStatus == EDataLoadingStatus.NotLoaded) ThrowInvalidOperationException("Data was not loaded");
				return field;
			}
			set;
		} = new();
		public static Dictionary<string, ItemType> itemTypes
		{
			get
			{
				if (dataLoadingStatus == EDataLoadingStatus.NotLoaded) ThrowInvalidOperationException("Data was not loaded");
				return field;
			}
			set;
		} = new();
		public static Dictionary<string, Armor> armors
		{
			get
			{
				if (dataLoadingStatus == EDataLoadingStatus.NotLoaded) ThrowInvalidOperationException("Data was not loaded");
				return field;
			}
			set;
		} = new();
		public static Dictionary<string, Misc> miscs
		{
			get
			{
				if (dataLoadingStatus == EDataLoadingStatus.NotLoaded) ThrowInvalidOperationException("Data was not loaded");
				return field;
			}
			set;
		} = new();
		public static Dictionary<string, Weapon> weapons
		{
			get
			{
				if (dataLoadingStatus == EDataLoadingStatus.NotLoaded) ThrowInvalidOperationException("Data was not loaded");
				return field;
			}
			set;
		} = new();
		public static List<Unique> uniques
		{
			get
			{
				if (dataLoadingStatus == EDataLoadingStatus.NotLoaded) ThrowInvalidOperationException("Data was not loaded");
				return field;
			}
			set;
		} = new();
		public static Dictionary<string, SetItem> setItems
		{
			get
			{
				if (dataLoadingStatus == EDataLoadingStatus.NotLoaded) ThrowInvalidOperationException("Data was not loaded");
				return field;
			}
			set;
		} = new();

		static Constants()
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.Debug(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}")
				.WriteTo.File("log.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
				.CreateLogger();
		}

		public static void LoadData(string root)
		{
			if (dataLoadingStatus != EDataLoadingStatus.NotLoaded)
			{
				return;
			}

			if (!Directory.Exists(root))
			{
				DirectoryNotFoundException ex = new($"The provided root directory '{root}' does not exist.");
				Log.Error(ex, $"The provided root directory '{root}' does not exist.");
				throw ex;
			}

			dataLoadingStatus = EDataLoadingStatus.Loading;

			ReadItemStatCosts(root);
			ReadItemTypes(root);
			ReadArmors(root);
			ReadMisc(root);
			ReadWeapons(root);
			ReadUniqueItems(root);
			ReadSetItems(root);

			dataLoadingStatus = EDataLoadingStatus.Loaded;
			Log.Information("Data loaded");
		}

		public static bool IsStackable(string code)
		{
			if (armors.TryGetValue(code, out Armor? armor))
			{
				return armor!.stackable ?? false;
			}
			if (weapons.TryGetValue(code, out Weapon? weapon))
			{
				return weapon!.stackable ?? false;
			}
			if (miscs.TryGetValue(code, out Misc? misc))
			{
				return misc!.stackable ?? false;
			}
			return false;
		}

		public static string? GetItemName(string code)
		{
			if (armors.TryGetValue(code, out Armor? armor))
			{
				return armor?.name;
			}
			if (weapons.TryGetValue(code, out Weapon? weapon))
			{
				return weapon?.name;
			}
			if (miscs.TryGetValue(code, out Misc? misc))
			{
				return misc?.name;
			}

			return null;
		}

		public static void ReadItemStatCosts(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\v99\\excel\\itemstatcost.txt");
			int headerCount = lines[0].Split('\t').Length;

			if (headerCount != 52)
			{
				string msg = $"Item stat cost data table header count was incorrect ({headerCount}). Expected: 52";
				InvalidOperationException ex = new(msg);
				Log.Error(ex, msg);
				throw ex;
			}

			for (int i = 1; i < lines.Length; i++)
			{
				string[] values = lines[i].Split('\t');

				item_stat_costs.Add(values[0], new()
				{
					stat = values[0],
					id = ParseUshort(values[1]),
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

			Log.Information("Loaded {Count} item stat costs", item_stat_costs.Count);
		}

		public static void ReadItemTypes(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\v99\\excel\\itemtypes.txt");
			int headerCount = lines[0].Split('\t').Length;

			if (headerCount != 36)
			{
				string msg = $"Weapon data table header count was incorrect ({headerCount}). Expected: 36";
				InvalidOperationException ex = new(msg);
				Log.Error(ex, msg);
				throw ex;
			}

			for (int i = 1; i < lines.Length; i++)
			{
				string[] values = lines[i].Split('\t');

				if (string.IsNullOrWhiteSpace(values[1])) continue;

				var itemType = new ItemType();

				itemType.Type = values[0];
				itemType.Code = values[1];
				itemType.Equiv1 = values[2];
				itemType.Equiv2 = values[3];
				itemType.Repair = ParseBool(values[4]);
				itemType.Body = ParseBool(values[5]);
				itemType.BodyLoc1 = values[6];
				itemType.BodyLoc2 = values[7];
				itemType.Shoots = values[8];
				itemType.Quiver = values[9];
				itemType.Throwable = ParseBool(values[10]);
				itemType.Reload = ParseBool(values[11]);
				itemType.ReEquip = ParseBool(values[12]);
				itemType.AutoStack = ParseBool(values[13]);
				itemType.Magic = ParseBool(values[14]);
				itemType.Rare = ParseBool(values[15]);
				itemType.Normal = ParseBool(values[16]);
				itemType.Beltable = ParseBool(values[17]);
				itemType.MaxSockets1 = ParseInt(values[18]);
				itemType.MaxSocketsLevelThreshold1 = ParseInt(values[19]);
				itemType.MaxSockets2 = ParseInt(values[20]);
				itemType.MaxSocketsLevelThreshold2 = ParseInt(values[21]);
				itemType.MaxSockets3 = ParseInt(values[22]);
				itemType.TreasureClass = ParseBool(values[23]);
				itemType.Rarity = ParseInt(values[24]);
				itemType.StaffMods = values[25];
				itemType.Class = values[26];
				itemType.VarInvGfx = ParseInt(values[27]);
				itemType.InvGfx1 = values[28];
				itemType.InvGfx2 = values[29];
				itemType.InvGfx3 = values[30];
				itemType.InvGfx4 = values[31];
				itemType.InvGfx5 = values[32];
				itemType.InvGfx6 = values[33];
				itemType.StorePage = values[34];

				itemTypes.Add(values[1], itemType);
			}

			Log.Information("Loaded {Count} item types", itemTypes.Count);
		}

		// Implementation for reading armors.txt
		public static void ReadArmors(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\v99\\excel\\armor.txt");
			int headerCount = lines[0].Split('\t').Length;

			if (headerCount != 166)
			{
				string msg = $"Armor data table header count was incorrect ({headerCount}). Expected: 166";
				InvalidOperationException ex = new(msg);
				Log.Error(ex, msg);
				throw ex;
			}

			for (int i = 1; i < lines.Length; i++)
			{
				var values = lines[i].Split('\t');
				var key = values[18];
				if (string.IsNullOrWhiteSpace(key)) continue;

				var item = new Armor();
				item.name = key;
				item.version = ParseInt(values[1]);
				item.compactsave = ParseBool(values[2]);
				item.rarity = ParseInt(values[3]);
				item.spawnable = ParseBool(values[4]);
				item.minac = ParseInt(values[5]);
				item.maxac = ParseInt(values[6]);
				item.speed = ParseInt(values[7]);
				item.reqstr = ParseInt(values[8]);
				item.reqdex = ParseInt(values[9]);
				item.block = ParseInt(values[10]);
				item.durability = ParseInt(values[11]);
				item.nodurability = ParseInt(values[12]);
				item.level = ParseInt(values[13]);
				item.ShowLevel = ParseInt(values[14]);
				item.levelreq = ParseInt(values[15]);
				item.cost = ParseInt(values[16]);
				item.gamble_cost = ParseInt(values[17]);
				item.code = values[18];
				item.namestr = values[19];
				item.magic_lvl = ParseInt(values[20]);
				item.auto_prefix = ParseInt(values[21]);
				item.alternategfx = values[22];
				item.normcode = values[23];
				item.ubercode = values[24];
				item.ultracode = values[25];
				item.component = ParseInt(values[26]);
				item.invwidth = ParseInt(values[27]);
				item.invheight = ParseInt(values[28]);
				item.hasinv = ParseInt(values[29]);
				item.gemsockets = ParseInt(values[30]);
				item.gemapplytype = ParseInt(values[31]);
				item.flippyfile = values[32];
				item.invfile = values[33];
				item.uniqueinvfile = values[34];
				item.setinvfile = values[35];
				item.rArm = ParseInt(values[36]);
				item.lArm = ParseInt(values[37]);
				item.Torso = ParseInt(values[38]);
				item.Legs = ParseInt(values[39]);
				item.rSPad = ParseInt(values[40]);
				item.lSPad = ParseInt(values[41]);
				item.useable = ParseBool(values[42]);
				item.stackable = ParseBool(values[43]);
				item.minstack = ParseInt(values[44]);
				item.maxstack = ParseInt(values[45]);
				item.spawnstack = ParseInt(values[46]);
				item.Transmogrify = ParseBool(values[47]);
				item.TMogType = values[48];
				item.TMogMin = ParseInt(values[49]);
				item.TMogMax = ParseInt(values[50]);
				item.type = values[51];
				item.type2 = values[52];
				item.dropsound = values[53];
				item.dropsfxframe = ParseInt(values[54]);
				item.usesound = values[55];
				item.unique = ParseBool(values[56]);
				item.transparent = ParseBool(values[57]);
				item.transtbl = ParseInt(values[58]);
				item.quivered = ParseBool(values[59]);
				item.lightradius = ParseInt(values[60]);
				item.belt = ParseInt(values[61]);
				item.quest = values[62];
				item.questdiffcheck = values[63];
				item.missiletype = ParseBool(values[64]);
				item.durwarning = ParseInt(values[65]);
				item.qntwarning = ParseInt(values[66]);
				item.mindam = ParseInt(values[67]);
				item.maxdam = ParseInt(values[68]);
				item.StrBonus = ParseInt(values[69]);
				item.DexBonus = ParseInt(values[70]);
				item.gemoffset = ParseInt(values[71]);
				item.bitfield1 = ParseInt(values[72]);
				item.CharsiMin = ParseInt(values[73]);
				item.CharsiMax = ParseInt(values[74]);
				item.CharsiMagicMin = ParseInt(values[75]);
				item.CharsiMagicMax = ParseInt(values[76]);
				item.CharsiMagicLvl = ParseInt(values[77]);
				item.GheedMin = ParseInt(values[78]);
				item.GheedMax = ParseInt(values[79]);
				item.GheedMagicMin = ParseInt(values[80]);
				item.GheedMagicMax = ParseInt(values[81]);
				item.GheedMagicLvl = ParseInt(values[82]);
				item.AkaraMin = ParseInt(values[83]);
				item.AkaraMax = ParseInt(values[84]);
				item.AkaraMagicMin = ParseInt(values[85]);
				item.AkaraMagicMax = ParseInt(values[86]);
				item.AkaraMagicLvl = ParseInt(values[87]);
				item.FaraMin = ParseInt(values[88]);
				item.FaraMax = ParseInt(values[89]);
				item.FaraMagicMin = ParseInt(values[90]);
				item.FaraMagicMax = ParseInt(values[91]);
				item.FaraMagicLvl = ParseInt(values[92]);
				item.LysanderMin = ParseInt(values[93]);
				item.LysanderMax = ParseInt(values[94]);
				item.LysanderMagicMin = ParseInt(values[95]);
				item.LysanderMagicMax = ParseInt(values[96]);
				item.LysanderMagicLvl = ParseInt(values[97]);
				item.DrognanMin = ParseInt(values[98]);
				item.DrognanMax = ParseInt(values[99]);
				item.DrognanMagicMin = ParseInt(values[100]);
				item.DrognanMagicMax = ParseInt(values[101]);
				item.DrognanMagicLvl = ParseInt(values[102]);
				item.HratliMin = ParseInt(values[103]);
				item.HratliMax = ParseInt(values[104]);
				item.HratliMagicMin = ParseInt(values[105]);
				item.HratliMagicMax = ParseInt(values[106]);
				item.HratliMagicLvl = ParseInt(values[107]);
				item.AlkorMin = ParseInt(values[108]);
				item.AlkorMax = ParseInt(values[109]);
				item.AlkorMagicMin = ParseInt(values[110]);
				item.AlkorMagicMax = ParseInt(values[111]);
				item.AlkorMagicLvl = ParseInt(values[112]);
				item.OrmusMin = ParseInt(values[113]);
				item.OrmusMax = ParseInt(values[114]);
				item.OrmusMagicMin = ParseInt(values[115]);
				item.OrmusMagicMax = ParseInt(values[116]);
				item.OrmusMagicLvl = ParseInt(values[117]);
				item.ElzixMin = ParseInt(values[118]);
				item.ElzixMax = ParseInt(values[119]);
				item.ElzixMagicMin = ParseInt(values[120]);
				item.ElzixMagicMax = ParseInt(values[121]);
				item.ElzixMagicLvl = ParseInt(values[122]);
				item.AshearaMin = ParseInt(values[123]);
				item.AshearaMax = ParseInt(values[124]);
				item.AshearaMagicMin = ParseInt(values[125]);
				item.AshearaMagicMax = ParseInt(values[126]);
				item.AshearaMagicLvl = ParseInt(values[127]);
				item.CainMin = ParseInt(values[128]);
				item.CainMax = ParseInt(values[129]);
				item.CainMagicMin = ParseInt(values[130]);
				item.CainMagicMax = ParseInt(values[131]);
				item.CainMagicLvl = ParseInt(values[132]);
				item.HalbuMin = ParseInt(values[133]);
				item.HalbuMax = ParseInt(values[134]);
				item.HalbuMagicMin = ParseInt(values[135]);
				item.HalbuMagicMax = ParseInt(values[136]);
				item.HalbuMagicLvl = ParseInt(values[137]);
				item.JamellaMin = ParseInt(values[138]);
				item.JamellaMax = ParseInt(values[139]);
				item.JamellaMagicMin = ParseInt(values[140]);
				item.JamellaMagicMax = ParseInt(values[141]);
				item.JamellaMagicLvl = ParseInt(values[142]);
				item.LarzukMin = ParseInt(values[143]);
				item.LarzukMax = ParseInt(values[144]);
				item.LarzukMagicMin = ParseInt(values[145]);
				item.LarzukMagicMax = ParseInt(values[146]);
				item.LarzukMagicLvl = ParseInt(values[147]);
				item.MalahMin = ParseInt(values[148]);
				item.MalahMax = ParseInt(values[149]);
				item.MalahMagicMin = ParseInt(values[150]);
				item.MalahMagicMax = ParseInt(values[151]);
				item.MalahMagicLvl = ParseInt(values[152]);
				item.AnyaMin = ParseInt(values[153]);
				item.AnyaMax = ParseInt(values[154]);
				item.AnyaMagicMin = ParseInt(values[155]);
				item.AnyaMagicMax = ParseInt(values[156]);
				item.AnyaMagicLvl = ParseInt(values[157]);
				item.Transform = ParseInt(values[158]);
				item.InvTrans = ParseInt(values[159]);
				item.SkipName = ParseBool(values[160]);
				item.NightmareUpgrade = values[161];
				item.HellUpgrade = values[162];
				item.Nameable = ParseBool(values[163]);
				item.PermStoreItem = ParseBool(values[164]);
				item.diablocloneweight = ParseInt(values[165]);

				armors.Add(key, item);
			}

			Log.Information("Loaded {Count} armors", armors.Count);
		}

		// Implementation for reading weapons.txt
		public static void ReadWeapons(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\v99\\excel\\weapons.txt");
			int headerCount = lines[0].Split('\t').Length;

			if (headerCount != 168)
			{
				string msg = $"Weapon data table header count was incorrect ({headerCount}). Expected: 168";
				InvalidOperationException ex = new(msg);
				Log.Error(ex, msg);
				throw ex;
			}

			for (int i = 1; i < lines.Length; i++)
			{
				var values = lines[i].Split('\t');
				var key = values[3];
				if (string.IsNullOrWhiteSpace(key)) continue;

				var item = new Weapon();
				item.name = key;
				item.type = values[1];
				item.type2 = values[2];
				item.code = values[3];
				item.alternategfx = values[4];
				item.namestr = values[5];
				item.version = ParseInt(values[6]);
				item.compactsave = ParseBool(values[7]);
				item.rarity = ParseInt(values[8]);
				item.spawnable = ParseBool(values[9]);
				item.Transmogrify = ParseBool(values[10]);
				item.TMogType = values[11];
				item.TMogMin = ParseInt(values[12]);
				item.TMogMax = ParseInt(values[13]);
				item.mindam = ParseInt(values[14]);
				item.maxdam = ParseInt(values[15]);
				item.one_or_two_handed = ParseBool(values[16]);
				item.two_handed = ParseBool(values[17]);
				item.two_handed_min_dam = ParseInt(values[18]);
				item.two_handed_max_dam = ParseInt(values[19]);
				item.min_mis_dam = ParseInt(values[20]);
				item.max_mis_dam = ParseInt(values[21]);
				item.rangeadder = ParseInt(values[22]);
				item.speed = ParseInt(values[23]);
				item.StrBonus = ParseInt(values[24]);
				item.DexBonus = ParseInt(values[25]);
				item.reqstr = ParseInt(values[26]);
				item.reqdex = ParseInt(values[27]);
				item.durability = ParseInt(values[28]);
				item.nodurability = ParseInt(values[29]);
				item.level = ParseInt(values[30]);
				item.ShowLevel = ParseInt(values[31]);
				item.levelreq = ParseInt(values[32]);
				item.cost = ParseInt(values[33]);
				item.gamble_cost = ParseInt(values[34]);
				item.magic_lvl = ParseInt(values[35]);
				item.auto_prefix = ParseInt(values[36]);
				item.normcode = values[37];
				item.ubercode = values[38];
				item.ultracode = values[39];
				item.wclass = values[40];
				item.two_handed_class = values[41];
				item.component = ParseInt(values[42]);
				item.hitclass = values[43];
				item.invwidth = ParseInt(values[44]);
				item.invheight = ParseInt(values[45]);
				item.stackable = ParseBool(values[46]);
				item.minstack = ParseInt(values[47]);
				item.maxstack = ParseInt(values[48]);
				item.spawnstack = ParseInt(values[49]);
				item.flippyfile = values[50];
				item.invfile = values[51];
				item.uniqueinvfile = values[52];
				item.setinvfile = values[53];
				item.hasinv = ParseInt(values[54]);
				item.gemsockets = ParseInt(values[55]);
				item.gemapplytype = ParseInt(values[56]);
				item.comment = values[57];
				item.useable = ParseBool(values[58]);
				item.dropsound = values[59];
				item.dropsfxframe = ParseInt(values[60]);
				item.usesound = values[61];
				item.unique = ParseBool(values[62]);
				item.transparent = ParseBool(values[63]);
				item.transtbl = ParseInt(values[64]);
				item.quivered = ParseBool(values[65]);
				item.lightradius = ParseInt(values[66]);
				item.belt = ParseInt(values[67]);
				item.quest = values[68];
				item.questdiffcheck = values[69];
				item.missiletype = ParseBool(values[70]);
				item.durwarning = ParseInt(values[71]);
				item.qntwarning = ParseInt(values[72]);
				item.gemoffset = ParseInt(values[73]);
				item.bitfield1 = ParseInt(values[74]);
				item.CharsiMin = ParseInt(values[75]);
				item.CharsiMax = ParseInt(values[76]);
				item.CharsiMagicMin = ParseInt(values[77]);
				item.CharsiMagicMax = ParseInt(values[78]);
				item.CharsiMagicLvl = ParseInt(values[79]);
				item.GheedMin = ParseInt(values[80]);
				item.GheedMax = ParseInt(values[81]);
				item.GheedMagicMin = ParseInt(values[82]);
				item.GheedMagicMax = ParseInt(values[83]);
				item.GheedMagicLvl = ParseInt(values[84]);
				item.AkaraMin = ParseInt(values[85]);
				item.AkaraMax = ParseInt(values[86]);
				item.AkaraMagicMin = ParseInt(values[87]);
				item.AkaraMagicMax = ParseInt(values[88]);
				item.AkaraMagicLvl = ParseInt(values[89]);
				item.FaraMin = ParseInt(values[90]);
				item.FaraMax = ParseInt(values[91]);
				item.FaraMagicMin = ParseInt(values[92]);
				item.FaraMagicMax = ParseInt(values[93]);
				item.FaraMagicLvl = ParseInt(values[94]);
				item.LysanderMin = ParseInt(values[95]);
				item.LysanderMax = ParseInt(values[96]);
				item.LysanderMagicMin = ParseInt(values[97]);
				item.LysanderMagicMax = ParseInt(values[98]);
				item.LysanderMagicLvl = ParseInt(values[99]);
				item.DrognanMin = ParseInt(values[100]);
				item.DrognanMax = ParseInt(values[101]);
				item.DrognanMagicMin = ParseInt(values[102]);
				item.DrognanMagicMax = ParseInt(values[103]);
				item.DrognanMagicLvl = ParseInt(values[104]);
				item.HratliMin = ParseInt(values[105]);
				item.HratliMax = ParseInt(values[106]);
				item.HratliMagicMin = ParseInt(values[107]);
				item.HratliMagicMax = ParseInt(values[108]);
				item.HratliMagicLvl = ParseInt(values[109]);
				item.AlkorMin = ParseInt(values[110]);
				item.AlkorMax = ParseInt(values[111]);
				item.AlkorMagicMin = ParseInt(values[112]);
				item.AlkorMagicMax = ParseInt(values[113]);
				item.AlkorMagicLvl = ParseInt(values[114]);
				item.OrmusMin = ParseInt(values[115]);
				item.OrmusMax = ParseInt(values[116]);
				item.OrmusMagicMin = ParseInt(values[117]);
				item.OrmusMagicMax = ParseInt(values[118]);
				item.OrmusMagicLvl = ParseInt(values[119]);
				item.ElzixMin = ParseInt(values[120]);
				item.ElzixMax = ParseInt(values[121]);
				item.ElzixMagicMin = ParseInt(values[122]);
				item.ElzixMagicMax = ParseInt(values[123]);
				item.ElzixMagicLvl = ParseInt(values[124]);
				item.AshearaMin = ParseInt(values[125]);
				item.AshearaMax = ParseInt(values[126]);
				item.AshearaMagicMin = ParseInt(values[127]);
				item.AshearaMagicMax = ParseInt(values[128]);
				item.AshearaMagicLvl = ParseInt(values[129]);
				item.CainMin = ParseInt(values[130]);
				item.CainMax = ParseInt(values[131]);
				item.CainMagicMin = ParseInt(values[132]);
				item.CainMagicMax = ParseInt(values[133]);
				item.CainMagicLvl = ParseInt(values[134]);
				item.HalbuMin = ParseInt(values[135]);
				item.HalbuMax = ParseInt(values[136]);
				item.HalbuMagicMin = ParseInt(values[137]);
				item.HalbuMagicMax = ParseInt(values[138]);
				item.HalbuMagicLvl = ParseInt(values[139]);
				item.JamellaMin = ParseInt(values[140]);
				item.JamellaMax = ParseInt(values[141]);
				item.JamellaMagicMin = ParseInt(values[142]);
				item.JamellaMagicMax = ParseInt(values[143]);
				item.JamellaMagicLvl = ParseInt(values[144]);
				item.LarzukMin = ParseInt(values[145]);
				item.LarzukMax = ParseInt(values[146]);
				item.LarzukMagicMin = ParseInt(values[147]);
				item.LarzukMagicMax = ParseInt(values[148]);
				item.LarzukMagicLvl = ParseInt(values[149]);
				item.MalahMin = ParseInt(values[150]);
				item.MalahMax = ParseInt(values[151]);
				item.MalahMagicMin = ParseInt(values[152]);
				item.MalahMagicMax = ParseInt(values[153]);
				item.MalahMagicLvl = ParseInt(values[154]);
				item.AnyaMin = ParseInt(values[155]);
				item.AnyaMax = ParseInt(values[156]);
				item.AnyaMagicMin = ParseInt(values[157]);
				item.AnyaMagicMax = ParseInt(values[158]);
				item.AnyaMagicLvl = ParseInt(values[159]);
				item.Transform = ParseInt(values[160]);
				item.InvTrans = ParseInt(values[161]);
				item.SkipName = ParseBool(values[162]);
				item.NightmareUpgrade = values[163];
				item.HellUpgrade = values[164];
				item.Nameable = ParseBool(values[165]);
				item.PermStoreItem = ParseBool(values[166]);
				item.diablocloneweight = ParseInt(values[167]);

				weapons.Add(key, item);
			}

			Log.Information("Loaded {Count} weapons", weapons.Count);
		}

		// Implementation for reading misc.txt — made similar to ReadWeapons (uses code field as key)
		public static void ReadMisc(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\v99\\excel\\misc.txt");
			int headerCount = lines[0].Split('\t').Length;

			if (headerCount != 167)
			{
				string msg = $"Weapon data table header count was incorrect ({headerCount}). Expected: 167";
				InvalidOperationException ex = new(msg);
				Log.Error(ex, msg);
				throw ex;
			}

			for (int i = 1; i < lines.Length; i++)
			{
				var values = lines[i].Split('\t');

				var code = values[14];
				if (string.IsNullOrWhiteSpace(code)) continue;

				var item = new Misc();
				item.name = values[0];
				item.compactsave = ParseBool(values[1]);
				item.version = ParseInt(values[2]);
				item.level = ParseInt(values[3]);
				item.ShowLevel = ParseInt(values[4]);
				item.levelreq = ParseInt(values[5]);
				item.reqstr = ParseInt(values[6]);
				item.reqdex = ParseInt(values[7]);
				item.rarity = ParseInt(values[8]);
				item.spawnable = ParseBool(values[9]);
				item.speed = ParseInt(values[10]);
				item.nodurability = ParseInt(values[11]);
				item.cost = ParseInt(values[12]);
				item.gamble_cost = ParseInt(values[13]);
				item.code = values[14];
				item.alternategfx = values[15];
				item.namestr = values[16];
				item.component = ParseInt(values[17]);
				item.invwidth = ParseInt(values[18]);
				item.invheight = ParseInt(values[19]);
				item.hasinv = ParseInt(values[20]);
				item.gemsockets = ParseInt(values[21]);
				item.gemapplytype = ParseInt(values[22]);
				item.flippyfile = values[23];
				item.invfile = values[24];
				item.uniqueinvfile = values[25];
				item.Transmogrify = ParseBool(values[26]);
				item.TMogType = values[27];
				item.TMogMin = ParseInt(values[28]);
				item.TMogMax = ParseInt(values[29]);
				item.useable = ParseBool(values[30]);
				item.type = values[31];
				item.type2 = values[32];
				item.dropsound = values[33];
				item.dropsfxframe = ParseInt(values[34]);
				item.usesound = values[35];
				item.unique = ParseBool(values[36]);
				item.transparent = ParseBool(values[37]);
				item.transtbl = ParseInt(values[38]);
				item.lightradius = ParseInt(values[39]);
				item.belt = ParseInt(values[40]);
				item.autobelt = ParseBool(values[41]);
				item.stackable = ParseBool(values[42]);
				item.minstack = ParseInt(values[43]);
				item.maxstack = ParseInt(values[44]);
				item.spawnstack = ParseInt(values[45]);
				item.quest = values[46];
				item.questdiffcheck = values[47];
				item.missiletype = ParseBool(values[48]);
				item.spellicon = ParseInt(values[49]);
				item.pSpell = ParseInt(values[50]);
				item.state = values[51];
				item.cstate1 = values[52];
				item.cstate2 = values[53];
				item.len = ParseInt(values[54]);
				item.stat1 = values[55];
				item.calc1 = ParseInt(values[56]);
				item.stat2 = values[57];
				item.calc2 = ParseInt(values[58]);
				item.stat3 = values[59];
				item.calc3 = ParseInt(values[60]);
				item.spelldesc = ParseInt(values[61]);
				item.spelldescstr = values[62];
				item.spelldescstr2 = values[63];
				item.spelldesccalc = ParseInt(values[64]);
				item.spelldesccolor = ParseInt(values[65]);
				item.durwarning = ParseInt(values[66]);
				item.qntwarning = ParseInt(values[67]);
				item.gemoffset = ParseInt(values[68]);
				item.BetterGem = values[69];
				item.bitfield1 = ParseInt(values[70]);
				item.CharsiMin = ParseInt(values[71]);
				item.CharsiMax = ParseInt(values[72]);
				item.CharsiMagicMin = ParseInt(values[73]);
				item.CharsiMagicMax = ParseInt(values[74]);
				item.CharsiMagicLvl = ParseInt(values[75]);
				item.GheedMin = ParseInt(values[76]);
				item.GheedMax = ParseInt(values[77]);
				item.GheedMagicMin = ParseInt(values[78]);
				item.GheedMagicMax = ParseInt(values[79]);
				item.GheedMagicLvl = ParseInt(values[80]);
				item.AkaraMin = ParseInt(values[81]);
				item.AkaraMax = ParseInt(values[82]);
				item.AkaraMagicMin = ParseInt(values[83]);
				item.AkaraMagicMax = ParseInt(values[84]);
				item.AkaraMagicLvl = ParseInt(values[85]);
				item.FaraMin = ParseInt(values[86]);
				item.FaraMax = ParseInt(values[87]);
				item.FaraMagicMin = ParseInt(values[88]);
				item.FaraMagicMax = ParseInt(values[89]);
				item.FaraMagicLvl = ParseInt(values[90]);
				item.LysanderMin = ParseInt(values[91]);
				item.LysanderMax = ParseInt(values[92]);
				item.LysanderMagicMin = ParseInt(values[93]);
				item.LysanderMagicMax = ParseInt(values[94]);
				item.LysanderMagicLvl = ParseInt(values[95]);
				item.DrognanMin = ParseInt(values[96]);
				item.DrognanMax = ParseInt(values[97]);
				item.DrognanMagicMin = ParseInt(values[98]);
				item.DrognanMagicMax = ParseInt(values[99]);
				item.DrognanMagicLvl = ParseInt(values[100]);
				item.HratliMin = ParseInt(values[101]);
				item.HratliMax = ParseInt(values[102]);
				item.HratliMagicMin = ParseInt(values[103]);
				item.HratliMagicMax = ParseInt(values[104]);
				item.HratliMagicLvl = ParseInt(values[105]);
				item.AlkorMin = ParseInt(values[106]);
				item.AlkorMax = ParseInt(values[107]);
				item.AlkorMagicMin = ParseInt(values[108]);
				item.AlkorMagicMax = ParseInt(values[109]);
				item.AlkorMagicLvl = ParseInt(values[110]);
				item.OrmusMin = ParseInt(values[111]);
				item.OrmusMax = ParseInt(values[112]);
				item.OrmusMagicMin = ParseInt(values[113]);
				item.OrmusMagicMax = ParseInt(values[114]);
				item.OrmusMagicLvl = ParseInt(values[115]);
				item.ElzixMin = ParseInt(values[116]);
				item.ElzixMax = ParseInt(values[117]);
				item.ElzixMagicMin = ParseInt(values[118]);
				item.ElzixMagicMax = ParseInt(values[119]);
				item.ElzixMagicLvl = ParseInt(values[120]);
				item.AshearaMin = ParseInt(values[121]);
				item.AshearaMax = ParseInt(values[122]);
				item.AshearaMagicMin = ParseInt(values[123]);
				item.AshearaMagicMax = ParseInt(values[124]);
				item.AshearaMagicLvl = ParseInt(values[125]);
				item.CainMin = ParseInt(values[126]);
				item.CainMax = ParseInt(values[127]);
				item.CainMagicMin = ParseInt(values[128]);
				item.CainMagicMax = ParseInt(values[129]);
				item.CainMagicLvl = ParseInt(values[130]);
				item.HalbuMin = ParseInt(values[131]);
				item.HalbuMax = ParseInt(values[132]);
				item.HalbuMagicMin = ParseInt(values[133]);
				item.HalbuMagicMax = ParseInt(values[134]);
				item.HalbuMagicLvl = ParseInt(values[135]);
				item.JamellaMin = ParseInt(values[136]);
				item.JamellaMax = ParseInt(values[137]);
				item.JamellaMagicMin = ParseInt(values[138]);
				item.JamellaMagicMax = ParseInt(values[139]);
				item.JamellaMagicLvl = ParseInt(values[140]);
				item.LarzukMin = ParseInt(values[141]);
				item.LarzukMax = ParseInt(values[142]);
				item.LarzukMagicMin = ParseInt(values[143]);
				item.LarzukMagicMax = ParseInt(values[144]);
				item.LarzukMagicLvl = ParseInt(values[145]);
				item.MalahMin = ParseInt(values[146]);
				item.MalahMax = ParseInt(values[147]);
				item.MalahMagicMin = ParseInt(values[148]);
				item.MalahMagicMax = ParseInt(values[149]);
				item.MalahMagicLvl = ParseInt(values[150]);
				item.AnyaMin = ParseInt(values[151]);
				item.AnyaMax = ParseInt(values[152]);
				item.AnyaMagicMin = ParseInt(values[153]);
				item.AnyaMagicMax = ParseInt(values[154]);
				item.AnyaMagicLvl = ParseInt(values[155]);
				item.Transform = ParseInt(values[156]);
				item.InvTrans = ParseInt(values[157]);
				item.SkipName = ParseBool(values[158]);
				item.NightmareUpgrade = values[159];
				item.HellUpgrade = values[160];
				item.mindam = ParseInt(values[161]);
				item.maxdam = ParseInt(values[162]);
				item.PermStoreItem = ParseBool(values[163]);
				item.multibuy = ParseBool(values[164]);
				item.Nameable = ParseBool(values[165]);
				item.diablocloneweight = ParseInt(values[166]);

				miscs.Add(code, item);
			}

			Log.Information("Loaded {Count} miscs", miscs.Count);
		}

		// Implementation for reading uniqueitems.txt (matches the updated Unique class; no bounds checks)
		public static void ReadUniqueItems(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\v99\\excel\\uniqueitems.txt");
			int headerCount = lines[0].Split('\t').Length;

			if (headerCount != 72)
			{
				string msg = $"Unique item data table header count was incorrect ({headerCount}). Expected: 72";
				InvalidOperationException ex = new(msg);
				Log.Error(ex, msg);
				throw ex;
			}

			for (int i = 1; i < lines.Length; i++)
			{
				var values = lines[i].Split('\t');
				var code = values[10];
				if (string.IsNullOrWhiteSpace(code)) continue;

				var item = new Unique();

				item.index = values[0];
				item.ID = ParseInt(values[1]);
				item.version = ParseInt(values[2]);
				item.enabled = ParseBool(values[3]);
				item.firstLadderSeason = ParseInt(values[4]);
				item.lastLadderSeason = ParseInt(values[5]);
				item.rarity = ParseInt(values[6]);
				item.nolimit = values[7];
				item.lvl = ParseInt(values[8]);
				item.lvl_req = ParseInt(values[9]);
				item.code = values[10];
				item.ItemName = values[11];
				item.carry1 = ParseInt(values[12]);
				item.cost_mult = ParseInt(values[13]);
				item.cost_add = ParseInt(values[14]);
				item.chrtransform = values[15];
				item.invtransform = values[16];
				item.flippyfile = values[17];
				item.invfile = values[18];
				item.dropsound = values[19];
				item.dropsfxframe = ParseInt(values[20]);
				item.usesound = values[21];

				item.prop1 = values[22];
				item.par1 = ParseInt(values[23]);
				item.min1 = ParseInt(values[24]);
				item.max1 = ParseInt(values[25]);

				item.prop2 = values[26];
				item.par2 = values[27];
				item.min2 = ParseInt(values[28]);
				item.max2 = ParseInt(values[29]);

				item.prop3 = values[30];
				item.par3 = values[31];
				item.min3 = ParseInt(values[32]);
				item.max3 = ParseInt(values[33]);

				item.prop4 = values[34];
				item.par4 = values[35];
				item.min4 = ParseInt(values[36]);
				item.max4 = ParseInt(values[37]);

				item.prop5 = values[38];
				item.par5 = values[39];
				item.min5 = ParseInt(values[40]);
				item.max5 = ParseInt(values[41]);

				item.prop6 = values[42];
				item.par6 = values[43];
				item.min6 = ParseInt(values[44]);
				item.max6 = ParseInt(values[45]);

				item.prop7 = values[46];
				item.par7 = values[47];
				item.min7 = ParseInt(values[48]);
				item.max7 = ParseInt(values[49]);

				item.prop8 = values[50];
				item.par8 = values[51];
				item.min8 = ParseInt(values[52]);
				item.max8 = ParseInt(values[53]);

				item.prop9 = values[54];
				item.par9 = values[55];
				item.min9 = ParseInt(values[56]);
				item.max9 = ParseInt(values[57]);

				item.prop10 = values[58];
				item.par10 = values[59];
				item.min10 = ParseInt(values[60]);
				item.max10 = ParseInt(values[61]);

				item.prop11 = values[62];
				item.par11 = values[63];
				item.min11 = ParseInt(values[64]);
				item.max11 = ParseInt(values[65]);

				item.prop12 = values[66];
				item.par12 = ParseInt(values[67]);
				item.min12 = ParseInt(values[68]);
				item.max12 = ParseInt(values[69]);

				item.diablocloneweight = ParseInt(values[70]);

				uniques.Add(item);
			}

			Log.Information("Loaded {Count} uniques", uniques.Count);
		}

		// Implementation for reading sets.txt
		public static void ReadSetItems(string root)
		{
			string[] lines = File.ReadAllLines(root + "\\data\\global\\v99\\excel\\setitems.txt");
			int headerCount = lines[0].Split('\t').Length;

			if (headerCount != 96)
			{
				string msg = $"Weapon data table header count was incorrect ({headerCount}). Expected: 96";
				InvalidOperationException ex = new(msg);
				Log.Error(ex, msg);
				throw ex;
			}

			for (int i = 1; i < lines.Length; i++)
			{
				var values = lines[i].Split('\t');
				var key = values[0];
				if (string.IsNullOrWhiteSpace(key)) continue;

				var item = new SetItem();
				item.index = key;
				item.ID = values[1];
				item.set = values[2];
				item.item = values[3];
				item.ItemName = values[4];
				item.rarity = ParseInt(values[5]);
				item.lvl = ParseInt(values[6]);
				item.lvl_req = ParseInt(values[7]);
				item.chrtransform = values[8];
				item.invtransform = values[9];
				item.invfile = values[10];
				item.flippyfile = values[11];
				item.dropsound = values[12];
				item.dropsfxframe = values[13];
				item.usesound = values[14];
				item.cost_mult = ParseInt(values[15]);
				item.cost_add = ParseInt(values[16]);
				item.add_func = ParseInt(values[17]);
				item.prop1 = values[18];
				item.par1 = ParseInt(values[19]);
				item.min1 = ParseInt(values[20]);
				item.max1 = ParseInt(values[21]);
				item.prop2 = values[22];
				item.par2 = ParseInt(values[23]);
				item.min2 = ParseInt(values[24]);
				item.max2 = ParseInt(values[25]);
				item.prop3 = values[26];
				item.par3 = ParseInt(values[27]);
				item.min3 = ParseInt(values[28]);
				item.max3 = ParseInt(values[29]);
				item.prop4 = values[30];
				item.par4 = ParseInt(values[31]);
				item.min4 = ParseInt(values[32]);
				item.max4 = ParseInt(values[33]);
				item.prop5 = values[34];
				item.par5 = ParseInt(values[35]);
				item.min5 = ParseInt(values[36]);
				item.max5 = ParseInt(values[37]);
				item.prop6 = values[38];
				item.par6 = ParseInt(values[39]);
				item.min6 = ParseInt(values[40]);
				item.max6 = ParseInt(values[41]);
				item.prop7 = values[42];
				item.par7 = ParseInt(values[43]);
				item.min7 = ParseInt(values[44]);
				item.max7 = ParseInt(values[45]);
				item.prop8 = values[46];
				item.par8 = ParseInt(values[47]);
				item.min8 = ParseInt(values[48]);
				item.max8 = ParseInt(values[49]);
				item.prop9 = values[50];
				item.par9 = ParseInt(values[51]);
				item.min9 = ParseInt(values[52]);
				item.max9 = ParseInt(values[53]);
				item.aprop1a = values[54];
				item.apar1a = ParseInt(values[55]);
				item.amin1a = ParseInt(values[56]);
				item.amax1a = ParseInt(values[57]);
				item.aprop1b = values[58];
				item.apar1b = ParseInt(values[59]);
				item.amin1b = ParseInt(values[60]);
				item.amax1b = ParseInt(values[61]);
				item.aprop2a = values[62];
				item.apar2a = ParseInt(values[63]);
				item.amin2a = ParseInt(values[64]);
				item.amax2a = ParseInt(values[65]);
				item.aprop2b = values[66];
				item.apar2b = ParseInt(values[67]);
				item.amin2b = ParseInt(values[68]);
				item.amax2b = ParseInt(values[69]);
				item.aprop3a = values[70];
				item.apar3a = ParseInt(values[71]);
				item.amin3a = ParseInt(values[72]);
				item.amax3a = ParseInt(values[73]);
				item.aprop3b = values[74];
				item.apar3b = ParseInt(values[75]);
				item.amin3b = ParseInt(values[76]);
				item.amax3b = ParseInt(values[77]);
				item.aprop4a = values[78];
				item.apar4a = ParseInt(values[79]);
				item.amin4a = ParseInt(values[80]);
				item.amax4a = ParseInt(values[81]);
				item.aprop4b = values[82];
				item.apar4b = ParseInt(values[83]);
				item.amin4b = ParseInt(values[84]);
				item.amax4b = ParseInt(values[85]);
				item.aprop5a = values[86];
				item.apar5a = ParseInt(values[87]);
				item.amin5a = ParseInt(values[88]);
				item.amax5a = ParseInt(values[89]);
				item.aprop5b = values[90];
				item.apar5b = ParseInt(values[91]);
				item.amin5b = ParseInt(values[92]);
				item.amax5b = ParseInt(values[93]);
				item.diablocloneweight = ParseInt(values[94]);

				setItems.Add(key, item);
			}

			Log.Information("Loaded {Count} set items", setItems.Count);
		}
	}
	
	public class ItemStatCost
	{
		public string? stat { get; set; }
		public ushort? id { get; set; }
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
		public int? gemoffset { get; set; }
		public string? BetterGem{ get; set; }
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
		public int? mindam { get; set; }
		public int? maxdam { get; set; }
		public bool? PermStoreItem { get; set; }
		public bool? multibuy { get; set; }
		public bool? Nameable { get; set; }
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
		public string? index { get; set; }
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
