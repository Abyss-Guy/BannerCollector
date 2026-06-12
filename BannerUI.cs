using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using BannerCollector.Resources;
using System.Windows.Forms;
using rail;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.UI;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.ID;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using BannerCollector;

namespace BannerCollector
{
    class BannerUI : UIState
    {
        //BannerButton bannerButton;
        BannerPanel bannerPanel;
        ButtonLeft buttonLeft;
        ButtonRight buttonRight;
        ButtonSort buttonSort;
        ButtonFilter buttonFilter;
        ButtonFilterMode buttonFilterMode;
        ButtonFilterMod buttonFilterMod;
        ButtonModEntry[] modEntries;
        UIPanel modDropdownPanel;
        bool modDropdownOpen;
        bool modPrevMouseLeft; // previous-frame left button state, for outside-click detection
        const float DropdownPadding = 8f; // inner gap between the panel border and the rows
        const float DropdownGap = 4f;     // gap between the filter button and the panel
        ButtonClose buttonClose;
        BannerSlot[] bannerSlot;
        ButtonPage[] buttonPage;
        public List<BannerInfo> bannerList;

        private int page = 1;
        private int totalPages = 0;

        private bool bannerCollectorVisible;
        private bool Sorting = false;
        private int filter = 0; //0: 전체, 1: 보유함. 2:미보유
        private int filterMode = 0; //0: 전체, 1: 하드모드 이전. 2:하드모드
        private int filterMod = 0; //0: All, 1: All Mods (modded only), 2: Terraria (vanilla), 3..: ModList[filterMod-3]

        // Window dragging state. The window's children are siblings positioned from the panel's
        // top-left, so a drag moves the panel and re-runs PositionElements to reflow everything.
        private const float DragThresholdSq = 16f; // a press must move >4px before it counts as a drag
        private bool windowDragging;         // a press started on the panel and the button is still held
        private bool windowMoved;            // the window actually moved this press (vs a plain click)
        private Vector2 windowDragOffset;    // cursor-to-panel-top-left offset captured at drag start
        public void SetFirstPage()
        {
            page = 1;
            buttonPage[0].SetPage();
            for (int i = 1; i < totalPages; i++)
            {
                buttonPage[i].UnSetPage();
            }
        }

        public bool BannerCollectorVisible
        {
            get => bannerCollectorVisible;
            set
            {
                if (value)
                {
                    Append(bannerPanel);
                    Append(buttonLeft);
                    Append(buttonRight);
                    Append(buttonSort);
                    Append(buttonFilter);
                    Append(buttonFilterMode);
                    if (BannerLoad.isModded)
                    {
                        Append(buttonFilterMod);
                    }
                    Append(buttonClose);

                    RestoreWindowPosition(); // first open this session: apply the saved position
                    PositionElements();      // lay out header buttons, the slot grid and page dots
                    for (int i = 0; i < totalPages; i++)
                    {
                        Append(buttonPage[i]);
                    }

                    AppendBannerSlot();
                    PageLoad();
                }
                else
                {
                    CloseModDropdown();
                    RemoveChild(bannerPanel);
                    RemoveChild(buttonLeft);
                    RemoveChild(buttonRight);
                    RemoveChild(buttonSort);
                    RemoveChild(buttonFilter);
                    RemoveChild(buttonFilterMode);
                    if (BannerLoad.isModded)
                    {
                        RemoveChild(buttonFilterMod);
                    }
                    RemoveChild(buttonClose);

                    for (int i = 0; i < totalPages; i++)
                    {
                        RemoveChild(buttonPage[i]);
                    }
                    for (int i = 0; i < bannerSlot.Length; i++)
                    {
                        RemoveChild(bannerSlot[i]);
                    }
                }
                bannerCollectorVisible = value;
            }
        }

