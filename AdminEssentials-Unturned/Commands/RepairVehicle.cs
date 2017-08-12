using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;

namespace AdminEssentials.Commands
{
    public class RepairVehicle : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "RepairVehicle"
        };

        public override string Help => Translate("RepairVehicle_Help");

        public override string Usage => Commands[0];

        public override string DefaultPermission => "adminessentials.commands.repairvehicle";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override EAllowedCaller AllowedCaller => EAllowedCaller.PLAYER;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            UnturnedPlayer player = (UnturnedPlayer)executor;

            if (!player.IsInVehicle)
            {
                UnturnedChat.SendMessage(executor, Translate("OutVehicle"), ConsoleColor.Red);
                return;
            }
            player.Vehicle.Health = player.Vehicle.MaxHealth;
            UnturnedChat.SendMessage(executor, Translate("RepairVehicle_Success"), ConsoleColor.Green);
        }
    }
}
