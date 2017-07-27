using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;

namespace AdminEssentials.Commands
{
    [PointBlankCommand("GodMode", 0)]
    public class GodMode : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "god",
            "godmode"
        };

        public override string Help => AdminEssentials.Instance.Translate("GodMode_Help");

        public override string Usage => Commands[0] + AdminEssentials.Instance.Translate("GodMode_Usage");

        public override string DefaultPermission => "adminessentials.commands.godmode";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedPlayer player = executor;
            if(args.Length > 0)
            {
                if(!UnturnedPlayer.TryGetPlayer(args[0], out player))
                {
                    UnturnedChat.SendMessage(executor, AdminEssentials.Instance.Translate("PlayerNotFound"), ConsoleColor.Red);
                    return;
                }
            }
            if(UnturnedPlayer.IsServer(player))
            {
                UnturnedChat.SendMessage(executor, AdminEssentials.Instance.Translate("FailServer"), ConsoleColor.Red);
                return;
            }

            if (player.Metadata.ContainsKey("GodMode"))
                player.Metadata.Remove("GodMode");
            else
                player.Metadata.Add("GodMode", true);
            UnturnedChat.SendMessage(player, AdminEssentials.Instance.Translate("GodMode_Success"), ConsoleColor.Green);
        }
    }
}
