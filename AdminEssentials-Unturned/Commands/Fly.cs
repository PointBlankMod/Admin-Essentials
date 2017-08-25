using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using Steamworks;
using SDG.Unturned;

namespace AdminEssentials.Commands
{
#if DEBUG
    public class Fly : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Fly"
        };

        public override string Help => Translate("Fly_Help");

        public override string Usage => Commands[0] + Translate("Fly_Usage");

        public override string DefaultPermission => "adminessentials.commands.fly";

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

                if (player.Metadata.ContainsKey("Fly"))
                {
                    player.Metadata.Remove("Fly");
                    player.Movement.gravity = 1;
                    player.Stance.channel.send("tellStance", ESteamCall.OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
                    {
                        (byte)EPlayerStance.SWIM
                    });
                    UnturnedChat.SendMessage(player, Translate("Fly_Stop"), ConsoleColor.Green);
                }
                else
                {
                    player.Metadata.Add("Fly", true);
                    player.Movement.gravity = 0;
                    player.Stance.channel.send("tellStance", ESteamCall.OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
                    {
                        (byte)EPlayerStance.STAND
                    });
                    UnturnedChat.SendMessage(player, Translate("Fly_Start"), ConsoleColor.Green);
                }
            });
        }
    }
#endif
}
