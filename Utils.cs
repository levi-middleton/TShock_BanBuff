using System;
using System.Collections.Generic;

namespace Koishi.BanBuff
{
	public static class Utils
	{
		public static List<string> BuffID;

		static Utils()
		{
            BuffID = new List<string>()
			{
				"",
				"Obsidian Skin",
				"Regeneration",
				"Swiftness",
				"Gills",
				"Ironskin",
				"Mana Regeneration",
				"Magic Power",
				"Featherfall",
				"Spelunker",
				"Invisibility",
				"Shine",
				"Night Owl",
				"Battle",
				"Thorns",
				"Water Walking",
				"Archery",
				"Hunter",
				"Gravitation",
				"Shadow Orb",
				"Poisoned",
				"Potion Sickness",
				"Darkness",
				"Cursed",
				"On Fire!",
				"Tipsy",
				"Well Fed",
				"Fairy",
				"Werewolf",
				"Clairvoyance",
				"Bleeding",
				"Confused",
				"Slow",
				"Weak",
				"Merfolk",
				"Silenced",
				"Broken Armor",
				"Horrified",
				"The Tongue",
				"Cursed Inferno",
				"Pet Bunny",
				"Baby Penguin",
				"Pet Turtle",
				"Paladin's Shield",
				"Frostburn",
				"Baby Eater",
				"Chilled",
				"Frozen",
				"Honey",
				"Pygmies",
				"Baby Skeletron Head",
				"Baby Hornet",
				"Tiki Spirit",
				"Pet Lizard",
				"Pet Parrot",
				"Baby Truffle",
				"Pet Sapling",
				"Wisp",
				"Rapid Healing",
				"Shadow Dodge",
				"Leaf Crystal",
				"Baby Dinosaur",
				"Ice Barrier",
				"Panic!",
				"Baby Slime",
				"Eyeball Spring",
				"Baby Snowman",
				"Burning",
				"Suffocation",
				"Ichor",
				"Venom",
				"Weapon Imbue: Venom",
				"Midas",
				"Weapon Imbue: Cursed Flames",
				"Weapon Imbue: Fire",
				"Weapon Imbue: Gold",
				"Weapon Imbue: Ichor",
				"Weapon Imbue: Nanites",
				"Weapon Imbue: Confetti",
				"Weapon Imbue: Poison",
				"Blackout",
				"Pet Spider",
				"Squashling",
				"Ravens",
				"Black Cat",
				"Cursed Sapling",
				"Water Candle",
				"Cozy Fire",
				"Chaos State",
				"Heart Lamp",
				"Rudolph",
				"Puppy",
				"Baby Grinch",
				"Ammo Box",
				"Mana Sickness",
				"Beetle Endurance",
				"Beetle Endurance",
				"Beetle Endurance",
				"Beetle Might",
				"Beetle Might",
				"Beetle Might",
				"Fairy",
				"Fairy",
				"Wet",
				"Mining",
				"Heartreach",
				"Calm",
				"Builder",
				"Titan",
				"Flipper",
				"Summoning",
				"Dangersense",
				"Ammo Reservation",
				"Lifeforce",
				"Endurance",
				"Rage",
				"Inferno",
				"Wrath",
				"Minecart",
				"Lovestruck",
				"Stinky",
				"Fishing",
				"Sonar",
				"Crate",
				"Warmth",
				"Hornet",
				"Imp",
				"Zephyr Fish",
				"Bunny Mount",
				"Pigron Mount",
				"Slime Mount",
				"Turtle Mount",
				"Bee Mount",
				"Spider",
				"Twins",
				"Pirate",
				"Mini Minotaur",
				"Slime",
				"Minecart",
				"Sharknado",
				"UFO",
				"UFO Mount",
				"Drill Mount",
				"Scutlix Mount",
				"Electrified",
				"Moon Bite",
				"Happy!",
				"Banner",
				"Feral Bite",
				"Webbed",
				"Bewitched",
				"Life Drain",
				"Magic Lantern",
				"Shadowflame",
				"Baby Face Monster",
				"Crimson Heart",
				"Stoned",
				"Peace Candle",
				"Star in a Bottle",
				"Sharpened",
				"Dazed",
				"Deadly Sphere",
				"Unicorn Mount",
				"Obstructed",
				"Distorted",
				"Dryad's Blessing",
				"Minecart",
				"Minecart",
				"Cute Fishron Mount",
				"Penetrated",
				"Solar Blaze",
				"Solar Blaze",
				"Solar Blaze",
				"Life Nebula",
				"Life Nebula",
				"Life Nebula",
				"Mana Nebula",
				"Mana Nebula",
				"Mana Nebula",
				"Damage Nebula",
				"Damage Nebula",
				"Damage Nebula",
				"Stardust Cell",
				"Celled",
				"Minecart",
				"Minecart",
				"Dryad's Bane",
				"Stardust Guardian",
				"Stardust Dragon",
				"Daybroken",
				"Suspicious Looking Eye",
				"Companion Cube"
			};
		}

		public static BuffBan GetBuffBanById(int id)
		{
			for (int i = 0; i < Koishi.BanBuff.BanBuffPlugin.BuffManager.BuffBans.Count; i++)
			{
				if (Koishi.BanBuff.BanBuffPlugin.BuffManager.BuffBans[i].ID == id)
				{
					return Koishi.BanBuff.BanBuffPlugin.BuffManager.BuffBans[i];
				}
			}
			return null;
		}

		public static List<int> GetBuffByIdOrName(string text)
		{
			int num = -1;
			if (!int.TryParse(text, out num))
			{
				return Utils.GetBuffByName(text);
			}
			return new List<int>()
			{
				num
			};
		}

		public static List<int> GetBuffByName(string name)
		{
			List<int> nums = new List<int>();
			string lower = name.ToLower();

            foreach(string current in Utils.BuffID)
            {
                if (string.IsNullOrEmpty(current))
                {
                    continue;
                }

                if (current.ToLower() == lower)
                {
                    return new List<int>()
                        {
                            Utils.BuffID.IndexOf(current)
                        };
                }
                else
                {
                    if (current.ToLower().StartsWith(lower))
                    {
                        nums.Add(Utils.BuffID.IndexOf(current));
                    }
                }
            }

			return nums;
		}

		public static string Name(int id)
		{
            if(id < 1 || id >= BuffID.Count)
            {
                return string.Format("Unknown Buff ({0})", id);
            }
            return string.Format("{0} ({1})", BuffID[id], id);
		}
	}
}