using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using UnityEngine;

namespace AdminEssentials.Commands
{
    public class Jump : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "jump"
        };

        public override string Help => Translate("Jump_Help");

        public override string Usage => Commands[0] + Translate("Jump_Usage");

        public override string DefaultPermission => "adminessentials.commands.jump";

        public override EAllowedCaller AllowedCaller => EAllowedCaller.PLAYER;

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            float distance = Mathf.Infinity;
            UnturnedPlayer player = (UnturnedPlayer)executor;

            if(args.Length > 0)
            {
                if(!float.TryParse(args[0], out distance))
                {
                    UnturnedChat.SendMessage(executor, Translate("Jump_Distance"), ConsoleColor.Red);
                    return;
                }
            }
            Vector3? position = player.GetEyePosition(distance);

            if (!position.HasValue)
            {
                UnturnedChat.SendMessage(executor, Translate("InvalidPosition"), ConsoleColor.Red);
                return;
            }
            Vector3 pos = position.Value;
            pos.y += 6f;

            player.Metadata.Add("pPosition", player.Position.Duplicate());
            player.Teleport(pos);
            UnturnedChat.SendMessage(executor, Translate("Jump_Jump"), ConsoleColor.Green);
        }
    }
}
