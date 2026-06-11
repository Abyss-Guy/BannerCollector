using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Terraria.ModLoader.Config;

namespace BannerCollector
{
    internal class BannerCollectorConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Label("Enable banner buffs in collection")]
        [DefaultValue(true)]
        public bool EnableBannerBuff { get; set; }

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
