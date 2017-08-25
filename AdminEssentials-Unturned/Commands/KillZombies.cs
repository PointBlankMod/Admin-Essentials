using System;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using UnityEngine;

namespace AdminEssentials.Commands
{
    public class KillZombies : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "KillZombies"
        };

        public override string Help => Translate("KillZombies_Help");

        public override string Usage => Commands[0];

        public override string DefaultPermission => "adminessentials.commands.killzombies";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            ZombieManager.tickingZombies.ForEach((zombie) =>
            {
                ZombieManager.sendZombieDead(zombie, Vector3.zero);
            });
            UnturnedChat.SendMessage(executor, Translate("KillZombies_Success"), ConsoleColor.Green);
        }
    }
}
