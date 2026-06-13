using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BannerCollector
{
    /// <summary>
    /// Adds vanilla-style shift-click quick-deposit of mod banners into the collection window.
    /// <para/>
    /// Terraria routes every shift-click on an item slot through <see cref="ModPlayer.ShiftClickSlot"/>
    /// before its default action (quick-stack to chest / move to inventory) runs. We reuse
    /// that exact entry point so the behaviour matches vanilla and nothing in the existing click flow is
    /// altered. The deposit is only armed while the collection window is open; with the window closed every
    /// shift-click stays purely vanilla.
    /// <para/>
    /// While the window is open, mod banners are routed as follows:
    /// <list type="bullet">
    /// <item><b>inventory slot, no container open</b> — banner is pulled into the collection;</item>
    /// <item><b>inventory slot, container open</b> — left to vanilla, so it quick-stacks into the open
    /// chest/bank exactly like vanilla (chest takes priority over the collection);</item>
    /// <item><b>container slot (chest or bank)</b> — banner is pulled straight into the collection instead
    /// of vanilla's "move to inventory".</item>
    /// </list>
    /// Every other slot kind (coins, ammo, shop, crafting, trash, equipment...) and every non-banner item
    /// returns <c>false</c>, leaving Terraria's default handling untouched.
    /// <para/>
    /// An open Magic Storage (when that mod is installed) counts as "a container is open" via
    /// <see cref="MagicStorageBridge"/>, so it behaves just like a vanilla chest here.
    /// </summary>
    internal class PlayerShiftClick : ModPlayer
    {
        /// <summary>Per-banner storage cap, matching the limit enforced everywhere else in the mod.</summary>
        private const int MaxBannerCount = 9999;

        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            // Quick-deposit is only armed while the collection window is open; otherwise stay fully vanilla.
            BannerUI ui = BannerUISystem.Instance?.bannerUI;
            if (ui == null || !ui.BannerCollectorVisible)
                return false;

            // Only banners that belong to this mod are ever collected.
            Item item = inventory[slot];
            if (item == null || item.IsAir || !BannerLoad.BannerDict.ContainsKey(item.type))
                return false;

            switch (context)
            {
                // A banner in the player's inventory.
                case ItemSlot.Context.InventoryItem:
                    // An open storage takes priority over the collection, exactly like vanilla. A vanilla
                    // chest/bank is left to vanilla's quick-stack; an open Magic Storage is left to that mod's
                    // own shift-click deposit. Only with nothing open is the banner collected.
                    if (Player.chest != -1 || MagicStorageBridge.IsStorageOpen())
                        return false;
                    break;

                // A banner inside the open container (world chest = ChestItem, personal bank = BankItem):
                // pull it straight into the collection instead of vanilla's "move to inventory".
                case ItemSlot.Context.ChestItem:
                case ItemSlot.Context.BankItem:
                    break;

                // Coins, ammo, shop, crafting, trash, equipment, ... stay vanilla.
                default:
                    return false;
            }

            // Move as much of the stack as the per-banner cap allows into the collection.
            BannerInfo banner = BannerLoad.BannerDict[item.type];
            int free = MaxBannerCount - banner.BannerCount;
            if (free > 0)
            {
                int moved = Math.Min(item.stack, free);
                banner.BannerCount += moved;
                item.stack -= moved;
                if (item.stack <= 0)
                    item.TurnToAir();
                SoundEngine.PlaySound(SoundID.Grab);

                // Pulling from a world chest mutates a synced array, so the change must be broadcast (the
                // personal banks have a negative Player.chest and are client-local, needing no sync). Vanilla
                // would have synced this itself, but we have blocked its default handling by returning true.
                if (Player.chest >= 0 && Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendData(MessageID.SyncChestItem, -1, -1, null, Player.chest, slot);
            }

            // For a mod banner with the window open we own this click: block vanilla's default action even
            // when the collection is already full, so the banner is never accidentally moved or trashed.
            return true;
        }
    }

    // =====================================================================================================
    //  Magic Storage integration (optional weakReferences dependency)
    // -----------------------------------------------------------------------------------------------------
    //  Kept in this file because it is part of the same shift-click storage logic, just separated from the
    //  vanilla-slot handling above. Treats an open Magic Storage as "a container is open" so banners route
    //  into it exactly like a vanilla chest.
    // =====================================================================================================

    /// <summary>
    /// Optional integration with the Magic Storage mod. When a Magic Storage interface is open (a storage
    /// heart reached through a Storage Access or Crafting Access), the collection treats it as "a storage is
    /// open" exactly like a vanilla chest, so banner shift-clicks deposit into it.
    /// <para/>
    /// Magic Storage is a <c>weakReferences</c> dependency, so this mod loads with or without it. Every
    /// member that touches a Magic Storage type is isolated in a separate, non-inlined method reached only
    /// after <see cref="Available"/> (a cached <see cref="ModLoader.HasMod(string)"/> check) is confirmed
    /// true; the JIT therefore never resolves the foreign types when Magic Storage is absent.
    /// </summary>
    internal static class MagicStorageBridge
    {
        private static bool? available;

        /// <summary>True when the Magic Storage mod is loaded. Cached; touches no Magic Storage type.</summary>
        public static bool Available => available ??= ModLoader.HasMod("MagicStorage");

        /// <summary>
        /// Whether the local player currently has a depositable Magic Storage open. Safe to call
        /// unconditionally: it short-circuits to false when the mod is absent before any foreign type is
        /// touched.
        /// </summary>
        public static bool IsStorageOpen() => Available && IsStorageOpenInternal();

        /// <summary>
        /// Deposits up to <paramref name="count"/> banners of <paramref name="itemType"/> into the open
        /// Magic Storage, returning how many were actually stored (0 if the mod is absent, none is open, or
        /// nothing fit). Safe to call unconditionally.
        /// </summary>
        public static int Deposit(int itemType, int count)
            => Available && count > 0 ? DepositInternal(itemType, count) : 0;

        // --- The methods below reference Magic Storage types and run only when Available is true. ---

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool IsStorageOpenInternal()
            => MagicStorage.StoragePlayer.LocalPlayer.GetStorageHeart() != null;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int DepositInternal(int itemType, int count)
        {
            var heart = MagicStorage.StoragePlayer.LocalPlayer.GetStorageHeart();
            if (heart == null)
                return 0;

            Item item = new Item(itemType) { stack = count };

            // Mirror Magic Storage's own shift-click deposit, including its access-security context.
            using (MagicStorage.Common.Systems.SecuritySystem.CreateAccessContext())
                heart.TryDeposit(item);

            // Single-player: TryDeposit lowers item.stack by the amount that fit. Multiplayer: it serialises
            // the whole stack to the server and clears the item, so the client optimistically counts it all
            // as stored - identical to Magic Storage's own shift-click behaviour.
            int leftover = item.IsAir ? 0 : item.stack;
            return count - leftover;
        }
    }

    // =====================================================================================================
    //  Magic Storage withdraw -> collection (the reverse direction)
    // -----------------------------------------------------------------------------------------------------
    //  Magic Storage's storage grid is its own UI (not vanilla item slots), so ModPlayer.ShiftClickSlot
    //  never sees it. Its shift-withdraw funnels the banner into the player via Player.GetItem
    //  (StorageUIState: "Main.mouseItem = player.GetItem(...)"). We attach a single, tightly-gated detour
    //  on that vanilla method to divert a shift-withdrawn banner into the collection instead.
    //
    //  Carefulness:
    //   * The detour is attached ONLY when Magic Storage is installed, so without it the mod is byte-for-
    //     byte unchanged (no hook at all). tModLoader auto-removes MonoModHooks on unload.
    //   * Every GetItem call that is not exactly "local player shift-withdrawing a mod banner while both the
    //     Magic Storage and the collection window are open, with room left" passes straight through to orig.
    // =====================================================================================================

    /// <summary>
    /// Installs the optional Magic Storage withdraw redirect. See the banner above for the rationale.
    /// </summary>
    internal class MagicStorageWithdrawHook : ModSystem
    {
        private const int MaxBannerCount = 9999;

        private delegate Item GetItemFn(Player self, int plr, Item newItem, GetItemSettings settings);

        public override void Load()
        {
            // No Magic Storage -> no hook, no overhead, nothing to break.
            if (!MagicStorageBridge.Available)
                return;

            MethodInfo getItem = typeof(Player).GetMethod(nameof(Player.GetItem),
                new[] { typeof(int), typeof(Item), typeof(GetItemSettings) });
            MonoModHooks.Add(getItem, OnGetItem);
        }

        private static Item OnGetItem(GetItemFn orig, Player self, int plr, Item newItem, GetItemSettings settings)
        {
            if (self.whoAmI == Main.myPlayer
                && newItem is { IsAir: false }
                && BannerLoad.BannerDict.TryGetValue(newItem.type, out BannerInfo banner)
                && (Main.keyState.IsKeyDown(Keys.LeftShift) || Main.keyState.IsKeyDown(Keys.RightShift))
                && BannerUISystem.Instance?.bannerUI?.BannerCollectorVisible == true
                && MagicStorageBridge.IsStorageOpen())
            {
                int free = MaxBannerCount - banner.BannerCount;
                if (free > 0)
                {
                    int moved = Math.Min(newItem.stack, free);
                    banner.BannerCount += moved;
                    newItem.stack -= moved;
                    if (newItem.stack <= 0)
                        return new Item(); // fully collected; nothing returns to the inventory
                }
            }

            return orig(self, plr, newItem, settings);
        }
    }
}