        /// <summary>
        /// Positions the page-navigation dots in centered rows in the band below the banner grid,
        /// wrapping onto a new row once a row would exceed the grid's width, and grows the panel's
        /// height downward so every dot row stays inside the window. The per-row cap scales with the
        /// column count so the dots stay under the banners instead of stretching across the panel.
        /// </summary>
        private void LayoutPageButtons(float pX, float pY, float pW)
        {
            const int dotSize = 16;
            // Keep each dot row within the banner grid's width (Columns cells wide).
            int maxPerRow = Math.Max(1, BannerGrid.Columns * BannerGrid.CellPitchX / dotSize);
            int perRow = Math.Min(maxPerRow, Math.Max(1, (int)(pW / dotSize)));
            int rows = (int)Math.Ceiling((double)totalPages / perRow);

            // Grow the panel downward (its top-left is fixed) so every dot row fits inside it.
            bannerPanel.Height.Pixels = BannerGrid.DesignHeight + Math.Max(0, rows - 1) * dotSize;
            bannerPanel.Recalculate();

            for (int i = 0; i < totalPages; i++)
            {
                int row = i / perRow;
                int col = i % perRow;
                int dotsInRow = Math.Min(perRow, totalPages - row * perRow);
                float rowWidth = dotsInRow * dotSize;
                buttonPage[i].Left.Set((pW - rowWidth) / 2f + col * dotSize + pX, 0f);
                buttonPage[i].Top.Set(pY + BannerGrid.PageDotBandTop + row * dotSize, 0f);
            }
        }

        private void PageLoad()
        {
            if (totalPages == 1 || totalPages == 0)
            {
                RemoveChild(buttonLeft);
                RemoveChild(buttonRight);

            }
            else if (page == 1)
            {
                RemoveChild(buttonLeft);
                Append(buttonRight);

            }
            else if (page == totalPages)
            {
                Append(buttonLeft);
                RemoveChild(buttonRight);
            }
            else
            {
                Append(buttonLeft);
                Append(buttonRight);
            }

            int pageSize = bannerSlot.Length; // one full grid of slots
            for (int i = 0; i < pageSize; i++)
            {
                if (i + pageSize * (page - 1) >= bannerList.Count)
                {
                    RemoveChild(bannerSlot[i]);
                }
                else
                {
                    Append(bannerSlot[i]);
                    bannerSlot[i].SetBannerInfo(bannerList[i + pageSize * (page - 1)]);
                }
            }
        }

        private void AppendBannerSlot()
        {
            // Positions are set by PositionElements; this only adds the slots to the state.
            for (int i = 0; i < bannerSlot.Length; i++)
                Append(bannerSlot[i]);
        }

        /// <summary>
        /// Positions every child of the window - the header buttons, the banner-slot grid and
        /// the page dots - relative to the banner panel's current top-left. Called on open and on
        /// every drag frame so the whole window moves as one piece. Pure positioning: it never
        /// appends or removes elements, so it is safe to call regardless of which page is shown.
        /// </summary>
        private void PositionElements()
        {
            float pX = bannerPanel.GetDimensions().X;
            float pY = bannerPanel.GetDimensions().Y;
            float pW = bannerPanel.GetDimensions().Width;

            // Left-side header buttons keep their fixed positions. The close button tracks the right
            // edge (pX + pW), so it stays at the far right as the window widens. The page arrows are
            // centred exactly on the banner grid (the pill rows span TopEdgeH .. TopEdgeH+Rows*CellH),
            // so they sit dead-centre instead of riding high near the top as the original art did.
            float arrowTop = pY + BannerGrid.TopEdgeH + BannerGrid.Rows * BannerGrid.CellH / 2f - buttonLeft.Height.Pixels / 2f;
            buttonLeft.Left.Set(pX + 9f, 0f);
            buttonLeft.Top.Set(arrowTop, 0f);
            // The right arrow's wide base (its frame-facing side) carries a dark outline, so its gap to
            // the frame reads larger than the left arrow's light-edged base at the same distance. Nudged
            // 2px toward the frame so both bases sit equally close to their frame.
            buttonRight.Left.Set(pX + pW - 27f, 0f);
            buttonRight.Top.Set(arrowTop, 0f);
            buttonSort.Left.Set(pX + 43, 0f);
            buttonSort.Top.Set(pY + 14, 0f);
            buttonFilter.Left.Set(pX + 73, 0f);
            buttonFilter.Top.Set(pY + 14, 0f);
            buttonFilterMode.Left.Set(pX + 103, 0f);
            buttonFilterMode.Top.Set(pY + 14, 0f);
            buttonFilterMod.Left.Set(pX + 133, 0f);
            buttonFilterMod.Top.Set(pY + 14, 0f);
            buttonClose.Left.Set(pX + pW - 63, 0f);
            buttonClose.Top.Set(pY + 14, 0f);

            // Banner-slot grid: Columns x Rows, row-major.
            for (int i = 0; i < bannerSlot.Length; i++)
            {
                int col = i % BannerGrid.Columns;
                int row = i / BannerGrid.Columns;
                bannerSlot[i].Left.Set(pX + BannerGrid.FirstSlotX + col * BannerGrid.CellPitchX, 0f);
                bannerSlot[i].Top.Set(pY + BannerGrid.FirstSlotY + row * BannerGrid.CellPitchY, 0f);
            }

            LayoutPageButtons(pX, pY, pW);
        }

