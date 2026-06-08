using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BannerCollector
{
    /// <summary>
    /// Diagnostic-only auto-discovery of modded banners via the engine's banner registry,
    /// completely independent of the curated lists in BannerLoadMod. It does NOT touch
    /// BannerLoad.BannerDict or any game state - it only reports what an automatic approach
    /// would find, so the curated lists can be compared against it before relying on it.
    /// Invoke in-game with the chat command "/discoverbanners".
    /// </summary>
    internal static class BannerDiscovery
    {
        /// <summary>
        /// Banner item types of every loaded mod, grouped by mod internal name. Two passes:
        ///   1) resolve each modded NPC's banner the way the vanilla death-drop code does
        ///      (NPC -> <see cref="Item.NPCtoBanner"/> -> <see cref="Item.BannerToItem"/>).
        ///      This yields only genuine banner items and reveals which TILE types are banner
        ///      tiles;
        ///   2) any other modded item that places one of those confirmed banner tiles is also
        ///      a banner, even when its NPC link is missing or custom (e.g. Homeward Journey's
        ///      Drowned / Abyssquid share the same banner tile). Keying on a confirmed banner
        ///      tile means non-banner items are never pulled in.
        /// Vanilla banners are skipped. Pure read - changes no game state.
        /// </summary>
        public static Dictionary<string, List<int>> DiscoverModBanners()
        {
            var bannerItems = new HashSet<int>();
            var bannerTiles = new HashSet<int>();

            // Pass 1: NPC banner registry (precise) + collect each banner's tile type.
            for (int npcType = NPCID.Count; npcType < NPCLoader.NPCCount; npcType++)
            {
                if (!ContentSamples.NpcsByNetId.TryGetValue(npcType, out NPC npc))
                    continue;

                int banner = Item.NPCtoBanner(npc.BannerID());
                if (banner <= 0)
                    continue;

                int itemType = Item.BannerToItem(banner);
                if (itemType < ItemID.Count)
                    continue; // 0 (no banner) or a vanilla banner

                bannerItems.Add(itemType);
                if (ContentSamples.ItemsByType.TryGetValue(itemType, out Item bannerSample) && bannerSample.createTile >= 0)
                    bannerTiles.Add(bannerSample.createTile);
            }

            // Pass 2: sibling banners that place a confirmed banner tile but were missed above.
            if (bannerTiles.Count > 0)
            {
                for (int itemType = ItemID.Count; itemType < ItemLoader.ItemCount; itemType++)
                {
                    if (ContentSamples.ItemsByType.TryGetValue(itemType, out Item sample)
                        && sample.createTile >= 0
                        && bannerTiles.Contains(sample.createTile))
                    {
                        bannerItems.Add(itemType);
                    }
                }
            }

            // Group by owning mod.
            var result = new Dictionary<string, List<int>>();
            foreach (int itemType in bannerItems)
            {
                ModItem modItem = ItemLoader.GetItem(itemType);
                if (modItem == null)
                    continue;

                string modName = modItem.Mod?.Name ?? "Unknown";
                if (!result.TryGetValue(modName, out List<int> list))
                    result[modName] = list = new List<int>();
                list.Add(itemType);
            }

            return result;
        }

        private static string NameOf(int itemType) => ItemLoader.GetItem(itemType)?.Name ?? itemType.ToString();

        /// <summary>
        /// Writes a per-mod reconciliation of the engine banner registry (the authoritative
        /// set of real buff-granting enemy banners) against what we actually registered
        /// (<see cref="BannerLoad.BannerDict"/>):
        ///   MISSING = the game has it, our list doesn't  -> we should add it;
        ///   EXTRA   = our list has it, the game doesn't   -> not a real banner, remove it.
        /// Also lists curated names that failed Mod.TryFind. Pure diagnostic, changes nothing.
        /// </summary>
        public static void LogDiscoveredBanners()
        {
            Dictionary<string, List<int>> registry = DiscoverModBanners();

            // What we actually registered, grouped by mod (vanilla has ModName == null).
            Dictionary<string, HashSet<int>> ours = BannerLoad.BannerDict.Values
                .Where(b => b.ModName != null)
                .GroupBy(b => b.ModName)
                .ToDictionary(g => g.Key, g => new HashSet<int>(g.Select(b => b.ItemId)));

            var lines = new List<string>();
            foreach (string mod in registry.Keys.Union(ours.Keys).OrderBy(m => m))
            {
                HashSet<int> reg = registry.TryGetValue(mod, out List<int> r) ? new HashSet<int>(r) : new HashSet<int>();
                HashSet<int> have = ours.TryGetValue(mod, out HashSet<int> h) ? h : new HashSet<int>();

                List<string> missing = reg.Except(have).Select(NameOf).OrderBy(n => n).ToList();
                List<string> extra = have.Except(reg).Select(NameOf).OrderBy(n => n).ToList();

                Main.NewText($"[BannerDiscovery] {mod}: game={reg.Count} ours={have.Count} missing={missing.Count} extra={extra.Count}");

                lines.Add($"### {mod}  (game={reg.Count}  ours={have.Count})");
                lines.Add($"-- MISSING from our list, add these ({missing.Count}):");
                lines.AddRange(missing);
                lines.Add($"-- EXTRA in our list, not real banners, remove these ({extra.Count}):");
                lines.AddRange(extra);
                lines.Add(string.Empty);
            }

            // Curated names that failed Mod.TryFind during the last load (texture-vs-class
            // name mismatches that would otherwise silently drop a banner).
            lines.Add($"### SKIPPED / not found ({BannerLoad.SkippedBanners.Count})");
            lines.AddRange(BannerLoad.SkippedBanners.OrderBy(n => n));

            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "My Games", "Terraria", "tModLoader", "BannerDiscovery.txt");
            File.WriteAllLines(path, lines);
            Main.NewText($"[BannerDiscovery] written to {path}");
        }
    }

    /// <summary>Chat command "/discoverbanners" that runs <see cref="BannerDiscovery"/>.</summary>
    internal class BannerDiscoveryCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "discoverbanners";
        public override string Description => "List all modded banners auto-discovered from the NPC banner registry.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            BannerDiscovery.LogDiscoveredBanners();
        }
    }
}
