using System;
using System.Linq;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Server;

namespace AdminEssentials.Commands
{
    public class Vanish : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "vanish"
        };

        public override string Help => Translate("Vanish_Help");

        public override string Usage => Commands[0] + Translate("Vanish_Usage");

        public override string DefaultPermission => "adminessentials.commands.vanish";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override EAllowedCaller AllowedCaller => EAllowedCaller.PLAYER;
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

            if (player.Metadata.ContainsKey("Vanish"))
            {
                UnturnedServer.Players.ForEach((ply) =>
                {
                    if (ply == player)
                        return;

                    if (!ply.PlayerList.Contains(player))
                        ply.AddPlayer(player);
                });
                player.Metadata.Remove("Vanish");
                UnturnedChat.SendMessage(player, Translate("Vanish_Unvanish"), ConsoleColor.Green);
            }
            else
            {
                UnturnedServer.Players.ForEach((ply) =>
                {
                    if (ply == player)
                        return;

                    if (ply.PlayerList.Contains(player))
                        ply.RemovePlayer(player);
                });
                player.Metadata.Add("Vanish", true);
                UnturnedChat.SendMessage(player, Translate("Vanish_Vanish"), ConsoleColor.Green);
            }
        }
    }
}