        /// <summary>Starts a window drag from a press on the panel background (not on a child).</summary>
        private void StartWindowDrag(UIMouseEvent evt, UIElement listeningElement)
        {
            // Dragging is disabled while the window is locked (the default, and after a config reset).
            if (BannerCollectorConfig.Instance?.LockWindowPosition ?? true)
                return;

            windowDragging = true;
            windowMoved = false;
            windowDragOffset = evt.MousePosition - new Vector2(bannerPanel.Left.Pixels, bannerPanel.Top.Pixels);
        }

        /// <summary>
        /// Drives an in-progress window drag: while the button is held and the cursor has moved past
        /// the threshold, the panel follows the cursor (clamped to the screen) and the children are
        /// reflowed. On release, the new position is saved. Detecting release here (rather than via a
        /// mouse-up event) makes the drag robust even if the cursor leaves the panel.
        /// </summary>
        private void UpdateWindowDrag()
        {
            if (!windowDragging)
                return;

            if (Main.mouseLeft)
            {
                Main.LocalPlayer.mouseInterface = true; // block item use/world interaction while dragging
                Vector2 target = Main.MouseScreen - windowDragOffset;
                if (!windowMoved &&
                    Vector2.DistanceSquared(target, new Vector2(bannerPanel.Left.Pixels, bannerPanel.Top.Pixels)) >= DragThresholdSq)
                {
                    windowMoved = true;
                }

                if (windowMoved)
                {
                    target = ClampToScreen(target);
                    bannerPanel.Left.Set(target.X, 0f);
                    bannerPanel.Top.Set(target.Y, 0f);
                    bannerPanel.Recalculate();
                    PositionElements();
                }
            }
            else
            {
                windowDragging = false;
                if (windowMoved)
                    SaveWindowPosition();
                // Cleared here (after the save, and after BannerPanelLeftClicked has read it) so the
                // drag-end save is never skipped and the next plain click deposits normally.
                windowMoved = false;
            }
        }

        /// <summary>Keeps the given panel top-left within the screen bounds.</summary>
        private Vector2 ClampToScreen(Vector2 topLeft)
        {
            float maxX = Math.Max(0f, Main.screenWidth - bannerPanel.GetDimensions().Width);
            float maxY = Math.Max(0f, Main.screenHeight - bannerPanel.GetDimensions().Height);
            return new Vector2(MathHelper.Clamp(topLeft.X, 0f, maxX), MathHelper.Clamp(topLeft.Y, 0f, maxY));
        }

        /// <summary>Persists the current window position to the client config so it survives a restart.</summary>
        private void SaveWindowPosition()
        {
            BannerCollectorConfig config = BannerCollectorConfig.Instance;
            if (config == null)
                return;

            config.WindowX = (int)Math.Round(bannerPanel.Left.Pixels);
            config.WindowY = (int)Math.Round(bannerPanel.Top.Pixels);
            config.Save();
        }

        /// <summary>
        /// Applies the window position saved in the config each time the window opens, so it always
        /// reflects the last dragged spot - and, after "Reset to Defaults", returns to the original
        /// position. Clamped to the screen in case the saved spot is now off-screen (e.g. after a
        /// resolution change).
        /// </summary>
        private void RestoreWindowPosition()
        {
            BannerCollectorConfig config = BannerCollectorConfig.Instance;
            if (config == null)
                return;

            Vector2 pos = ClampToScreen(new Vector2(config.WindowX, config.WindowY));
            bannerPanel.Left.Set(pos.X, 0f);
            bannerPanel.Top.Set(pos.Y, 0f);
            bannerPanel.Recalculate();
        }

