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
    public class Back : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Back"
        };

        public override string Help => Translate("Back_Help");

        public override string Usage => Commands[0];

        public override string DefaultPermission => "adminessentials.commands.back";

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
                UnturnedChat.SendMessage(executor, Translate("Back_Server"), ConsoleColor.Red);
                return;
            }
            if (!player.Metadata.ContainsKey("pPosition"))
            {
                UnturnedChat.SendMessage(executor, Translate("Back_NoLocation"), ConsoleColor.Red);
                return;
            }
            Vector3 pos = new Vector3(player.Position.x, player.Position.y, player.Position.z);

            player.Teleport((Vector3)player.Metadata["pPosition"]);
            player.Metadata.Add("pPosition", pos);
            UnturnedChat.SendMessage(executor, Translate("Back_Successful", player.PlayerName), ConsoleColor.Green);
        }
    }
}
