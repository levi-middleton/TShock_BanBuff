using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace Koishi.BanBuff.Commands
{
    public class DelCommand : IChatCommand
    {
        private BuffManager _buffManager;

        public DelCommand(BuffManager buffManager)
        {
            _buffManager = buffManager;
        }

        public string Command { get { return "del"; } }

        public int ParameterCount { get { return 2; } }

        public string Usage { get { return "del <buff name>"; } }

        public string Description { get { return "Deletes a buff ban"; } }

        public void Invoke(CommandArgs args)
        {
            string buffName = args.Parameters[1];
            List<int> buffByIdOrName = Utils.GetBuffByIdOrName(buffName);
            if (buffByIdOrName.Count == 0)
            {
                args.Player.SendErrorMessage("Invalid buff - {0}.", buffName);
                return;
            }
            else if (buffByIdOrName.Count > 1)
            {
                TShock.Utils.SendMultipleMatchError(args.Player, buffByIdOrName.Select(x => Utils.Name(x)));
                return;
            }

            _buffManager.RemoveBan(buffByIdOrName[0]);
            args.Player.SendSuccessMessage("Unbanned {0}.", Utils.Name(buffByIdOrName[0]));
        }
    }
}
