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
    public class RequestURL : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "ReqURL",
            "RequestURL"
        };

        public override string Help => Translate("RequestURL_Help");

        public override string Usage => Commands[0] + Translate("RequestURL_Usage");

        public override string DefaultPermission => "adminessentials.commands.requesturl";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 3;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(!UnturnedPlayer.TryGetPlayers(args[2], out UnturnedPlayer[] players))
            {
                UnturnedChat.SendMessage(executor, Translate("PlayerNotFound"), ConsoleColor.Red);
                return;
            }

            players.ForEach((player) =>
            {
                player.Player.sendBrowserRequest(args[1], args[0]);
                UnturnedChat.SendMessage(executor, Translate("RequestURL_Success", player.PlayerName), ConsoleColor.Green);
            });
        }
    }
}
