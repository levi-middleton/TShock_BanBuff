using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace Koishi.BanBuff.Commands
{
    public interface IChatCommand
    {
        string Command { get; }

        int ParameterCount { get; }

        string Usage { get; }

        string Description { get; }

        void Invoke(CommandArgs args);
    }
}
