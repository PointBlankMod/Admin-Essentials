using System;
using PointBlank.API.Collections;
using PointBlank.API.Plugins;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;

namespace AdminEssentials
{
    public class AdminEssentials : Plugin
    {
        #region Variables
        private PlayerEvents.PlayerHurtHandler HurtHandler = null;

        private DateTime lastRun;
        #endregion

        #region Properties
        
        public override TranslationList Translations => new TranslationList()
        {
            { "PlayerNotFound", "The specified player has not been found!" },
            { "FailServer", "Please target a specific player!" },
            { "InVehicle", "The player is inside a vehicle!" },

            #region GodMode
            { "GodMode_Help", "Disables all damage done to the player" },
            { "GodMode_Usage", " [player]" },
            { "GodMode_Success", "You have been godmodded!" },
            #endregion

            #region Broadcast
            { "Broadcast_Help", "Broadcasts a message to the entire server" },
            { "Broadcast_Usage", " <message>" },
            #endregion

            #region Heal
            { "Heal_Help", "Heal a specified player or yourself" },
            { "Heal_Usage", " [player]" },
            { "Heal_Success", "The player {0} has been healed!" },
            #endregion

            #region TpHere
            { "TpHere_Help", "Teleports a player to you" },
            { "TpHere_Usage", " <player>" },
            #endregion
        };

        public override ConfigurationList Configurations => new ConfigurationList() { };

        public override string Version => "1.0.0.0";

        public override string BuildURL => "";

        public override string VersionURL => "";
        
        #endregion

        public override void Load()
        {
            // Set the trash
            HurtHandler = new PlayerEvents.PlayerHurtHandler(OnHurt);
            lastRun = DateTime.Now;

            // Hook events
            PlayerEvents.OnPlayerHurt += HurtHandler;
        }

        public override void Unload()
        {
            // Unhook events
            PlayerEvents.OnPlayerHurt -= HurtHandler;

            // Remove the trash
            HurtHandler = null;
        }

        #region Mono Functions
        void FixedUpdate()
        {
            if((DateTime.Now - lastRun).TotalMilliseconds > 2000)
            {
                for(int i = 0; i < UnturnedServer.Players.Length; i++)
                    if (UnturnedServer.Players[i].Metadata.ContainsKey("GodMode"))
                        UnturnedServer.Players[i].Life.sendRevive();
                lastRun = DateTime.Now;
            }
        }
        #endregion

        #region Event Functions
        private void OnHurt(UnturnedPlayer player, ref byte damage, ref EDeathCause cause, ref ELimb limb, ref UnturnedPlayer damager, ref bool cancel)
        {
            if(player.Metadata.ContainsKey("GodMode"))
                cancel = true;
        }
        #endregion
    }
}
