using DiabloIISaveLib.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DiabloIISaveLib.Types.Item;

namespace DiabloIISaveLib.Versions
{
	public static class ItemHelpers
	{
		public static int? GetTrueDefenseRating(Item item)
		{
			if (item.defense_rating == null)
				return null;

			int defense = item.defense_rating.Value;

			if (item.modifiers?.Count > 0)
			{
				int enhanced_defence = item.modifiers.SelectMany(_ => _).Where(x => x.stat == "item_armor_percent").Sum(y => y.value);
				int plus_defence = item.modifiers.SelectMany(_ => _).Where(x => x.stat == "armorclass").Sum(y => y.value);

				defense += (int)(defense * (enhanced_defence / 100f));
				defense += plus_defence;
			}

			return defense;
		}

		public static int? GetTrueMaxDurability(Item item)
		{
			if (item.max_durability == null)
				return null;

			int max_durability = item.max_durability.Value;

			if (item.modifiers?.Count > 0)
			{
				int added_max_durability = item.modifiers.SelectMany(_ => _).Where(x => x.stat == "maxdurability").Sum(y => y.value);
				max_durability += added_max_durability;
			}

			return max_durability;
		}
	}
}