        private void SortFilterList()
        {
            bannerList = BannerLoad.BannerDict.Values.ToList();

            //페이지 변경
            for (int i = 0; i < totalPages; i++)
            {
                RemoveChild(buttonPage[i]);
            }

            //정렬
            if (Sorting)
            {
                bannerList = bannerList.OrderByDescending(b => b.BannerCount).ToList();
            }

            switch (filter)
            {
                case 0: //전체
                    break;
                case 1: //보유
                    bannerList.RemoveAll(banner => banner.BannerCount == 0);
                    break;
                case 2: //미보유
                    bannerList.RemoveAll(banner => banner.BannerCount > 0);
                    break;
            }

            switch (filterMode)
            {
                case 0: //전체
                    break;
                case 1: //하드모드 이전
                    bannerList.RemoveAll(banner => banner.IsHardMode == true);
                    break;
                case 2: //하드모드
                    bannerList.RemoveAll(banner => banner.IsHardMode == false);
                    break;
            }

            if (BannerLoad.isModded)
            {
                if (filterMod == 0) { }                                  // All: every banner
                else if (filterMod == 1)                                 // All Mods: modded banners only
                    bannerList.RemoveAll(banner => banner.ModName == null);
                else if (filterMod == 2)                                 // Terraria: vanilla banners only
                    bannerList.RemoveAll(banner => banner.ModName != null);
                else                                                     // a specific mod
                    bannerList.RemoveAll(banner => banner.ModName != BannerLoad.ModList[filterMod - 3]);
            }

            totalPages = (int)Math.Ceiling((double)bannerList.Count / bannerSlot.Length);
            float pX = bannerPanel.GetDimensions().X;
            float pY = bannerPanel.GetDimensions().Y;
            float pW = bannerPanel.GetDimensions().Width;
            LayoutPageButtons(pX, pY, pW);
            for (int i = 0; i < totalPages; i++)
            {
                Append(buttonPage[i]);
            }
            buttonPage[page - 1].UnSetPage();
            buttonPage[0].SetPage();
            page = 1;
            PageLoad();
        }
        public void InitializePage()
        {
            RebuildGridIfNeeded(); // make the slots/panel match the configured grid before paging
            totalPages = (int)Math.Ceiling((double)BannerLoad.BannerDict.Count / bannerSlot.Length);
            buttonPage = new ButtonPage[totalPages]; //Y촤표 176 간격 16픽셀씩
            for (int i = 0; i < totalPages; i++)
            {
                int index = i;
                buttonPage[index] = new ButtonPage();
                buttonPage[index].thisPage = index + 1;
                buttonPage[index].OnLeftClick += (evt, listeningElement) => ButtonPageClicked(evt, listeningElement, index);
            }
            // Clamp the current page in case a grid change reduced the page count.
            if (page < 1 || page > totalPages)
                page = 1;
            buttonPage[page - 1].SetPage();
        }
        public override void OnInitialize()
        {
            BannerCollectorResources.PreloadAssets();

            //UI로드

            bannerPanel = new BannerPanel();
            bannerPanel.Top.Set(258, 0f);
            bannerPanel.Left.Set(20f, 0f);
            bannerPanel.OnLeftClick += BannerPanelLeftClicked;
            bannerPanel.OnLeftMouseDown += StartWindowDrag; // begin a window drag from the panel background

            //패널의 UI들은 bannerCollecterVisible에서 좌표 정함.
            buttonLeft = new ButtonLeft();
            buttonLeft.OnLeftClick += ButtonLeftClicked;
            buttonLeft.OnMouseOver += ButtonMouseOver;
            buttonRight = new ButtonRight();
            buttonRight.OnLeftClick += ButtonRightClicked;
            buttonRight.OnMouseOver += ButtonMouseOver;
            buttonSort = new ButtonSort();
            buttonSort.OnLeftClick += ButtonSortClicked;
            buttonSort.OnMouseOver += ButtonMouseOver;
            buttonFilter = new ButtonFilter();
            buttonFilter.OnLeftClick += ButtonFilterClicked;
            buttonFilter.OnRightClick += ButtonFilterRightClicked;
            buttonFilter.OnMouseOver += ButtonMouseOver;
            buttonFilterMode = new ButtonFilterMode();
            buttonFilterMode.OnLeftClick += ButtonFilterModeClicked;
            buttonFilterMode.OnRightClick += ButtonFilterModeRightClicked;
            buttonFilterMode.OnMouseOver += ButtonMouseOver;
            buttonFilterMod = new ButtonFilterMod();
            buttonFilterMod.OnLeftClick += ButtonFilterModClicked;
            buttonFilterMod.OnMouseOver += ButtonMouseOver;
            buttonClose = new ButtonClose();
            buttonClose.OnLeftClick += ButtonCloseClicked;
            buttonClose.OnMouseOver += ButtonMouseOver;

            CreateBannerSlots();
        }

