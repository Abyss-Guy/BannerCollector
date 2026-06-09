using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            lines.Add(string.Empty);

            // Health report: verifies that every banner WE registered actually grants a working
            // buff on the correct enemy, using the exact index the buff tile sets at runtime.
            AppendModHealthReport(lines);

            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "My Games", "Terraria", "tModLoader", "BannerDiscovery.txt");
            File.WriteAllLines(path, lines);
            Main.NewText($"[BannerDiscovery] written to {path}");
        }

        /// <summary>
        /// Appends a health report that verifies every banner WE registered
        /// (<see cref="BannerLoad.BannerDict"/>) actually grants a working nearby-banner buff on
        /// the correct enemy. It is grounded entirely in engine data - for each banner it computes
        /// the EXACT index the buff tile writes at runtime
        /// (<see cref="BannerInfo.BannerId"/>) and validates it:
        ///   OK proven - <see cref="Item.BannerToItem"/> of the index is this exact banner item,
        ///             so the buff provably protects against this banner's own enemy;
        ///   OK name - the index points at a real enemy whose name matches the banner;
        ///   CHECK   - the index points at a real enemy whose name does NOT match (shown to verify);
        ///   BROKEN  - the index is a number but no enemy maps to it (grants nothing).
        /// When the index does not resolve, the mod can grant no buff for that banner:
        ///   FIXABLE    - the same enemy has a banner under a DIFFERENT item id (a wrong id to fix);
        ///   UNRESOLVED - nothing maps it and its banner tile is shared by several enemies, so the
        ///                enemy is ambiguous from engine data; flagged for an in-game check, not a
        ///                guess. Single-enemy-tile items are resolved upstream and never land here.
        /// Nothing here guesses: it reports what the engine returns.
        /// </summary>
        private static void AppendModHealthReport(List<string> lines)
        {
            Dictionary<int, int> bannerToNpc = BuildBannerToNpc();
            int buffArrayLength = Main.SceneMetrics.NPCBannerBuff.Length;

            // Normalised enemy name -> its canonical banner item, so a NO BUFF banner can be told
            // apart: the enemy has a real banner under a different item id (wrong id, fixable) or
            // the enemy has no banner at all (the entry is bogus and should be removed).
            var enemyToBannerItem = new Dictionary<string, int>();
            foreach (KeyValuePair<int, int> pair in bannerToNpc)
            {
                string enemyKey = NameKey(EnemyName(pair.Value));
                if (!enemyToBannerItem.ContainsKey(enemyKey))
                    enemyToBannerItem[enemyKey] = Item.BannerToItem(pair.Key);
            }

            var detail = new List<string>();
            // How many distinct banner numbers sit on each banner tile, so an unresolved banner can
            // be described accurately (a tile shared by many enemies is ambiguous; a tile with one
            // is single-enemy and would already have resolved).
            var tileBannerCount = new Dictionary<int, HashSet<int>>();
            foreach (KeyValuePair<int, int> pair in bannerToNpc)
            {
                int it = Item.BannerToItem(pair.Key);
                if (it > 0 && ContentSamples.ItemsByType.TryGetValue(it, out Item s) && s.createTile >= 0)
                {
                    if (!tileBannerCount.TryGetValue(s.createTile, out HashSet<int> set))
                        tileBannerCount[s.createTile] = set = new HashSet<int>();
                    set.Add(pair.Key);
                }
            }

            int total = 0, proven = 0, nameOk = 0, check = 0, broken = 0, fixable = 0, unresolved = 0;

            foreach (BannerInfo banner in BannerLoad.BannerDict.Values
                .OrderBy(b => b.ModName ?? "Terraria", StringComparer.Ordinal)
                .ThenBy(b => b.ItemName, StringComparer.Ordinal))
            {
                total++;
                string mod = banner.ModName ?? "Terraria";
                string itemName = Lang.GetItemNameValue(banner.ItemId);

                // The exact index BannerBuffTile.NearbyEffects writes for this banner.
                int buffIndex = banner.ModName == null ? banner.Index - 21 : banner.NpcType;

                if (buffIndex < 0 || buffIndex >= buffArrayLength)
                {
                    string expected = StripBannerSuffix(itemName);

                    // The mod could not resolve a banner number for this item (not in the registry,
                    // and not on a single-enemy tile). Report it honestly instead of guessing:
                    //   FIXABLE    - the same enemy DOES have a banner under a different item id;
                    //   UNRESOLVED - the mod grants no buff for it; its banner tile is shared by
                    //                several enemies, so which enemy it belongs to is ambiguous from
                    //                engine data alone. Verify in-game before changing anything.
                    if (enemyToBannerItem.TryGetValue(NameKey(expected), out int realItem) && realItem != banner.ItemId)
                    {
                        fixable++;
                        detail.Add($"[FIXABLE]    {mod} | {itemName}: engine has a '{expected}' banner at item id {realItem} ('{Lang.GetItemNameValue(realItem)}'); our item id {banner.ItemId} is wrong - fix the id");
                    }
                    else
                    {
                        unresolved++;
                        Item sample = ContentSamples.ItemsByType.TryGetValue(banner.ItemId, out Item s2) ? s2 : null;
                        int tile = sample?.createTile ?? -1;
                        int onTile = tile >= 0 && tileBannerCount.TryGetValue(tile, out HashSet<int> set) ? set.Count : 0;
                        detail.Add($"[UNRESOLVED] {mod} | {itemName} (item {banner.ItemId}): mod grants no buff; banner tile {tile} carries {onTile} different banners (ambiguous). Place it in-game and read the buff tooltip - if it grants nothing it is not a real banner.");
                    }
                    continue;
                }
                if (!bannerToNpc.TryGetValue(buffIndex, out int npcType))
                {
                    broken++;
                    detail.Add($"[BROKEN]  {mod} | {itemName}: buff #{buffIndex} is not a real banner -> buff does nothing");
                    continue;
                }

                if (Item.BannerToItem(buffIndex) == banner.ItemId)
                {
                    proven++; // round-trips to this exact banner item: provably correct enemy
                    continue;
                }

                string enemy = EnemyName(npcType);
                if (NameKey(StripBannerSuffix(itemName)) == NameKey(enemy))
                {
                    nameOk++;
                    continue;
                }

                check++;
                detail.Add($"[CHECK]   {mod} | {itemName}: buff protects against '{enemy}' (#{buffIndex}); banner name suggests '{StripBannerSuffix(itemName)}'");
            }

            int directBuff = proven + nameOk + check;
            lines.Add("### MOD HEALTH REPORT  (does every banner WE register buff the right enemy?)");
            lines.Add($"total registered banners : {total}");
            lines.Add($"OK proven                : {proven}   (buff round-trips to this banner's own enemy)");
            lines.Add($"OK name-match            : {nameOk}   (buff hits an enemy whose name matches the banner)");
            lines.Add($"CHECK                    : {check}   (buff hits a real enemy, name differs - verify below)");
            lines.Add($"FIXABLE                  : {fixable}   (the enemy has a banner under a different item id - fix the id)");
            lines.Add($"BROKEN                   : {broken}   (buff index is a number with no enemy - grants nothing)");
            lines.Add($"UNRESOLVED               : {unresolved}   (mod grants no buff; ambiguous tile - verify in-game before deciding)");
            lines.Add($"=> working banners: {directBuff} / {total};  need attention: {fixable + broken + unresolved}");
            lines.Add(string.Empty);
            lines.Add("Banners not directly proven (CHECK = works, verify enemy; FIXABLE = fix id; UNRESOLVED = check in-game; BROKEN = wrong number):");
            if (detail.Count == 0)
                lines.Add("  (none - every registered banner grants a buff that lands on the matching enemy)");
            else
                lines.AddRange(detail);
        }

        /// <summary>
        /// Maps each engine banner number to a representative NPC net id, built from the game's own
        /// association (<see cref="Item.NPCtoBanner"/> over EVERY net id, including the negative-id
        /// variant NPCs - coloured slimes, Pinky, etc. - that a 0..NPCCount walk would skip).
        /// Read-only.
        /// </summary>
        private static Dictionary<int, int> BuildBannerToNpc()
        {
            var map = new Dictionary<int, int>();
            foreach (KeyValuePair<int, NPC> pair in ContentSamples.NpcsByNetId)
            {
                int banner = Item.NPCtoBanner(pair.Value.BannerID());
                if (banner > 0 && !map.ContainsKey(banner))
                    map[banner] = pair.Key;
            }
            return map;
        }

        /// <summary>Display name of an NPC by net id, correct for negative-id variants too.</summary>
        private static string EnemyName(int netId)
            => ContentSamples.NpcsByNetId.TryGetValue(netId, out NPC npc) ? npc.TypeName : netId.ToString();

        /// <summary>Removes a trailing "Banner" word from a display name (e.g. "Blue Slime Banner" -> "Blue Slime").</summary>
        private static string StripBannerSuffix(string name)
        {
            name = name.Trim();
            if (name.EndsWith("Banner", StringComparison.OrdinalIgnoreCase))
                name = name.Substring(0, name.Length - "Banner".Length).Trim();
            return name;
        }

        /// <summary>Normalises a name to letters/digits only, lower-case, for tolerant comparison.</summary>
        private static string NameKey(string s)
        {
            var sb = new StringBuilder(s.Length);
            foreach (char c in s)
                if (char.IsLetterOrDigit(c))
                    sb.Append(char.ToLowerInvariant(c));
            return sb.ToString();
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
