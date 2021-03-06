﻿using System;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;

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
            if(!UnturnedPlayer.TryGetPlayers(args[0], out UnturnedPlayer[] players))
            {
                UnturnedChat.SendMessage(executor, Translate("PlayerNotFound"), ConsoleColor.Red);
                return;
            }

            players.ForEach((player) =>
            {
                if (player.Metadata.ContainsKey("FreezePosition"))
                {
                    player.Metadata.Remove("FreezePosition");
                    UnturnedChat.SendMessage(executor, Translate("Freeze_Unfreeze", player.PlayerName), ConsoleColor.Green);
                }
                else
                {
                    player.Metadata.Add("FreezePosition", player.Position.Duplicate());
                    UnturnedChat.SendMessage(executor, Translate("Freeze_Freeze", player.PlayerName), ConsoleColor.Green);
                }
            });
        }
    }
}