        /// <summary>
        /// (Re)creates the banner-slot array sized to the current grid (<see cref="BannerGrid.PageSize"/>)
        /// and wires each slot's mouse handlers.
        /// </summary>
        private void CreateBannerSlots()
        {
            bannerSlot = new BannerSlot[BannerGrid.PageSize];
            for (int i = 0; i < bannerSlot.Length; i++)
            {
                int index = i;
                bannerSlot[index] = new BannerSlot();
                bannerSlot[index].OnLeftMouseDown += (evt, listeningElement) => BannerSlotLeftMouseDown(evt, listeningElement, index);
                bannerSlot[index].OnRightMouseDown += (evt, listeningElement) => BannerSlotRightMouseDown(evt, listeningElement, index);
                bannerSlot[index].OnRightMouseUp += BannerSlotRightMouseUp;
                bannerSlot[index].OnMouseOut += BannerSlotMouseOut;
            }
        }

        /// <summary>
        /// Rebuilds the slot array and panel size to match the configured grid if they are out of
        /// sync (e.g. the column/row config changed since the slots were last created). Pure setup -
        /// callers re-page afterwards. Run before laying out pages.
        /// </summary>
        private void RebuildGridIfNeeded()
        {
            if (bannerSlot != null && bannerSlot.Length == BannerGrid.PageSize)
                return;

            CreateBannerSlots();
            bannerPanel.Width.Pixels = BannerGrid.PanelWidth;
            bannerPanel.Height.Pixels = BannerGrid.DesignHeight;
            bannerPanel.Recalculate();
        }

        /// <summary>
        /// Applies a changed grid size from the config: rebuilds the slots, panel and pages so the
        /// next opening shows the new grid. Called from the config's OnChanged. No-op when the grid
        /// is unchanged, the UI is not ready, or while at the main menu (the world load rebuilds it).
        /// </summary>
        public void ApplyGridConfig()
        {
            if (Main.gameMenu || bannerSlot == null || bannerSlot.Length == BannerGrid.PageSize)
                return;

            bool wasVisible = bannerCollectorVisible;
            if (wasVisible)
                BannerCollectorVisible = false; // detach the old slots/dots before rebuilding

            bannerList = BannerLoad.BannerDict.Values.ToList();
            InitializePage(); // rebuilds the grid (RebuildGridIfNeeded) and the page buttons
            SetFirstPage();

            if (wasVisible)
                BannerCollectorVisible = true;
        }
        public void ButtonModSetDefault()
        {
            buttonFilterMod.SetDefault();
            BuildModEntries();
        }

