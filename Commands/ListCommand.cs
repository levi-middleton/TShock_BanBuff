using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace Koishi.BanBuff.Commands
{
    public class ListCommand : IChatCommand
    {
        private BuffManager _buffManager;

        public ListCommand(BuffManager buffManager)
        {
            _buffManager = buffManager;
        }

        public string Command { get { return "list"; } }

        public int ParameterCount { get { return 0; } }

        public string Usage {  get { return "list [page]"; } }

        public string Description {  get { return "Lists all buff bans"; } }

        public void Invoke(CommandArgs args)
        {
            int num;
            if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out num))
            {
                return;
            }
            IEnumerable<string> buffBans = from BuffBan in _buffManager.BuffBans select BuffBan.Name;
            List<string> strs = PaginationTools.BuildLinesFromTerms(buffBans, null, ", ", 80);
            PaginationTools.Settings setting = new PaginationTools.Settings();
            setting.HeaderFormat = "Buff Bans ({0}/{1}):";
            setting.FooterFormat = StringExt.SFormat("Type {0}banbuff list {{0}} for more.", TShockAPI.Commands.Specifier);
            setting.NothingToDisplayString = "There are currently no banned buffs.";
            PaginationTools.SendPage(args.Player, num, strs, setting);
        }
    }
}
