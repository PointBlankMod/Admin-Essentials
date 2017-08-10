using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;

namespace AdminEssentials.Commands
{
    public class ClearInventory : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "ci",
            "clearinventory"
        };

        public override string Help => Translate("ClearInventory_Help");

        public override string Usage => Commands[0] + Translate("ClearInventory_Usage");

        public override string DefaultPermission => "adminessentials.commands.clearinventory";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            UnturnedPlayer player = (UnturnedPlayer)executor;

            if(args.Length > 0)
            {
                if(!UnturnedPlayer.TryGetPlayer(args[0], out player))
                {
                    UnturnedChat.SendMessage(executor, Translate("PlayerNotFound"), ConsoleColor.Red);
                    return;
                }
            }
            if (UnturnedPlayer.IsServer(player))
            {
                UnturnedChat.SendMessage(executor, Translate("TargetServer"), ConsoleColor.Red);
                return;
            }

            while(player.Items.Length > 0)
                player.RemoveItem(player.Items[0]);
            UnturnedChat.SendMessage(executor, Translate("ClearInventory_Success", player.PlayerName), ConsoleColor.Green);
        }
    }
}
