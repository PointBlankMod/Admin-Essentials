using System;
using PointBlank.API.Commands;
using PointBlank.API.Collections;
using PointBlank.API.Plugins;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Zombie;
using Steamworks;
using SDG.Unturned;
using UnityEngine;
using CMDS = PointBlank.Commands;
using Typ = SDG.Unturned.Types;

namespace AdminEssentials
{
    public class AdminEssentials : PointBlankPlugin
    {
        #region Variables
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
            { "InvalidPosition", "Invalid location!" },
            { "OutVehicle", "The player is not inside a vehicle!" },

            #region GodMode
            { "GodMode_Help", "Disables all damage done to the player" },
            { "GodMode_Usage", " [player]" },
            { "GodMode_God", "You have been godded!" },
            { "GodMode_Ungod", "You have been ungodded!" },
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
            { "Freeze_Freeze", "{0} has been frozen!" },
            #endregion

            #region Kick All
            { "KickAll_Help", "Kicks all player from the server." },
            { "KickAll_Usage", " [reason]" },
            #endregion

            #region Boom
            { "Boom_Help", "Explodes a player" },
            { "Boom_Usage", " <player>" },
            { "Boom_Boom", "{0} has been exploded!" },
            #endregion

            #region Jump
            { "Jump_Help", "Teleports the player to the position they are looking at." },
            { "Jump_Usage", " [max distance]" },
            { "Jump_Distance", "Invalid distance!" },
            { "Jump_Jump", "Jumped successfully!" },
            #endregion

            #region TpAll
            { "TpAll_Help" , "Teleports all players to the specified city or player." },
            { "TpAll_Usage" , " [city] | [player]" },
            { "TpAll_Invalid", "Invalid teleport location!" },
            #endregion

            #region Kill Animals
            { "KillAnimals_Help", "Kills all animals on the server." },
            { "KillAnimals_Success", "Successfully killed all animals on server!" },
            #endregion

            #region Kill Zombies
            { "KillZombies_Help", "Kills all zombies on the server." },
            { "KillZombies_Success", "Successfully killed all zombies on the server!" },
            #endregion

            #region Max Skills
            { "MaxSkills_Help", "Maxes out all the skills on a player." },
            { "MaxSkills_Usage", " [player]" },
            { "MaxSkills_Success", "Successfully maxed out skills for {0}!" },
            #endregion

            #region Refuel Vehicle
            { "RefuelVehicle_Help", "Refuels the current vehicle." },
            { "RefuelVehicle_Success", "Vehicle successfully refueled!" },
            #endregion

            #region Repair
            { "Repair_Help", "Repairs all items in the player's inventory" },
            { "Repair_Usage", " [player]" },
            { "Repair_Success", "Successfully repaired {0}'s items!" },
            #endregion

            #region Repair Vehicle
            { "RepairVehicle_Help", "Repairs the current vehicle." },
            { "RepairVehicle_Success", "Vehicle successfully repaired!" },
            #endregion

            #region Request URL
            { "RequestURL_Help", "Opens a URL on the specified player." },
            { "RequestURL_Usage", " <URL> <message> <player>" },
            { "RequestURL_Success", "Successfully sent URl request to {0}!" },
            #endregion

            #region Sudo
            { "Sudo_Help", "Executes a message as a player." },
            { "Sudo_Usage", " <message> [player]" },
            { "Sudo_Admin", "Can't execute sudo on admins!" },
            { "Sudo_Server", "Can't execute sudo on server!" },
            { "Sudo_Self", "Can't execute sudo on yourself!" },
            { "Sudo_Success", "Message has been executed successfully!" },
            #endregion

            #region Vanish
            { "Vanish_Help", "Makes a player invisible to others." },
            { "Vanish_Usage", " [player]" },
            { "Vanish_Vanish", "You have been vanished!" },
            { "Vanish_Unvanish", "You have been unvanished!" },
            #endregion
        };

        public override ConfigurationList Configurations => new ConfigurationList() { };

        public override string Version => "1.0.1.0";

        public override string BuildURL => "http://198.245.61.226/kr4ken/pointblank/adminessentials/AdminEssentials.dll";

        public override string VersionURL => "http://198.245.61.226/kr4ken/pointblank/adminessentials/Version.txt";
        #endregion

        public override void Load()
        {
            // Set the trash
            lastRun = DateTime.Now;

            // Disable existing commands
            PointBlankCommandManager.DisableCommand(PointBlankCommandManager.GetCommand<CMDS.CommandTeleport>());

            // Hook events
            PlayerEvents.OnPlayerHurt += OnHurt;
            ServerEvents.OnPacketSent += OnPacketSent;
            ZombieEvents.OnZombieAlert += OnZombieAlert;
        }

        public override void Unload()
        {
            // Unhook events
            PlayerEvents.OnPlayerHurt -= OnHurt;
            ServerEvents.OnPacketSent -= OnPacketSent;
            ZombieEvents.OnZombieAlert -= OnZombieAlert;

            // Reenable existing commands
            PointBlankCommandManager.EnableCommand(PointBlankCommandManager.GetCommand<CMDS.CommandTeleport>());
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
            if (player == null)
                return;

            if(player.Metadata.ContainsKey("GodMode"))
                cancel = true;
        }

        private void OnZombieAlert(UnturnedZombie zombie, ref UnturnedPlayer player, ref bool cancel)
        {
            if (player.Metadata.ContainsKey("Vanish"))
                cancel = true;
        }

        private void OnPacketSent(ref CSteamID steamID, ref ESteamPacket type, ref byte[] packet, ref int size, ref int channel, ref bool cancel)
        {
            if(type == ESteamPacket.CONNECTED)
            {
                object[] info = SteamPacker.getObjects(steamID, 0, 0, packet, new Type[]
                {
                    Typ.BYTE_TYPE,
                    Typ.STEAM_ID_TYPE,
                    Typ.BYTE_TYPE,
                    Typ.STRING_TYPE,
                    Typ.STRING_TYPE,
                    Typ.VECTOR3_TYPE,
                    Typ.BYTE_TYPE,
                    Typ.BOOLEAN_TYPE,
                    Typ.BOOLEAN_TYPE,
                    Typ.INT32_TYPE,
                    Typ.STEAM_ID_TYPE,
                    Typ.STRING_TYPE,
                    Typ.BYTE_TYPE,
                    Typ.BYTE_TYPE,
                    Typ.BYTE_TYPE,
                    Typ.COLOR_TYPE,
                    Typ.COLOR_TYPE,
                    Typ.BOOLEAN_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_TYPE,
                    Typ.INT32_ARRAY_TYPE,
                    Typ.BYTE_TYPE,
                    Typ.STRING_TYPE
                });
                UnturnedPlayer player = UnturnedPlayer.Get((CSteamID)info[1]);

                if (player.Metadata.ContainsKey("Vanish"))
                    cancel = true;
            }
        }
        #endregion
    }
}
