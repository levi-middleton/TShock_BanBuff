using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace Koishi.BanBuff.Commands
{
    public class AllowCommand : IChatCommand
    {
        private BuffManager _buffManager;

        public AllowCommand(BuffManager buffManager)
        {
            _buffManager = buffManager;
        }

        public string Command {  get { return "allow"; } }

        public int ParameterCount {  get { return 3; } }

        public string Usage {  get { return "allow <buff name|buff id> <group name>"; } }

        public string Description { get { return "Allows a group to use a buff"; } }

        public void Invoke(CommandArgs args)
        {
            string buffName = args.Parameters[1];
            List<int> nums = Utils.GetBuffByIdOrName(buffName);
            if (nums.Count == 0)
            {
                args.Player.SendErrorMessage("Invalid buff - {0}.", buffName);
                return;
            }
            else if (nums.Count > 1)
            {
                TShock.Utils.SendMultipleMatchError(args.Player, nums.Select(i => Utils.Name(i)));
                return;
            }

            BuffBan buffBanById = Utils.GetBuffBanById(nums[0]);
            if (buffBanById == null)
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
            else if (buffBanById.AllowedGroups.Contains(groupName))
            {
                args.Player.SendWarningMessage("{0} is already allowed to use {1}.", groupName, Utils.Name(nums[0]));
                return;
            }

            _buffManager.AllowGroup(nums[0], groupName);
            args.Player.SendSuccessMessage("{0} has been allowed to use {1}.", groupName, Utils.Name(nums[0]));
            return;
        }
    }
}
