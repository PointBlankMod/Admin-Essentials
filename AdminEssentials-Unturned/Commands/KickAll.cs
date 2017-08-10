using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Player;

namespace AdminEssentials.Commands
{
    public class KickAll : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "kickall"
        };

        public override string Help => Translate("KickAll_Help");

        public override string Usage => Commands[0] + Translate("KickAll_Usage");

        public override string DefaultPermission => "adminessentials.commands.kickall";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            string reason = Translate("NotSpecified");

            if (args.Length > 0)
                reason = args[0];
            while(UnturnedServer.Players.Length > 0)
                UnturnedServer.Players[0].Kick(reason);
        }
    }
}
