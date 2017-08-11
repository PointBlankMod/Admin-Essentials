using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;

namespace AdminEssentials.Commands
{
    public class Repair : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Fix",
            "Repair"
        };

        public override string Help => Translate("Repair_Help");

        public override string Usage => Commands[0] + Translate("Repair_Usage");

        public override string DefaultPermission => "adminessentials.commands.repair";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            UnturnedPlayer[] players = new UnturnedPlayer[1];
            players[0] = (UnturnedPlayer)executor;

            if(args.Length > 0)
            {
                if(!UnturnedPlayer.TryGetPlayers(args[0], out players))
                {
                    UnturnedChat.SendMessage(executor, Translate("PlayerNotFound"), ConsoleColor.Red);
                    return;
                }
            }

            players.ForEach((player) =>
            {
                if (UnturnedPlayer.IsServer(player))
                {
                    UnturnedChat.SendMessage(executor, Translate("TargetServer"), ConsoleColor.Red);
                    return;
                }

                player.Items.ForEach((item) =>
                {
                    
                });
            });
        }
    }
}
