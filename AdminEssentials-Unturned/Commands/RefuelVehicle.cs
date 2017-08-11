using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Vehicle;
using SDG.Unturned;

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
            VehicleManager.instance.channel.send("tellVehicleFuel", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER,
                player.Vehicle.InstanceID, player.Vehicle.MaxFuel);
            UnturnedChat.SendMessage(executor, Translate("RefuelVehicle_Success"), ConsoleColor.Green);
        }
    }
}
