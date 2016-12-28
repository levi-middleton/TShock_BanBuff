using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace Koishi.BanBuff.Commands
{
    public class DisallowCommand : IChatCommand
    {
        private BuffManager _buffManager;

        public DisallowCommand(BuffManager buffManager)
        {
            _buffManager = buffManager;
        }

        public string Command { get { return "disallow"; } }

        public int ParameterCount {  get { return 3; } }

        public string Usage { get { return "disallow <buff name|buff id> <group name>"; } }

        public string Description { get { return "Disallows a group from using a buff"; } }

        public void Invoke(CommandArgs args)
        {
            string buffName = args.Parameters[1];
            List<int> nums = Utils.GetBuffByIdOrName(buffName);
            if (nums.Count == 0)
            {
                args.Player.SendErrorMessage("Invalid buff - {0}.", buffName);
                return;
            }
            if (nums.Count > 1)
            {
                TShock.Utils.SendMultipleMatchError(args.Player, nums.Select(i => Utils.Name(i)));
                return;
            }

            BuffBan buffBan = Utils.GetBuffBanById(nums[0]);
            if (buffBan == null)
            {
                args.Player.SendErrorMessage("{0} is not banned.", Utils.Name(nums[0]));
                return;
            }

            string groupName = args.Parameters[2];
            if (!TShock.Groups.GroupExists(groupName))
            {
                args.Player.SendErrorMessage("Invalid group - {0}.", groupName);
                return;
            }
            if (!buffBan.AllowedGroups.Contains(groupName))
            {
                args.Player.SendWarningMessage("{0} is already disallowed to use {1}.", groupName, Utils.Name(nums[0]));
                return;
            }

            _buffManager.RemoveGroup(nums[0], groupName);
            args.Player.SendSuccessMessage("{0} has been disallowed to use {1}.", groupName, Utils.Name(nums[0]));
        }
    }
}
