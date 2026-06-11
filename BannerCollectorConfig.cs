using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using Terraria.ModLoader.Config;

namespace BannerCollector
{
    internal class BannerCollectorConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Label("Enable banner buffs in collection")]
        [DefaultValue(true)]
        public bool EnableBannerBuff { get; set; }

        // Locks dragging of the mod window. Listed before the X/Y fields for convenience. The window
        // keeps its saved position while locked - only "Reset to Defaults" returns it to the start.
        [DefaultValue(true)]
        public bool LockWindowPosition { get; set; }

        // Saved top-left position of the mod window, in UI pixels. Updated when the window is dragged
        // and persisted via Save(); the defaults are the window's original spot. The Range spans any
        // screen size so the value shows correctly in the menu instead of being clamped by the
        // default 0-100 slider.
        [Range(0, 5000)]
        [Increment(1)]
        [DefaultValue(20)]
        public int WindowX { get; set; }

        [Range(0, 5000)]
        [Increment(1)]
        [DefaultValue(258)]
        public int WindowY { get; set; }

        // Remembered open/closed state of the collection toggle. Persisted but hidden from the config
        // menu: [JsonProperty] makes it serialize, and being non-public keeps it out of the menu
        // (which lists only public members). Managed automatically on world load/exit.
        [JsonProperty]
        internal bool CollectionOpen;

        // 정적 인스턴스
        public static BannerCollectorConfig Instance { get; private set; }

        // Cached handle to tModLoader's internal ConfigManager.Save(ModConfig). Resolved through
        // the assembly so it works regardless of the type's accessibility, and stays null if the
        // API ever changes - in which case Save() is a harmless no-op instead of throwing.
        private static readonly MethodInfo SaveMethod = typeof(ModConfig).Assembly
            .GetType("Terraria.ModLoader.Config.ConfigManager")
            ?.GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic, null, new[] { typeof(ModConfig) }, null);

        // 인스턴스 초기화
        public override void OnLoaded()
        {
            Instance = this; // 설정이 로드될 때 인스턴스를 초기화
        }

        /// <summary>
        /// Writes the current config values to disk, so a change made outside the config menu
        /// (the buff-toggle keybind) survives a restart. This is the same save the config menu
        /// performs when it closes; it only writes the file and does not trigger a reload.
        /// Persistence is best-effort: any failure is swallowed so it can never break the toggle.
        /// </summary>
        public void Save()
        {
            try { SaveMethod?.Invoke(null, new object[] { this }); }
            catch { /* best-effort: a failed save must not interrupt gameplay */ }
        }
    }
}
