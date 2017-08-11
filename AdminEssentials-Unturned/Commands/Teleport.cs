using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace AdminEssentials.Commands
{
    public class Teleport : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "tp",
            "Teleport"
        };

        public override string Help => Translate("Teleport_Help");

        public override string Usage => Commands[0] + Translate("Teleport_Usage");

        public override string DefaultPermission => "adminessentials.commands.teleport";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override int MinimumParams => 1;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            UnturnedPlayer player = (UnturnedPlayer)executor;

            if(args.Length > 1)
            {
                if (!UnturnedPlayer.TryGetPlayer(args[1], out player))
                {
                    UnturnedChat.SendMessage(executor, Translate("PlayerNotFound"), ConsoleColor.Red);
                    return;
                }
            }

            if (UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer pTarget))
            {
                if (player.Metadata.ContainsKey("pPosition"))
                    player.Metadata["pPosition"] = player.Position.Duplicate();
                else
                    player.Metadata.Add("pPosition", player.Position.Duplicate());
                player.Teleport(pTarget.Player.transform.position);
                UnturnedChat.SendMessage(executor, string.Format(Translate("Teleport_Teleport"), player.PlayerName, pTarget.PlayerName), ConsoleColor.Green);
            }
            else
            {
                Node nTarget = LevelNodes.nodes.FirstOrDefault(a => a.type == ENodeType.LOCATION && NameTool.checkNames(args[0], ((LocationNode)a).name));

                if (nTarget == null)
                {
                    UnturnedChat.SendMessage(executor, Translate("Teleport_Invalid"), ConsoleColor.Red);
                    return;
                }

                if (player.Metadata.ContainsKey("pPosition"))
                    player.Metadata["pPosition"] = player.Position.Duplicate();
                else
                    player.Metadata.Add("pPosition", player.Position.Duplicate());
                player.Teleport(nTarget.point);
                UnturnedChat.SendMessage(executor, string.Format(Translate("Teleport_Teleport"), player.PlayerName, ((LocationNode)nTarget).name), ConsoleColor.Green);
            }
        }
    }
}
