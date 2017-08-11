using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using UnityEngine;

namespace AdminEssentials.Commands
{
    public class KillAnimals : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "KillAnimals"
        };

        public override string Help => Translate("KillAnimals_Help");

        public override string Usage => Commands[0];

        public override string DefaultPermission => "adminessentials.commands.killanimals";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            AnimalManager.animals.ForEach((animal) =>
            {
                AnimalManager.sendAnimalDead(animal, Vector3.zero);
            });
            UnturnedChat.SendMessage(executor, Translate("KillAnimals_Success"), ConsoleColor.Green);
        }
    }
}
