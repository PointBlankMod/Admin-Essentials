using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;

namespace AdminEssentials.Commands
{
    public class Heal : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Heal"
        };

        public override string Help => Translate("Heal_Help");

        public override string Usage => Commands[0] + Translate("Heal_Usage");

        public override string DefaultPermission => "adminessentials.commands.heal";

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
                    UnturnedChat.SendMessage(executor, Translate("FailServer"), ConsoleColor.Red);
                    return;
                }

                player.Life.sendRevive();
                UnturnedChat.SendMessage(executor, Translate("Heal_Success", player.PlayerName), ConsoleColor.Green);
            });
        }
    }
}
