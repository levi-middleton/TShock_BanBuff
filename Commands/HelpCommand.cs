using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace Koishi.BanBuff.Commands
{
    public class HelpCommand : IChatCommand
    {
        private IDictionary<string, IChatCommand> _cmds;

        public HelpCommand(IDictionary<string, IChatCommand> cmds)
        {
            _cmds = cmds;
        }

        public string Command { get { return "help"; } }

        public int ParameterCount { get { return 0; } }

        public string Usage { get { return "help [page]"; } }

        public string Description { get { return "Provides help text"; } }

        public void Invoke(CommandArgs args)
        {
            int num;
            if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out num))
            {
                return;
            }
            List<string> strs = _cmds.Select(x => string.Format("{0} - {1}.", x.Value.Usage, x.Value.Description)).ToList();
            PaginationTools.Settings setting = new PaginationTools.Settings();
            setting.HeaderFormat = "Buff Ban Sub-Commands ({0}/{1}):";
            setting.FooterFormat = StringExt.SFormat("Type {0}banbuff help {{0}} for more sub-commands.", TShockAPI.Commands.Specifier);
            PaginationTools.SendPage(args.Player, num, strs, setting);
        }
    }
}
