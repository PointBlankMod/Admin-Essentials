using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using UnityEngine;

namespace AdminEssentials.Commands
{
    public class Freeze : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "freeze"
        };

        public override string Help => Translate("Freeze_Help");

        public override string Usage => Commands[0] + Translate("Freeze_Usage");

        public override string DefaultPermission => "adminessentials.commands.freeze";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer player))
            {
                UnturnedChat.SendMessage(executor, Translate("PlayerNotFound"), ConsoleColor.Red);
                return;
            }

            if (player.Metadata.ContainsKey("FreezePosition"))
            {
                player.Metadata.Remove("FreezePosition");
                UnturnedChat.SendMessage(executor, Translate("Freeze_Unfreeze", player.PlayerName), ConsoleColor.Green);
            }
            else
            {
                player.Metadata.Add("FreezePosition", new Vector3(player.Position.x, player.Position.y, player.Position.z));
                UnturnedChat.SendMessage(executor, Translate("Freeze_Freeze", player.PlayerName), ConsoleColor.Green);
            }
        }
    }
}
