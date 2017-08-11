using System.Linq;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Unturned.Server;
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

        #endregion

        public override void Execute(PointBlankPlayer executor, string[] args)
        {
            if(UnturnedServer.Players.Where(i => i.CharacterName == args[0]).FirstOrDefault() != null)
            {
                foreach(var player in UnturnedServer.Players)
                {
                    player.Teleport(UnturnedServer.Players.Where(i => i.CharacterName == args[0]).FirstOrDefault().Position);
                }
            }
            else if(LevelNodes.nodes.Find(i => ((LocationNode)i).name.ToLower() == args[0].ToLower()) != null)
            {
                foreach(var player in UnturnedServer.Players)
                {
                    player.Teleport(LevelNodes.nodes.Find(i => ((LocationNode)i).name.ToLower() == args[0].ToLower()).point);
                }
            }
        }
    }
}
