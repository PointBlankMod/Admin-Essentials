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
    public class Sudo : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Sudo"
        };

        public override string Help => Translate("Sudo_Help");

        public override string Usage => Commands[0] + Translate("Sudo_Usage");

        public override string DefaultPermission => "adminessentials.commands.sudo";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            UnturnedPlayer[] players = new UnturnedPlayer[0];

            if(args.Length > 1)
            {
                if(UnturnedPlayer.TryGetPlayers(args[1], out players))
                {
                    players.ForEach((player) =>
                    {
                        if (player.IsAdmin && (!UnturnedPlayer.IsServer(executor) && !((UnturnedPlayer)executor).HasPermission("adminessentials.commands.sudo.admin")))
                        {
                            UnturnedChat.SendMessage(executor, Translate("Sudo_Admin"), ConsoleColor.Red);
                            return;
                        }
                        if (player == (UnturnedPlayer)executor)
                        {
                            UnturnedChat.SendMessage(executor, Translate("Sudo_Self"), ConsoleColor.Red);
                            return;
                        }

                        player.Sudo(args[0]);
                    });
                    UnturnedChat.SendMessage(executor, Translate("Sudo_Success"), ConsoleColor.Green);
                    return;
                }
            }

            if (UnturnedPlayer.IsServer(executor) || ((UnturnedPlayer)executor).HasPermission("adminessentials.commands.sudo.server"))
            {
                CommandWindow.input.onInputText(args[0]);
                UnturnedChat.SendMessage(executor, Translate("Sudo_Success"), ConsoleColor.Green);
            }
            else
                UnturnedChat.SendMessage(executor, Translate("Sudo_Server"), ConsoleColor.Red);
        }
    }
}
