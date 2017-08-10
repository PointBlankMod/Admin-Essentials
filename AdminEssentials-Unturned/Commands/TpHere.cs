using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using UnityEngine;

namespace AdminEssentials.Commands
{
    public class TpHere : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "TpHere"
        };

        public override string Help => Translate("TpHere_Help");

        public override string Usage => Commands[0] + Translate("TpHere_Usage");

        public override string DefaultPermission => "adminessentials.commands.tphere";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override EAllowedCaller AllowedCaller => EAllowedCaller.PLAYER;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer player))
            {
                UnturnedChat.SendMessage(executor, Translate("PlayerNotFound"), ConsoleColor.Red);
                return;
            }
            if (player.IsInVehicle)
            {
                UnturnedChat.SendMessage(executor, Translate("InVehicle"), ConsoleColor.Red);
                return;
            }

            player.Metadata.Add("pPosition", new Vector3(player.Position.x, player.Position.y, player.Position.z));
            player.Teleport(((UnturnedPlayer)executor).Position);
        }
    }
}
