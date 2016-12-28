using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace Koishi.BanBuff.Commands
{
    public class AddCommand : IChatCommand
    {
        private BuffManager _buffManager;

        public AddCommand(BuffManager buffManager)
        {
            _buffManager = buffManager;
        }

        public string Command { get { return "add"; } }

        public int ParameterCount { get { return 2; } }

        public string Usage { get { return "add <buff name|buff id>"; } }

        public string Description { get { return "Adds a buff ban"; } }

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

            _buffManager.AddNewBan(buffByIdOrName[0]);
            args.Player.SendSuccessMessage("Banned {0}.", Utils.Name(buffByIdOrName[0]));
        }
    }
}
