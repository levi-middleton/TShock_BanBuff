using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.DB;
using System.Linq;

namespace Koishi.BanBuff
{
	[ApiVersion(1, 25)]
	public class BanBuff : TerrariaPlugin
	{
		public static BuffManager BuffBans;

		public override string Author
		{
			get
			{
				return "RYH(Koishi)";
			}
		}

		public override string Description
		{
			get
			{
				return "Ban Buff";
			}
		}

		public override string Name
		{
			get
			{
				return "BanBuff";
			}
		}

		public override System.Version Version
		{
			get
			{
				return new System.Version("1.3.0.0");
			}
		}

		public BanBuff(Main game) : base(game)
		{
            base.Order = 1;
		}

		private static void BuffBan(CommandArgs args)
		{
			int num = 0;
			int num1 = 0;
			string str = (args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower());
			string str1 = str;
			if (str != null)
			{
				if (str1 == "add")
				{
					if (args.Parameters.Count != 2)
					{
						TSPlayer player = args.Player;
						object[] specifier = new object[] { Commands.Specifier };
						player.SendErrorMessage("Invalid syntax! Proper syntax: {0}buffban add <buff name>", specifier);
						return;
					}
					List<int> buffByIdOrName = Koishi.BanBuff.Utils.GetBuffByIdOrName(args.Parameters[1]);
					if (buffByIdOrName.Count == 0)
					{
						args.Player.SendErrorMessage("Invalid buff.");
						return;
					}
					if (buffByIdOrName.Count <= 1)
					{
						Koishi.BanBuff.BanBuff.BuffBans.AddNewBan(buffByIdOrName[0]);
						args.Player.SendSuccessMessage(string.Concat("Banned ", Koishi.BanBuff.Utils.Name(buffByIdOrName[0]), "."));
						return;
					}
					TShock.Utils.SendMultipleMatchError(args.Player, 
						from i in buffByIdOrName
						select Koishi.BanBuff.Utils.Name(i));
					return;
				}
				if (str1 == "allow")
				{
					if (args.Parameters.Count != 3)
					{
						TSPlayer tSPlayer = args.Player;
						object[] objArray = new object[] { Commands.Specifier };
						tSPlayer.SendErrorMessage("Invalid syntax! Proper syntax: {0}buffban allow <buff name> <group name>", objArray);
						return;
					}
					List<int> nums = Koishi.BanBuff.Utils.GetBuffByIdOrName(args.Parameters[1]);
					if (nums.Count == 0)
					{
						args.Player.SendErrorMessage("Invalid buff.");
						return;
					}
					if (nums.Count > 1)
					{
						TShock.Utils.SendMultipleMatchError(args.Player, 
							from i in nums
							select Koishi.BanBuff.Utils.Name(i));
						return;
					}
					if (!TShock.Groups.GroupExists(args.Parameters[2]))
					{
						args.Player.SendErrorMessage("Invalid group.");
						return;
					}
					Koishi.BanBuff.BuffBan buffBanById = Koishi.BanBuff.Utils.GetBuffBanById(nums[0]);
					if (buffBanById == null)
					{
						TSPlayer player1 = args.Player;
						object[] objArray1 = new object[] { Koishi.BanBuff.Utils.Name(nums[0]) };
						player1.SendErrorMessage("{0} is not banned.", objArray1);
						return;
					}
					if (buffBanById.AllowedGroups.Contains(args.Parameters[2]))
					{
						TSPlayer tSPlayer1 = args.Player;
						object[] item = new object[] { args.Parameters[2], Koishi.BanBuff.Utils.Name(nums[0]) };
						tSPlayer1.SendWarningMessage("{0} is already allowed to use {1}.", item);
						return;
					}
					Koishi.BanBuff.BanBuff.BuffBans.AllowGroup(nums[0], args.Parameters[2]);
					TSPlayer player2 = args.Player;
					object[] item1 = new object[] { args.Parameters[2], Koishi.BanBuff.Utils.Name(nums[0]) };
					player2.SendSuccessMessage("{0} has been allowed to use {1}.", item1);
					return;
				}
				if (str1 == "del")
				{
					if (args.Parameters.Count != 2)
					{
						TSPlayer tSPlayer2 = args.Player;
						object[] specifier1 = new object[] { Commands.Specifier };
						tSPlayer2.SendErrorMessage("Invalid syntax! Proper syntax: {0}buffban del <buff name>", specifier1);
						return;
					}
					List<int> buffByIdOrName1 = Koishi.BanBuff.Utils.GetBuffByIdOrName(args.Parameters[1]);
					if (buffByIdOrName1.Count == 0)
					{
						args.Player.SendErrorMessage("Invalid buff.");
						return;
					}
					if (buffByIdOrName1.Count <= 1)
					{
						Koishi.BanBuff.BanBuff.BuffBans.RemoveBan(buffByIdOrName1[0]);
						args.Player.SendSuccessMessage(string.Concat("Unbanned ", Koishi.BanBuff.Utils.Name(buffByIdOrName1[0]), "."));
						return;
					}
					TShock.Utils.SendMultipleMatchError(args.Player, 
						from i in buffByIdOrName1
						select Koishi.BanBuff.Utils.Name(i));
					return;
				}
				if (str1 == "disallow")
				{
					if (args.Parameters.Count != 3)
					{
						TSPlayer player3 = args.Player;
						object[] specifier2 = new object[] { Commands.Specifier };
						player3.SendErrorMessage("Invalid syntax! Proper syntax: {0}buffban disallow <buff name> <group name>", specifier2);
						return;
					}
					List<int> nums1 = Koishi.BanBuff.Utils.GetBuffByIdOrName(args.Parameters[1]);
					if (nums1.Count == 0)
					{
						args.Player.SendErrorMessage("Invalid buff.");
						return;
					}
					if (nums1.Count > 1)
					{
						TShock.Utils.SendMultipleMatchError(args.Player, 
							from i in nums1
							select Koishi.BanBuff.Utils.Name(i));
						return;
					}
					if (!TShock.Groups.GroupExists(args.Parameters[2]))
					{
						args.Player.SendErrorMessage("Invalid group.");
						return;
					}
					Koishi.BanBuff.BuffBan buffBan = Koishi.BanBuff.Utils.GetBuffBanById(nums1[0]);
					if (buffBan == null)
					{
						TSPlayer tSPlayer3 = args.Player;
						object[] objArray2 = new object[] { Koishi.BanBuff.Utils.Name(nums1[0]) };
						tSPlayer3.SendErrorMessage("{0} is not banned.", objArray2);
						return;
					}
					if (!buffBan.AllowedGroups.Contains(args.Parameters[2]))
					{
						TSPlayer player4 = args.Player;
						object[] item2 = new object[] { args.Parameters[2], Koishi.BanBuff.Utils.Name(nums1[0]) };
						player4.SendWarningMessage("{0} is already disallowed to use {1}.", item2);
						return;
					}
					Koishi.BanBuff.BanBuff.BuffBans.RemoveGroup(nums1[0], args.Parameters[2]);
					TSPlayer tSPlayer4 = args.Player;
					object[] item3 = new object[] { args.Parameters[2], Koishi.BanBuff.Utils.Name(nums1[0]) };
					tSPlayer4.SendSuccessMessage("{0} has been disallowed to use {1}.", item3);
					return;
				}
				if (str1 == "help")
				{
					if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out num))
					{
						return;
					}
					List<string> strs = new List<string>()
					{
						"add <buff name> - Adds an item ban.",
						"allow <buff name> <group name> - Allows a group to use an item.",
						"del <buff name> - Deletes an item ban.",
						"disallow <buff name> <group name> - Disallows a group from using an item.",
						"list [page] - Lists all item bans."
					};
					TSPlayer player5 = args.Player;
					PaginationTools.Settings setting = new PaginationTools.Settings();
					setting.HeaderFormat = "Buff Ban Sub-Commands ({0}/{1}):";
					object[] specifier3 = new object[] { Commands.Specifier };
					setting.FooterFormat = StringExt.SFormat("Type {0}buffban help {{0}} for more sub-commands.", specifier3);
					PaginationTools.SendPage(player5, num, strs, setting);
					return;
				}
				if (str1 != "list")
				{
					return;
				}
				if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out num1))
				{
					return;
				}
				IEnumerable<string> buffBans = 
					from BuffBan in Koishi.BanBuff.BanBuff.BuffBans.BuffBans
					select BuffBan.Name;
				TSPlayer tSPlayer5 = args.Player;
				List<string> strs1 = PaginationTools.BuildLinesFromTerms(buffBans, null, ", ", 80);
				PaginationTools.Settings setting1 = new PaginationTools.Settings();
				setting1.HeaderFormat = "Buff bans ({0}/{1}):";
				object[] objArray3 = new object[] { Commands.Specifier };
				setting1.FooterFormat = StringExt.SFormat("Type {0}buffban list {{0}} for more.", objArray3);
				setting1.NothingToDisplayString = "There are currently no banned buffs.";
				PaginationTools.SendPage(tSPlayer5, num1, strs1, setting1);
			}
		}

		public override void Initialize()
		{
			List<Command> chatCommands = Commands.ChatCommands;
			CommandDelegate commandDelegate = new CommandDelegate(Koishi.BanBuff.BanBuff.BuffBan);
			string[] strArrays = new string[] { "banbuff", "buffban" };
			chatCommands.Add(new Command("komeiji.banbuff", commandDelegate, strArrays));
			Koishi.BanBuff.BanBuff.BuffBans = new BuffManager(TShock.DB);
			ServerApi.Hooks.NetGetData.Register(this, new HookHandler<GetDataEventArgs>(this.OnGetData));
		}

		private void OnGetData(GetDataEventArgs args)
		{
			if (args.MsgID == PacketTypes.PlayerBuff)
			{
				using (MemoryStream memoryStream = new MemoryStream(args.Msg.readBuffer, args.Index, args.Length))
				{
					using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true))
					{
						binaryReader.ReadByte();
						TSPlayer players = TShock.Players[args.Msg.whoAmI];
						for (int i = 0; i < 22; i++)
						{
							byte num = binaryReader.ReadByte();
							if (Koishi.BanBuff.BanBuff.BuffBans.BuffIsBanned((int)num, players))
							{
								TShock.Utils.ForceKick(players, string.Concat("Has Banned Buff", Koishi.BanBuff.Utils.Name((int)num)), false, true);
							}
						}
					}
				}
			}
			if (args.MsgID == PacketTypes.PlayerAddBuff)
			{
				using (MemoryStream memoryStream1 = new MemoryStream(args.Msg.readBuffer, args.Index, args.Length))
				{
					using (BinaryReader binaryReader1 = new BinaryReader(memoryStream1, Encoding.UTF8, true))
					{
						binaryReader1.ReadByte();
						int num1 = binaryReader1.ReadByte();
						binaryReader1.ReadInt16();
						TSPlayer tSPlayer = TShock.Players[args.Msg.whoAmI];
						if (Koishi.BanBuff.BanBuff.BuffBans.BuffIsBanned(num1, tSPlayer))
						{
							TShock.Utils.ForceKick(tSPlayer, string.Concat("Has Banned Buff", Koishi.BanBuff.Utils.Name(num1)), false, true);
						}
					}
				}
			}
		}
	}
}