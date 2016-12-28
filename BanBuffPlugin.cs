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
using Koishi.BanBuff.Commands;

namespace Koishi.BanBuff
{
	[ApiVersion(2, 0)]
	public class BanBuffPlugin : TerrariaPlugin
	{
		public static BuffManager BuffManager;

		public override string Author
		{
			get
			{
				return "RYH(Koishi) and Levi Middleton";
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

		public override Version Version
		{
			get
			{
				return new Version("1.3.0.0");
			}
		}

        IDictionary<string, IChatCommand> _commandDict = new SortedDictionary<string, IChatCommand>();

		public BanBuffPlugin(Main game) : base(game)
		{
            base.Order = 1;
            {
                IChatCommand cmd = new AddCommand(BuffManager);
                _commandDict.Add(cmd.Command.ToLower(), cmd);
            }
            {
                IChatCommand cmd = new AllowCommand(BuffManager);
                _commandDict.Add(cmd.Command.ToLower(), cmd);
            }
            {
                IChatCommand cmd = new DelCommand(BuffManager);
                _commandDict.Add(cmd.Command.ToLower(), cmd);
            }
            {
                IChatCommand cmd = new DisallowCommand(BuffManager);
                _commandDict.Add(cmd.Command.ToLower(), cmd);
            }
            {
                IChatCommand cmd = new ListCommand(BuffManager);
                _commandDict.Add(cmd.Command.ToLower(), cmd);
            }
            {
                IChatCommand cmd = new HelpCommand(_commandDict);
                _commandDict.Add(cmd.Command.ToLower(), cmd);
            }
        }

		private void OnChatCommand(CommandArgs args)
		{
			string action = (args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower());
            IChatCommand cmd;
            if(!_commandDict.TryGetValue(action, out cmd))
            {
                cmd = _commandDict["help"];
            }
            if(args.Parameters.Count != cmd.ParameterCount && cmd.ParameterCount != 0)
            {
                args.Player.SendErrorMessage("Usage: {0}banbuff {1}", TShockAPI.Commands.Specifier, cmd.Usage);
                return;
            }
            cmd.Invoke(args);
		}

		public override void Initialize()
		{
            TShockAPI.Commands.ChatCommands.Add(new Command("komeiji.banbuff", OnChatCommand, "banbuff", "buffban"));
			BuffManager = new BuffManager(TShock.DB);
			ServerApi.Hooks.NetGetData.Register(this, new HookHandler<GetDataEventArgs>(OnGetData));
		}

		private void OnGetData(GetDataEventArgs args)
		{
            switch(args.MsgID)
            {
                case PacketTypes.PlayerBuff:
                    {
                        using (MemoryStream memoryStream = new MemoryStream(args.Msg.readBuffer, args.Index, args.Length))
                        using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true))
                        {
                            binaryReader.ReadByte();
                            TSPlayer player = TShock.Players[args.Msg.whoAmI];
                            for (int i = 0; i < 22; i++)
                            {
                                byte num = binaryReader.ReadByte();
                                if (BuffManager.BuffIsBanned(num, player))
                                {
                                    TShock.Utils.ForceKick(player, string.Concat("Has Banned Buff", Utils.Name(num)), false, true);
                                }
                            }
                        }
                    }
                    break;
                case PacketTypes.PlayerAddBuff:
                    {
                        using (MemoryStream memoryStream = new MemoryStream(args.Msg.readBuffer, args.Index, args.Length))
                        using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true))
                        {
                            binaryReader.ReadByte();
                            int num = binaryReader.ReadByte();
                            binaryReader.ReadInt16();
                            TSPlayer player = TShock.Players[args.Msg.whoAmI];
                            if (BuffManager.BuffIsBanned(num, player))
                            {
                                TShock.Utils.ForceKick(player, string.Concat("Has Banned Buff", Utils.Name(num)), false, true);
                            }
                        }
                    }
                    break;
            }
		}
	}
}