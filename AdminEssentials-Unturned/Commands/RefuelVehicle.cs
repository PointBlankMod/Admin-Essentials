using System;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;

namespace AdminEssentials.Commands
{
    public class RefuelVehicle : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Refuel",
            "RefuelVehicle"
        };

        public override string Help => Translate("RefuelVehicle_Help");

        public override string Usage => Commands[0];

        public override string DefaultPermission => "adminessentials.commands.refuelvehicle";

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
            player.Vehicle.Fuel = player.Vehicle.MaxFuel;
            UnturnedChat.SendMessage(executor, Translate("RefuelVehicle_Success"), ConsoleColor.Green);
        }
    }
}
