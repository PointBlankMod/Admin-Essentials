using System;
using System.Linq;
using PointBlank.API.Commands;
using PointBlank.API.Collections;
using PointBlank.API.Plugins;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;
using UnityEngine;
using CMDS = PointBlank.Commands;

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
            { "TargetServer", "Can't use command on server!" },
            { "NotSpecified", "Unspecified" },

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

            #region Teleport
            { "Teleport_Help", "This teleports the first player to the second or a location." },
            { "Teleport_Usage", " <target/node> [player]" },
            { "Teleport_Invalid", "The specified teleport location is invalid!" },
            { "Teleport_Teleport", "{0} has been teleported to {1}!" },
            #endregion

            #region Back
            { "Back_Help", "Teleports you back to the previous position before you teleported." },
            { "Back_Usage", " [player]" },
            { "Back_NoLocation", "No previous location has been stored!" },
            { "Back_Successful", "{0} has been successfully sent back!" },
            #endregion

            #region Clear Inventory
            { "ClearInventory_Help", "Clears a player's inventory" },
            { "ClearInventory_Usage", " [player]" },
            { "ClearInventory_Success", "{0}'s inventory has been cleared!" },
            #endregion

            #region Freeze
            { "Freeze_Help", "Freezes a player in place" },
            { "Freeze_Usage", " <player>" },
            { "Freeze_Unfreeze", "{0} has been unfrozen!" },
            { "Freeze_freeze", "{0} has been frozen!" },
            #endregion

            #region Kick All
            { "KickAll_Help", "Kicks all player from the server." },
            { "KickAll_Usage", " [reason]" },
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

            // Disable existing commands
            CommandManager.DisableCommand(CommandManager.GetCommand<CMDS.CommandTeleport>());

            // Hook events
            PlayerEvents.OnPlayerHurt += HurtHandler;
        }

        public override void Unload()
        {
            // Unhook events
            PlayerEvents.OnPlayerHurt -= HurtHandler;

            // Reenable existing commands
            CommandManager.EnableCommand(CommandManager.GetCommand<CMDS.CommandTeleport>());

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
            foreach(UnturnedPlayer player in UnturnedServer.Players)
                if (player.Metadata.ContainsKey("FreezePosition"))
                    player.Teleport((Vector3)player.Metadata["FreezePosition"]);
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