        /// <summary>
        /// Rebuilds the mod-filter dropdown rows: "All" (every banner), "All Mods" (modded
        /// banners only), "Terraria" (vanilla banners only), then one row per mod from the
        /// (alphabetically sorted) <see cref="BannerLoad.ModList"/>. Each row carries the filter
        /// value it selects so picking it maps straight onto the index-based filter
        /// (<c>filterMod</c>): 0 = All, 1 = All Mods, 2 = Terraria, 3.. = ModList[filterMod-3].
        /// </summary>
        private void BuildModEntries()
        {
            CloseModDropdown();
            modDropdownPanel = null;

            // Reset the filter to "All" on every (re)build, so a selection kept from a
            // previous world can never index past a now-shorter ModList in SortFilterList.
            filterMod = 0;
            buttonFilterMod.ChangeState(0);

            if (!BannerLoad.isModded)
            {
                modEntries = new ButtonModEntry[0];
                return;
            }

            modEntries = new ButtonModEntry[BannerLoad.ModList.Count + 3];
            modEntries[0] = new ButtonModEntry("All");          // every banner
            modEntries[0].OnLeftClick += (evt, listeningElement) => SelectMod(0);
            modEntries[1] = new ButtonModEntry("All Mods");     // modded banners only
            modEntries[1].OnLeftClick += (evt, listeningElement) => SelectMod(1);
            modEntries[2] = new ButtonModEntry("Terraria");     // vanilla banners only
            modEntries[2].OnLeftClick += (evt, listeningElement) => SelectMod(2);
            for (int i = 0; i < BannerLoad.ModList.Count; i++)
            {
                int value = i + 3; // 0 = All, 1 = All Mods, 2 = Terraria (vanilla)
                modEntries[value] = new ButtonModEntry(BannerLoad.ModList[i]);
                modEntries[value].OnLeftClick += (evt, listeningElement) => SelectMod(value);
            }

            // Authentic Terraria panel framing the rows, with equal padding above the first
            // and below the last row.
            modDropdownPanel = new UIPanel();
            modDropdownPanel.BackgroundColor = new Color(40, 50, 90);   // opaque (A = 255)
            modDropdownPanel.BorderColor = new Color(13, 17, 35);       // opaque dark border
            modDropdownPanel.Width.Set(ButtonModEntry.RowWidth + DropdownPadding * 2f, 0f);
            modDropdownPanel.Height.Set(modEntries.Length * ButtonModEntry.RowHeight + DropdownPadding * 2f, 0f);
        }

        /// <summary>Opens the dropdown if closed, closes it if open.</summary>
        private void ToggleModDropdown()
        {
            if (modDropdownOpen)
                CloseModDropdown();
            else
                OpenModDropdown();
        }

        /// <summary>
        /// Appends the dropdown rows in a vertical column directly below the mod-filter button.
        /// Rows are appended last so they draw on top of the banner grid.
        /// </summary>
        private void OpenModDropdown()
        {
            if (modEntries == null || modEntries.Length == 0 || modDropdownPanel == null)
                return;

            float panelLeft = buttonFilterMod.Left.Pixels;
            float panelTop = buttonFilterMod.Top.Pixels + buttonFilterMod.Height.Pixels + DropdownGap;
            modDropdownPanel.Left.Set(panelLeft, 0f);
            modDropdownPanel.Top.Set(panelTop, 0f);
            Append(modDropdownPanel); // appended first so the rows draw on top of it

            float rowLeft = panelLeft + DropdownPadding;
            float rowTop = panelTop + DropdownPadding;
            for (int i = 0; i < modEntries.Length; i++)
            {
                modEntries[i].Left.Set(rowLeft, 0f);
                modEntries[i].Top.Set(rowTop + i * ButtonModEntry.RowHeight, 0f);
                Append(modEntries[i]);
            }
            modDropdownOpen = true;
        }

        /// <summary>Removes the dropdown rows. Safe to call when already closed.</summary>
        private void CloseModDropdown()
        {
            if (modDropdownPanel != null)
                RemoveChild(modDropdownPanel);
            if (modEntries != null)
            {
                foreach (var entry in modEntries)
                    RemoveChild(entry);
            }
            modDropdownOpen = false;
        }

        /// <summary>Applies the picked mod filter, closes the dropdown and refreshes the list.</summary>
        private void SelectMod(int value)
        {
            buttonFilterMod.ChangeState(value);
            filterMod = value;
            CloseModDropdown();
            SortFilterList();
        }
        
