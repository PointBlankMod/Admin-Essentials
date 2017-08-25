using System;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace AdminEssentials.Commands
{
    public class Boom : PointBlankCommand
    {
        #region Info
        public static readonly float Damage = 200f;
        #endregion

        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Boom",
            "Explode"
        };

        public override string Help => Translate("Boom_Help");

        public override string Usage => Commands[0] + Translate("Boom_Usage");

        public override string DefaultPermission => "adminessentials.commands.boom";

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
                if (UnturnedPlayer.IsServer(player))
                {
                    UnturnedChat.SendMessage(executor, Translate("TargetServer"), ConsoleColor.Red);
                    return;
                }

                EffectManager.sendEffect(20, EffectManager.INSANE, player.Position);
                DamageTool.explode(player.Position, 10f, EDeathCause.KILL, (UnturnedPlayer.IsServer(executor) ? CSteamID.Nil : ((UnturnedPlayer)executor).SteamID),
                                   Damage, Damage, Damage, Damage, Damage, Damage, Damage, Damage);
                UnturnedChat.SendMessage(executor, Translate("Boom_Boom", player.PlayerName), ConsoleColor.Green);
            });
        }
    }
}
