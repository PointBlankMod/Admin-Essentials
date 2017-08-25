using System;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Skills;
using SDG.Unturned;

namespace AdminEssentials.Commands
{
    public class MaxSkills : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "MaxSkills"
        };

        public override string Help => Translate("MaxSkills_Help");

        public override string Usage => Commands[0] + Translate("MaxSkills_Usage");

        public override string DefaultPermission => "adminessentials.commands.maxskills";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            UnturnedPlayer[] players = new UnturnedPlayer[1];
            players[0] = (UnturnedPlayer)executor;

            if(args.Length > 0)
            {
                if (!UnturnedPlayer.TryGetPlayers(args[0], out players))
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

                for(int i = 0; i < 7; i++)
                {
                    Skill skl = player.USkills.skills[(int)ESkillset.DEFENSE][i];

                    skl.level = skl.max;
                }
                for(int i = 0; i < 7; i++)
                {
                    Skill skl = player.USkills.skills[(int)ESkillset.OFFENSE][i];

                    skl.level = skl.max;
                }
                for (int i = 0; i < 8; i++)
                {
                    Skill skl = player.USkills.skills[(int)ESkillset.SUPPORT][i];

                    skl.level = skl.max;
                }
                player.USkills.askSkills(player.SteamID);
                UnturnedChat.SendMessage(executor, Translate("MaxSkills_Success", player.PlayerName), ConsoleColor.Green);
            });
        }
    }
}
