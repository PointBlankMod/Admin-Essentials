using PointBlank.API.Collections;
using PointBlank.API.Plugins;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;

namespace AdminEssentials
{
    public class AdminEssentials : Plugin
    {
        #region Variables
        
        private PlayerEvents.PlayerHurtHandler HurtHandler = null;
        
        #endregion

        #region Properties
        
        public override TranslationList Translations => new TranslationList()
        {
            { "PlayerNotFound", "The specified player has not been found!" },
            { "FailServer", "Please target a specific player!" },
            { "GodMode_Help", "Disables all damage done to the player" },
            { "GodMode_Usage", " [player]" },
            { "GodMode_Success", "You have been godmodded!" }
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

        #region Event Functions
        
        private void OnHurt(UnturnedPlayer player, ref byte damage, ref EDeathCause cause, ref ELimb limb, ref UnturnedPlayer damager, ref bool cancel)
        {
            if(player.Metadata.ContainsKey("GodMode"))
                cancel = true;
        }
        
        #endregion
    }
}
