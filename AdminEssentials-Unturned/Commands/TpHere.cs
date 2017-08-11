using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
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
            if(!UnturnedPlayer.TryGetPlayers(args[0], out UnturnedPlayer[] players))
            {
                UnturnedChat.SendMessage(executor, Translate("PlayerNotFound"), ConsoleColor.Red);
                return;
            }
            players.ForEach((player) =>
            {
                if (player.IsInVehicle)
                {
                    UnturnedChat.SendMessage(executor, Translate("InVehicle"), ConsoleColor.Red);
                    return;
                }

                if (player.Metadata.ContainsKey("pPosition"))
                    player.Metadata["pPosition"] = player.Position.Duplicate();
                else
                    player.Metadata.Add("pPosition", player.Position.Duplicate());
                player.Teleport(((UnturnedPlayer)executor).Position);
            });
        }
    }
}
