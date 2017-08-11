using System;
using System.Linq;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;

namespace AdminEssentials.Commands
{
    public class TpAll : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "TpAll"
        };

        public override string Help => Translate("TpAll_Help");

        public override string Usage => Commands[0] + Translate("TpAll_Usage");

        public override string DefaultPermission => "adminessentials.commands.tpall";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(args.Length > 0)
            {
                if(UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer target))
                {
                    UnturnedServer.Players.ForEach((player) =>
                    {
                        if (player.Metadata.ContainsKey("pPosition"))
                            player.Metadata["pPosition"] = player.Position.Duplicate();
                        else
                            player.Metadata.Add("pPosition", player.Position.Duplicate());
                        player.Teleport(target.Position);
                    });
                    return;
                }
                Node nTarget = LevelNodes.nodes.FirstOrDefault(a => a.type == ENodeType.LOCATION && NameTool.checkNames(args[0], ((LocationNode)a).name));

                if (nTarget == null)
                {
                    UnturnedChat.SendMessage(executor, Translate("TpAll_Invalid"), ConsoleColor.Red);
                    return;
                }
                UnturnedServer.Players.ForEach((player) =>
                {
                    if (player.Metadata.ContainsKey("pPosition"))
                        player.Metadata["pPosition"] = player.Position.Duplicate();
                    else
                        player.Metadata.Add("pPosition", player.Position.Duplicate());
                    player.Teleport(nTarget.point);
                });
            }
            else
            {
                if (UnturnedPlayer.IsServer(executor))
                {
                    UnturnedChat.SendMessage(executor, Translate("TargetServer"), ConsoleColor.Red);
                    return;
                }
                UnturnedServer.Players.ForEach((player) =>
                {
                    if (player.Metadata.ContainsKey("pPosition"))
                        player.Metadata["pPosition"] = player.Position.Duplicate();
                    else
                        player.Metadata.Add("pPosition", player.Position.Duplicate());
                    player.Teleport(((UnturnedPlayer)executor).Position);
                });
            }
        }
    }
}
