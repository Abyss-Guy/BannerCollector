using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace BannerCollector
{
    /// <summary>
    /// Makes the collection window a true top-most panel: while the cursor is over it, no click, hover or
    /// scroll reaches anything drawn behind it - vanilla item slots, the loadout / map / accessory HUD,
    /// other mods' panels, or the game world.
    /// <para/>
    /// Every interactive HUD element hit-tests the cursor through <see cref="Main.mouseX"/> /
    /// <see cref="Main.mouseY"/> (directly, or via <see cref="Main.MouseScreen"/>); none of them consult a
    /// "something is on top" flag (verified by decompiling tModLoader - the whole of <c>Main.DrawInventory</c>,
    /// the loadout buttons and the map all branch on raw <c>mouseX</c>/<c>mouseY</c> rectangle tests). So the
    /// one universal lever is the cursor position, blocked at the single choke point of each frame phase that
    /// reads it - no per-element code:
    /// <list type="bullet">
    /// <item><b>Draw phase</b> - all vanilla HUD, vanilla item slots and every mod
    /// <see cref="LegacyGameInterfaceLayer"/>. The cursor is pushed off-screen for the layers drawn BELOW the
    /// window and restored right before the window (and the vanilla cursor) draw. Crucially the offset is
    /// applied at the zoom source (<c>PlayerInput._originalMouseX/Y</c>), not just <see cref="Main.mouseX"/>:
    /// the interface draw re-derives <c>Main.mouseX</c> from that source on every <c>SetZoom_*</c> call (e.g.
    /// the map layer calls <c>SetZoom_UI</c> mid-draw), so offsetting only <see cref="Main.mouseX"/> would be
    /// wiped the next time any layer zooms. The source is re-cached from the real cursor once per frame in the
    /// update phase, never during the draw, so the offset safely holds for the whole layer stack.</item>
    /// <item><b>Update phase</b> - every other element-based interface, i.e. essentially all modded
    /// <see cref="UIState"/> panels. <see cref="UserInterface.Update"/> is detoured and, for any interface but
    /// this mod's, the cursor is pushed off-screen for the duration of the update (no <c>SetZoom</c> runs
    /// inside it, so a plain <see cref="Main.mouseX"/> offset is enough).</item>
    /// <item><b>World interaction</b> - item use, mining, placing - reads <see cref="Player.mouseInterface"/>,
    /// which <see cref="BannerUI.Update"/> raises while the cursor is over the window.</item>
    /// </list>
    /// All three share the <see cref="OverWindow"/> guard, which is effectively free while the window is
    /// closed. tModLoader auto-removes MonoMod hooks on unload, so no teardown is needed.
    /// </summary>
    internal class BlockClickThroughModWindow : ModSystem
    {
        // Any negative value misses every on-screen rectangle; SetZoom only ever scales it by a positive
        // factor, so it stays off-screen at every UI scale.
        private const int OffScreen = -10000;

        private delegate void UserInterfaceUpdate(UserInterface self, GameTime gameTime);

        // The private source the zoom system restores Main.mouseX/Y from on every SetZoom call during the
        // interface draw. Blinding must hit this, not just Main.mouseX/Y (see the class summary).
        private static FieldInfo originalMouseX;
        private static FieldInfo originalMouseY;

        public override void Load()
        {
            if (Main.dedServ)
                return; // no local UI on a server

            originalMouseX = typeof(PlayerInput).GetField("_originalMouseX", BindingFlags.NonPublic | BindingFlags.Static);
            originalMouseY = typeof(PlayerInput).GetField("_originalMouseY", BindingFlags.NonPublic | BindingFlags.Static);

            MethodInfo update = typeof(UserInterface).GetMethod(
                nameof(UserInterface.Update), BindingFlags.Public | BindingFlags.Instance,
                null, new[] { typeof(GameTime) }, null);

            if (update != null)
                MonoModHooks.Add(update, BlockOtherInterfaces);
        }

        // ---- Draw phase: blind every HUD layer drawn below the window ------------------------------

        private static bool drawBlinded;
        private static int savedMouseX, savedMouseY, savedOriginalX, savedOriginalY;

        /// <summary>
        /// Runs as the first interface layer of the frame. While the cursor is over the window it stashes
        /// the real cursor position (both the live <see cref="Main.mouseX"/>/Y and the zoom source) and pushes
        /// the cursor off-screen, so every layer that follows hit-tests nothing. Clears its own flag first,
        /// so a frame whose restore was skipped (e.g. an earlier layer aborted the draw) cannot leave the
        /// cursor stuck - the zoom source is re-cached from the real cursor next frame regardless.
        /// </summary>
        public static void BlindBelowWindow()
        {
            drawBlinded = false;
            if (originalMouseX == null || originalMouseY == null || !OverWindow())
                return;

            savedMouseX = Main.mouseX;
            savedMouseY = Main.mouseY;
            savedOriginalX = (int)originalMouseX.GetValue(null);
            savedOriginalY = (int)originalMouseY.GetValue(null);

            Main.mouseX = Main.mouseY = OffScreen;
            originalMouseX.SetValue(null, OffScreen);
            originalMouseY.SetValue(null, OffScreen);
            drawBlinded = true;
        }

        /// <summary>
        /// Restores the real cursor position (live and zoom source). Called at the start of the window's own
        /// draw layer, so the window and everything above it (tooltips, the vanilla cursor) draw at the true
        /// position.
        /// </summary>
        public static void RestoreCursor()
        {
            if (!drawBlinded)
                return;

            Main.mouseX = savedMouseX;
            Main.mouseY = savedMouseY;
            originalMouseX.SetValue(null, savedOriginalX);
            originalMouseY.SetValue(null, savedOriginalY);
            drawBlinded = false;
        }

        // ---- Update phase: blind every other element-based interface -------------------------------

        /// <summary>
        /// Wraps every <see cref="UserInterface.Update"/>. The mod's own interface always runs as normal.
        /// For any OTHER interface, while the cursor is over the window, the cursor is moved off-screen for
        /// the duration of the update only, so the interface hit-tests nothing and raises no hover or click;
        /// the real position is restored in the <c>finally</c>, so nothing outside the call observes it and
        /// the interface still updates (animations, layout) as usual.
        /// </summary>
        private static void BlockOtherInterfaces(UserInterfaceUpdate orig, UserInterface self, GameTime gameTime)
        {
            if (self == BannerUISystem.Instance?.BannerInterface || !OverWindow())
            {
                orig(self, gameTime);
                return;
            }

            int mouseX = Main.mouseX, mouseY = Main.mouseY;
            Main.mouseX = Main.mouseY = OffScreen;
            try
            {
                orig(self, gameTime);
            }
            finally
            {
                Main.mouseX = mouseX;
                Main.mouseY = mouseY;
            }
        }

        // ---- Shared guard -------------------------------------------------------------------------

        /// <summary>
        /// True while the cursor is over the open collection window (or its open dropdown). Guarded so the
        /// block only applies with the inventory open, the in-game options window closed and the game
        /// focused, so everything keeps working normally in every other situation.
        /// </summary>
        private static bool OverWindow()
        {
            if (!Main.playerInventory || Main.ingameOptionsWindow || !Main.hasFocus)
                return false;

            BannerUI ui = BannerUISystem.Instance?.bannerUI;
            return ui != null && ui.IsMouseOverWindow();
        }
    }
}
