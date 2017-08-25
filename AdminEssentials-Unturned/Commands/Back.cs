using System;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using UnityEngine;

namespace AdminEssentials.Commands
{
    public class Back : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Back"
        };

        public override string Help => Translate("Back_Help");

        public override string Usage => Commands[0] + Translate("Back_Usage");

        public override string DefaultPermission => "adminessentials.commands.back";

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
                if (!player.Metadata.ContainsKey("pPosition"))
                {
                    UnturnedChat.SendMessage(executor, Translate("Back_NoLocation"), ConsoleColor.Red);
                    return;
                }
                Vector3 pos = player.Position.Duplicate();

                player.Teleport((Vector3)player.Metadata["pPosition"]);
                if (player.Metadata.ContainsKey("pPosition"))
                    player.Metadata["pPosition"] = pos;
                else
                    player.Metadata.Add("pPosition", pos);
                UnturnedChat.SendMessage(executor, Translate("Back_Successful", player.PlayerName), ConsoleColor.Green);
            });
        }
    }
}
