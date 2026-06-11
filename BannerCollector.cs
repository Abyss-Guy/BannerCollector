using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using System.Windows.Forms;

namespace BannerCollector
{
    public class BannerCollector : Mod
    {
        /// <summary>
        /// Keybind that toggles the banner buff on/off - the same setting as
        /// <see cref="BannerCollectorConfig.EnableBannerBuff"/>. Registered here so it appears in
        /// Settings &gt; Controls and is handled in <see cref="PlayerBuff.ProcessTriggers"/>.
        /// Unbound by default (no key) so it never clashes with anything; assign a key in Controls.
        /// </summary>
        public static ModKeybind ToggleBannerBuffKeybind;

        public override void Load()
        {
            ToggleBannerBuffKeybind = KeybindLoader.RegisterKeybind(this, "ToggleBannerBuffs", "None");
        }

        public override void Unload()
        {
            // Drop the static reference so the mod unloads without leaking it.
            ToggleBannerBuffKeybind = null;
        }
    }
}
