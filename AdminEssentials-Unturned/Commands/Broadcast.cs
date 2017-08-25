using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Unturned.Chat;
using UnityEngine;

namespace AdminEssentials.Commands
{
    public class Broadcast : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Broadcast"
        };

        public override string Help => Translate("Broadcast_Help");

        public override string Usage => Commands[0] + Translate("Broadcast_Usage");

        public override string DefaultPermission => "adminessentials.commands.broadcast";

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            UnturnedChat.Broadcast(args[0], Color.magenta);
        }
    }
}