        public  void ShowBannerCollection(bool show)
        {
            if (show == true)
            {
                BannerCollectorVisible = false;
            }
            else
            {
                BannerCollectorVisible = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Keep the window in sync with the (now persistent) collection toggle: hide it when the
            // inventory closes WITHOUT clearing the toggle, and re-show it when the inventory is
            // reopened if the toggle is still "open" (CurrentState == 1). This makes the toggle
            // sticky across inventory toggles and, with the saved CollectionOpen, across sessions.
            BannerButtonBuilderToggle toggle = BannerButtonBuilderToggle.Instance;
            if (!Main.playerInventory)
            {
                if (bannerCollectorVisible)
                    BannerCollectorVisible = false;
            }
            else if (toggle != null && toggle.CurrentState == 1 && !bannerCollectorVisible)
            {
                BannerCollectorVisible = true;
            }
            base.Update(gameTime);

            UpdateWindowDrag(); // move the window if a drag is in progress (after element events ran)

            // Close the mod dropdown when a fresh left click lands outside it (and outside the
            // button that toggles it). Uses own up->down edge detection rather than
            // Main.mouseLeftRelease, which the UI system clears while handling the click.
            // Runs after base.Update so a click that opened it or picked a row already ran and
            // counts as "inside".
            bool freshClick = Main.mouseLeft && !modPrevMouseLeft;
            modPrevMouseLeft = Main.mouseLeft;
            if (modDropdownOpen && freshClick)
            {
                Vector2 mouse = Main.MouseScreen;
                bool inside = (modDropdownPanel != null && modDropdownPanel.ContainsPoint(mouse))
                    || buttonFilterMod.ContainsPoint(mouse);
                if (!inside)
                    CloseModDropdown();
            }
        }

        #region 이벤트 관련 메서드
        private void ButtonFilterModClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            ToggleModDropdown();
        }

