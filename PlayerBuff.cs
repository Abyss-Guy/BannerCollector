using BannerCollector.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace BannerCollector
{
    internal class PlayerBuff : ModPlayer
    {
        private int tileUpdateTimer = 0; // 타일 업데이트를 위한 타이머
                                         // 
        public override void PreUpdate()
        {
            if (BannerCollectorConfig.Instance.EnableBannerBuff)
            {
                Player player = Main.LocalPlayer;
                tileUpdateTimer++;

                // 60 프레임마다 (1초에 한 번) 타일 업데이트
                if (tileUpdateTimer >= 30)
                {
                    tileUpdateTimer = 0; // 타이머 초기화
                    BannerBuffTile.UpdateTilePosition(player); // 타일 업데이트
                }
            }
        }

        /// <summary>
        /// Toggles the banner buff when the mod's keybind is pressed. The buff state lives in the
        /// client-side config and is read live by <see cref="BannerBuffTile"/>, so flipping it here
        /// takes effect immediately. <see cref="ProcessTriggers"/> only runs for the local player's
        /// input, which matches the client-side scope of the setting.
        /// </summary>
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (BannerCollector.ToggleBannerBuffKeybind == null || !BannerCollector.ToggleBannerBuffKeybind.JustPressed)
                return;

            BannerCollectorConfig config = BannerCollectorConfig.Instance;
            if (config == null)
                return;

            config.EnableBannerBuff = !config.EnableBannerBuff;
            config.Save(); // persist so the keybind toggle survives a restart
            Main.NewText($"Banner buffs: {(config.EnableBannerBuff ? "ON" : "OFF")}");
        }
    }
}