        private void ButtonMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
        }
        private void ButtonCloseClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            BannerCollectorVisible = false;
            BannerButtonBuilderToggle.Instance.CurrentState = 0;
        }
        private void ButtonFilterModeClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            buttonFilterMode.ChangeState((buttonFilterMode.filterIndex + 1) % 3);
            filterMode = buttonFilterMode.filterIndex;
            SortFilterList();
        }
        private void ButtonFilterModeRightClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            buttonFilterMode.ChangeState((buttonFilterMode.filterIndex + 2) % 3);
            filterMode = buttonFilterMode.filterIndex;
            SortFilterList();
        }

        private void ButtonFilterClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            buttonFilter.ChangeState((buttonFilter.filterIndex + 1) % 3);
            filter = buttonFilter.filterIndex;
            SortFilterList();
        }

        private void ButtonFilterRightClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            buttonFilter.ChangeState((buttonFilter.filterIndex + 2) % 3);
            filter = buttonFilter.filterIndex;
            SortFilterList();
        }


        private void ButtonSortClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Sorting)
            {
                Sorting = false;
                buttonSort.hoverText = buttonSort.sortingText1;
            }
            else
            {
                Sorting = true;
                buttonSort.hoverText = buttonSort.sortingText2;

            }
            SortFilterList();
        }

        private void ButtonPageClicked(UIMouseEvent evt, UIElement listeningElement, int i)
        {
            buttonPage[page - 1].UnSetPage();
            buttonPage[i].SetPage();
            page = i + 1;
            PageLoad();
        }

        private void MouseItemToSlot()
        {
            if (BannerLoad.BannerDict.ContainsKey(Main.mouseItem.type))
            {
                SoundEngine.PlaySound(SoundID.Grab);
                if (BannerLoad.BannerDict[Main.mouseItem.type].BannerCount >= 9999)
                {
                    //Main.NewText(1);
                    BannerLoad.BannerDict[Main.mouseItem.type].BannerCount = Main.mouseItem.stack;
                    Main.mouseItem.stack = 9999;

                }
                else if (BannerLoad.BannerDict[Main.mouseItem.type].BannerCount + Main.mouseItem.stack > 9999)
                {
                    //Main.NewText(2);
                    Main.mouseItem.stack = BannerLoad.BannerDict[Main.mouseItem.type].BannerCount + Main.mouseItem.stack - 9999;
                    BannerLoad.BannerDict[Main.mouseItem.type].BannerCount = 9999;
                }
                else
                {
                    BannerLoad.BannerDict[Main.mouseItem.type].BannerCount += Main.mouseItem.stack;
                    Main.mouseItem.stack = 0;
                }
                //bannerList = BannerLoad.BannerDict.Values.ToList();
            }
        }
        private void BannerSlotLeftMouseDown(UIMouseEvent evt, UIElement listeningElement, int i)
        {
            //마우스에 아이템을 들고있을 경우
            if (Main.mouseItem.stack > 0)
            {
                MouseItemToSlot();
            }
            else
            { //배너 슬롯에서 꺼내기
                if (bannerSlot[i].bannerInfo.BannerCount > 0)
                {
                    SoundEngine.PlaySound(SoundID.Grab);
                    Main.mouseItem = new Item(bannerSlot[i].bannerInfo.ItemId);
                    Main.mouseItem.stack = bannerSlot[i].bannerInfo.BannerCount;
                    BannerLoad.BannerDict[bannerSlot[i].bannerInfo.ItemId].BannerCount = 0;
                }
            }

        }

        private int clickDelay = 450; // 초기 지연 시간 (밀리초)
        private double currentDelay; // 현재 지연 시간
        private bool isRightMouseDown = false; // 우클릭 상태
        private int turn;

        // 아이템 쌓기 시작
        private void StartItemStacking(int i)
        {
            Task.Run(async () =>
            {
                while (isRightMouseDown)
                {
                    //슬롯에 아이템 없을경우
                    if (bannerSlot[i].bannerInfo.BannerCount <= 0)
                    {
                        return;
                    }
                    //슬롯과 들고있는 아이템이 다른경우
                    if (Main.mouseItem.type != bannerSlot[i].bannerInfo.ItemId && Main.mouseItem.type != ItemID.None)
                    {
                        return;
                    }
                    else
                    {
                        if (turn <= 10) SoundEngine.PlaySound(SoundID.MenuTick);
                        if (Main.mouseItem.stack == 0)
                            Main.mouseItem = new Item(bannerSlot[i].bannerInfo.ItemId);
                        else
                            Main.mouseItem.stack += 1;
                        BannerLoad.BannerDict[bannerSlot[i].bannerInfo.ItemId].BannerCount--;
                    }

                    // 지연 시간 조정
                    await Task.Delay((int)currentDelay);
                    turn++;
                    if (turn <= 10)
                    {
                        currentDelay = 70;
                    }
                    else if (turn > 10)
                    {
                        // 지연 시간 줄이기 (최소 지연 시간 설정 가능)
                        if (currentDelay > 1) // 예: 100ms 이하로는 줄이지 않음
                        {
                            currentDelay -= 0.5; // 지연 시간 감소
                        }
                    }
                }
            });
        }


        private void BannerSlotRightMouseDown(UIMouseEvent evt, UIElement listeningElement, int i)
        {
            isRightMouseDown = true;
            currentDelay = clickDelay; // 초기 지연 시간 설정
            turn = 1;
            StartItemStacking(i); // 아이템 쌓기 시작
        }
        private void BannerSlotRightMouseUp(UIMouseEvent evt, UIElement listeningElement)
        {
            isRightMouseDown = false;
        }
        private void BannerSlotMouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
            isRightMouseDown = false;
        }
        private void BannerPanelLeftClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            // A click that finished a window drag must not also deposit the held item. windowMoved is
            // only read here (not cleared) - UpdateWindowDrag runs right after this, saves the new
            // position and clears the flag, so clearing it here would lose the position save.
            if (windowMoved)
                return;
            MouseItemToSlot();
        }

        private void ButtonLeftClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            buttonPage[(page--) - 1].UnSetPage();
            buttonPage[page - 1].SetPage();
            PageLoad();
        }

        private void ButtonRightClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            buttonPage[(page++) - 1].UnSetPage();
            buttonPage[page - 1].SetPage();
            PageLoad();
        }
        #endregion
    }
}

public class BannerButtonBuilderToggle : BuilderToggle
{
    public static BannerButtonBuilderToggle Instance { get; private set; }
    public override string Texture => "BannerCollector/Resources/Banner_Button";
    public override string HoverTexture => "BannerCollector/Resources/Banner_Button_Border";
    public override bool Active() => true;

    public BannerButtonBuilderToggle()
    {
        Instance = this;
    }

    public override string DisplayValue()
    {
        string text = "";
        switch (CurrentState)
        {
            case 0:
                text = "Banner collection closed";
                break;
            case 1:
                text = "Banner collection opened";
                break;
        }

        return text;
    }

    public override bool Draw(SpriteBatch spriteBatch, ref BuilderToggleDrawParams drawParams)
    {

        return true;
    }

    public override bool DrawHover(SpriteBatch spriteBatch, ref BuilderToggleDrawParams drawParams)
    {
        return true;
    }

    public override bool OnLeftClick(ref SoundStyle? sound)
    {
        if (CurrentState == 1)
        {
            BannerUISystem.Instance.ShowBannerCollection(true);
        }
        else
        {
            BannerUISystem.Instance.ShowBannerCollection(false);
        }
        return true;
    }
}