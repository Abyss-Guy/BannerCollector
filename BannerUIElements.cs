using BannerCollector.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;
using System.Windows.Forms;
using Terraria.ID;
using Terraria.GameContent.Tile_Entities;
using Terraria.ObjectData;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria.Audio;

namespace BannerCollector
{

    internal class BannerUIElements : UIElement
    {
        public override void Update(GameTime gameTime)
        {
        }
        public BannerUIElements() { }

        public BannerUIElements(Texture2D asset)
        {
        }
        public static void HideMouseOverInteractions()
        {
            Main.player[Main.myPlayer].mouseInterface = true;
            Main.mouseText = true;
            Main.LocalPlayer.cursorItemIconEnabled = false;
            Main.LocalPlayer.cursorItemIconID = -1;
            Main.ItemIconCacheUpdate(0);
        }
    }



    internal class BannerPanel : BannerUIElements
    {
        public BannerPanel()
        {
            Width.Pixels = BannerGrid.PanelWidth;
            Height.Pixels = BannerGrid.DesignHeight;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                HideMouseOverInteractions();

            base.Draw(spriteBatch);

            // The board (UI_Board.png) is rebuilt to fit any column/row count as a true 9-slice:
            //   * CENTRE - the pill cells, tiled N columns x M rows. The cell tile is pure pills+beige
            //     with no frame pixels, so tiling can never repeat a frame fragment between cells.
            //   * TOP edge + FOOTER (frame line, page-dot band, bottom border) - horizontally uniform,
            //     so they are stretched across the centre width from one clean column.
            //   * LEFT and RIGHT edges - full-height strips that carry the inner frame's vertical line.
            //     That line bulges outward at each corner, so the strip is its OWN vertical 9-slice
            //     (top bulge + stretched straight middle + bottom bulge + footer): keeping the bulges
            //     1:1 is what stops them repeating at every row (the side "sticks") and what restores
            //     the bottom corners. The right edge is the left edge mirrored, so corners are symmetric.
            // Only the page-dot band stretches vertically (for extra dot rows); everything else keeps
            // its source pixels.
            //
            // Everything is composed in WHOLE SCREEN PIXELS under an identity matrix with point
            // sampling - not under Main.UIScaleMatrix - so every edge lands on an exact screen pixel
            // and neighbouring pieces share an identical edge at ANY UI scale. This is the same
            // seam-free technique used for banners in BannerSlot.DrawBannerFrames.
            Texture2D tex = BannerCollectorResources.UI_Panel.Value;
            Rectangle dst = GetInnerDimensions().ToRectangle();
            int columns = BannerGrid.Columns;
            int rows = BannerGrid.Rows;
            int totalH = dst.Height;                                 // panel height incl. extra dot rows
            int extra = totalH - (int)BannerGrid.DesignHeight;       // height added for extra dot rows (>= 0)
            int yB = BannerGrid.TopEdgeH + rows * BannerGrid.CellH;  // footer top (UI offset from panel top)
            int fa = BannerGrid.FooterArtY;
            float ui = Main.UIScale;

            // Panel top-left in exact screen pixels (the UI matrix is applied by hand here so the
            // identity-matrix batch below positions the panel correctly).
            Vector2 origin = Vector2.Transform(new Vector2(dst.X, dst.Y), Main.UIScaleMatrix);
            int ox = (int)Math.Round(origin.X);
            int oy = (int)Math.Round(origin.Y);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

            // --- Centre: the pill grid, tiled in both directions. ---
            for (int r = 0; r < rows; r++)
            {
                int py0 = PxY(oy, ui, BannerGrid.TopEdgeH + r * BannerGrid.CellH);
                int py1 = PxY(oy, ui, BannerGrid.TopEdgeH + (r + 1) * BannerGrid.CellH);
                for (int c = 0; c < columns; c++)
                {
                    int px0 = PxX(ox, ui, BannerGrid.EdgeW + c * BannerGrid.CellW);
                    int px1 = PxX(ox, ui, BannerGrid.EdgeW + (c + 1) * BannerGrid.CellW);
                    spriteBatch.Draw(tex, new Rectangle(px0, py0, px1 - px0, py1 - py0),
                        new Rectangle(BannerGrid.CellArtX, BannerGrid.CellArtY, BannerGrid.CellW, BannerGrid.CellH), Color.White);
                }
            }

            // --- Top edge and footer bands: horizontally uniform, stretched across the centre width. ---
            int bandBottom = yB + BannerGrid.FooterLineH + BannerGrid.FooterBandH + extra; // bottom border start
            DrawCenterBand(spriteBatch, tex, ox, oy, ui, 0, BannerGrid.TopEdgeH, 0, BannerGrid.TopEdgeH);
            DrawCenterBand(spriteBatch, tex, ox, oy, ui, yB, yB + BannerGrid.FooterLineH, fa, BannerGrid.FooterLineH);
            DrawCenterBand(spriteBatch, tex, ox, oy, ui, yB + BannerGrid.FooterLineH, bandBottom, fa + BannerGrid.FooterLineH, BannerGrid.FooterBandH);
            DrawCenterBand(spriteBatch, tex, ox, oy, ui, bandBottom, totalH, fa + BannerGrid.FooterLineH + BannerGrid.FooterBandH, BannerGrid.FooterBottomH);

            // --- Left and right edges (full height). The right edge is the left, mirrored. ---
            DrawEdge(spriteBatch, tex, ox, oy, ui, 0, false, yB, extra, totalH);
            DrawEdge(spriteBatch, tex, ox, oy, ui, (int)BannerGrid.PanelWidth - BannerGrid.EdgeW, true, yB, extra, totalH);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
        }

        /// <summary>Maps a UI-space X offset (from the panel's left edge) to a whole screen pixel.</summary>
        private static int PxX(int ox, float ui, int offUi) => ox + (int)Math.Round(offUi * ui);

        /// <summary>Maps a UI-space Y offset (from the panel's top edge) to a whole screen pixel.</summary>
        private static int PxY(int oy, float ui, int offUi) => oy + (int)Math.Round(offUi * ui);

        /// <summary>
        /// Draws one horizontally-uniform band of the centre (the top frame line, or a footer
        /// sub-band) by stretching a single clean column (<see cref="BannerGrid.FrameMidArtX"/>)
        /// across the gap between the two edges, from source rows [<paramref name="srcY"/>,
        /// srcY+<paramref name="srcH"/>) into screen rows [<paramref name="yAui"/>,
        /// <paramref name="yBui"/>). Boundaries are rounded to whole screen pixels, so the band abuts
        /// the edges exactly at any UI scale.
        /// </summary>
        private static void DrawCenterBand(SpriteBatch sb, Texture2D tex, int ox, int oy, float ui,
            int yAui, int yBui, int srcY, int srcH)
        {
            int xl = PxX(ox, ui, BannerGrid.EdgeW);
            int xr = PxX(ox, ui, (int)BannerGrid.PanelWidth - BannerGrid.EdgeW);
            int y0 = PxY(oy, ui, yAui);
            int y1 = PxY(oy, ui, yBui);
            if (xr > xl && y1 > y0)
                sb.Draw(tex, new Rectangle(xl, y0, xr - xl, y1 - y0), new Rectangle(BannerGrid.FrameMidArtX, srcY, 1, srcH), Color.White);
        }

        /// <summary>
        /// Draws one left/right edge strip (a vertical 9-slice of the inner frame's vertical line)
        /// from the LEFT edge art at sheet x 0..38. The two corner bulges are kept 1:1 while the
        /// straight middle and the page-dot band stretch, so the bulges never repeat and stay at the
        /// real corners. <paramref name="flip"/> mirrors the art horizontally for the right edge, so
        /// both sides are exact symmetric copies. The footer sub-bands use the same Y layout as
        /// <see cref="DrawCenterBand"/>, so the frame line and border are continuous across the seam.
        /// </summary>
        /// <param name="destXOff">UI-space X of the strip's left edge (0 for left, PanelWidth-EdgeW for right).</param>
        /// <param name="flip">Mirror the art horizontally (for the right edge).</param>
        /// <param name="yB">Footer top as a UI-space offset from the panel's top.</param>
        /// <param name="extra">Extra height (page-dot band stretch).</param>
        /// <param name="totalH">Full panel height in UI pixels.</param>
        private static void DrawEdge(SpriteBatch sb, Texture2D tex, int ox, int oy, float ui,
            int destXOff, bool flip, int yB, int extra, int totalH)
        {
            SpriteEffects fx = flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            int x0 = PxX(ox, ui, destXOff);
            int w = PxX(ox, ui, destXOff + BannerGrid.EdgeW) - x0;
            int fa = BannerGrid.FooterArtY;
            int bandTop = yB + BannerGrid.FooterLineH;
            int capBottom = bandTop + BannerGrid.EdgeBandCapH;
            int bandBottom = bandTop + BannerGrid.FooterBandH + extra;

            // Top corner (border + frame line + top bulge), 1:1.
            DrawEdgePiece(sb, tex, x0, w, oy, ui, 0, BannerGrid.EdgeTopCornerH, 0, BannerGrid.EdgeTopCornerH, fx);
            // Straight vertical line, stretched between the two bulges.
            DrawEdgePiece(sb, tex, x0, w, oy, ui, BannerGrid.EdgeTopCornerH, yB - BannerGrid.EdgeBulgeH, BannerGrid.EdgeMidArtY, 1, fx);
            // Bottom bulge, 1:1, just above the footer.
            DrawEdgePiece(sb, tex, x0, w, oy, ui, yB - BannerGrid.EdgeBulgeH, yB, BannerGrid.EdgeBulgeArtY, BannerGrid.EdgeBulgeH, fx);
            // Footer frame line.
            DrawEdgePiece(sb, tex, x0, w, oy, ui, yB, bandTop, fa, BannerGrid.FooterLineH, fx);
            // Page-dot band: the corner block at its top stays 1:1 (so it never thickens when the band
            // grows for extra dot rows); only the uniform beige below it is stretched.
            DrawEdgePiece(sb, tex, x0, w, oy, ui, bandTop, capBottom, fa + BannerGrid.FooterLineH, BannerGrid.EdgeBandCapH, fx);
            DrawEdgePiece(sb, tex, x0, w, oy, ui, capBottom, bandBottom, fa + BannerGrid.FooterLineH + BannerGrid.EdgeBandCapH, 1, fx);
            // Bottom border.
            DrawEdgePiece(sb, tex, x0, w, oy, ui, bandBottom, totalH, fa + BannerGrid.FooterLineH + BannerGrid.FooterBandH, BannerGrid.FooterBottomH, fx);
        }

        /// <summary>
        /// Draws one vertical piece of an edge strip: source (sheet x 0..38,
        /// [<paramref name="srcY"/>, srcY+<paramref name="srcH"/>)) into the strip's screen column
        /// [<paramref name="x0"/>, x0+<paramref name="w"/>) and screen rows mapped from
        /// [<paramref name="yAui"/>, <paramref name="yBui"/>). Heights are scaled (srcH -> the row
        /// span); the width is the fixed edge width. <paramref name="fx"/> mirrors it for the right edge.
        /// </summary>
        private static void DrawEdgePiece(SpriteBatch sb, Texture2D tex, int x0, int w, int oy, float ui,
            int yAui, int yBui, int srcY, int srcH, SpriteEffects fx)
        {
            int y0 = PxY(oy, ui, yAui);
            int y1 = PxY(oy, ui, yBui);
            if (y1 > y0)
                sb.Draw(tex, new Rectangle(x0, y0, w, y1 - y0), new Rectangle(0, srcY, BannerGrid.EdgeW, srcH),
                    Color.White, 0f, Vector2.Zero, fx, 0f);
        }
    }

    internal class ButtonLeft : BannerUIElements
    {
        public ButtonLeft()
        {
            Width.Pixels = 18f;
            Height.Pixels = 46f;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                HideMouseOverInteractions();

            base.Draw(spriteBatch);

            Color color = new Color(155, 155, 145);
            // 패널 그리기
            Rectangle inner = GetInnerDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen))
            {   //마우스 호버링 중이면 버튼 어둡게
                Vector2 downPos = new Vector2(this.Left.Pixels, this.Top.Pixels + 2);
                inner = new Rectangle(0, 0, 18, 46);
                spriteBatch.Draw(BannerCollectorResources.Button_Left.Value, downPos, inner, color);
            }
            else
            {
                spriteBatch.Draw(BannerCollectorResources.Button_Left.Value, inner, Color.White);
            }
        }
    }

    internal class ButtonRight : BannerUIElements
    {
        public ButtonRight()
        {
            Width.Pixels = 18f;
            Height.Pixels = 46f;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                HideMouseOverInteractions();

            base.Draw(spriteBatch);

            Color color = new Color(155, 155, 145);
            // 패널 그리기
            Rectangle inner = GetInnerDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen))
            {
                Vector2 downPos = new Vector2(this.Left.Pixels, this.Top.Pixels + 2);
                inner = new Rectangle(0, 0, 18, 46);
                spriteBatch.Draw(BannerCollectorResources.Button_Right.Value, downPos, inner, color);
            }
            else
            {
                spriteBatch.Draw(BannerCollectorResources.Button_Right.Value, inner, Color.White);
            }
        }
    }

    internal class ButtonSort : BannerUIElements
    {
        public string sortingText1 = "Sort by Type";
        public string sortingText2 = "Sort by Count";
        public string hoverText;
        Color color = new Color(155, 155, 145);
        public ButtonSort()
        {
            Width.Pixels = 24f;
            Height.Pixels = 24f;
            hoverText = sortingText1;
        }

        public override void Update(GameTime gameTime) { }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                HideMouseOverInteractions();

            base.Draw(spriteBatch);

            Rectangle inner = GetInnerDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen))
            {
                Main.mouseText = true; // 마우스 텍스트 활성화
                Main.hoverItemName = hoverText; // 아이템 이름 설정
                Vector2 downPos = new Vector2(this.Left.Pixels, this.Top.Pixels + 1);
                inner = new Rectangle(0, 0, 24, 24);
                spriteBatch.Draw(BannerCollectorResources.Button_Sort.Value, downPos, inner, color);
            }
            else
            {
                spriteBatch.Draw(BannerCollectorResources.Button_Sort.Value, inner, Color.White);
                
                //Vector2 downPos = new Vector2(50, 50);
                //Rectangle R = new Rectangle(0, 0, 2322, 54);
                //spriteBatch.Draw(BannerLoad.ModBannerTexture["CalamityMod"].Value, downPos, R, Color.White);
            }
        }
    }

    internal class ButtonFilter : BannerUIElements
    {
        private string[] filteringText = { "Filter All", "Filter Owned Only", "Filter Unowned Only" };
        public int filterIndex;
        private string hoverText;
        Color color = new Color(180, 180, 180);
        Asset<Texture2D> texture;

        public ButtonFilter()
        {
            Width.Pixels = 24f;
            Height.Pixels = 24f;
            filterIndex = 0;
            hoverText = filteringText[filterIndex];
        }

        public override void Update(GameTime gameTime) { }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                HideMouseOverInteractions();

            base.Draw(spriteBatch);

            if (filterIndex == 0) { texture = BannerCollectorResources.Button_Filter; }
            else { texture = BannerCollectorResources.Button_Filter; }

            Rectangle inner = GetInnerDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen))
            {
                Main.mouseText = true; // 마우스 텍스트 활성화
                Main.hoverItemName = hoverText; // 아이템 이름 설정
                Vector2 downPos = new Vector2(this.Left.Pixels, this.Top.Pixels + 1);
                inner = new Rectangle(0, 0, 24, 24);
                spriteBatch.Draw(texture.Value, downPos, inner, color);
            }
            else
            {
                spriteBatch.Draw(texture.Value, inner, Color.White);
            }
        }

        public void ChangeState(int state)
        {
            filterIndex = state;
            hoverText = filteringText[filterIndex];
        }
    }

    internal class ButtonFilterMode : BannerUIElements
    {
        private string[] filteringText = { "Filter All", "Filter Pre-Hardmode", "Filter Hardmode", "Filter Post-Moon Lord" };
        public int filterIndex;
        private string hoverText;
        Color color = new Color(180, 180, 180);
        Asset<Texture2D> texture;

        public ButtonFilterMode()
        {
            Width.Pixels = 24f;
            Height.Pixels = 24f;
            filterIndex = 0;
            hoverText = filteringText[filterIndex];
        }

        public override void Update(GameTime gameTime) { }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                HideMouseOverInteractions();

            base.Draw(spriteBatch);

            if (filterIndex == 1) { texture = BannerCollectorResources.Button_FilterPreHard; }
            else if (filterIndex == 2) { texture = BannerCollectorResources.Button_FilterHard; }
            else if (filterIndex == 3) { texture = BannerCollectorResources.Button_FilterPostMoonLord; }
            else { texture = BannerCollectorResources.Button_FilterMode; }

            Rectangle inner = GetInnerDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen))
            {
                Main.mouseText = true; // 마우스 텍스트 활성화
                Main.hoverItemName = hoverText; // 아이템 이름 설정
                Vector2 downPos = new Vector2(this.Left.Pixels, this.Top.Pixels + 1);
                inner = new Rectangle(0, 0, 24, 24);
                spriteBatch.Draw(texture.Value, downPos, inner, color);
            }
            else
            {
                spriteBatch.Draw(texture.Value, inner, Color.White);
            }
        }


        public void ChangeState(int state)
        {
            filterIndex = state;
            hoverText = filteringText[filterIndex];
        }
    }
    internal class ButtonFilterMod : BannerUIElements
    {
        private List<string> filteringText = new List<string>();
        public int filterIndex;
        public int modNum;
        private string hoverText;
        Color color = new Color(180, 180, 180);
        Asset<Texture2D> texture;

        public ButtonFilterMod()
        {
            Width.Pixels = 24f;
            Height.Pixels = 24f;
            filterIndex = 0;
            filteringText.Add("Filter All");
            modNum = filteringText.Count;
            hoverText = filteringText[filterIndex];
        }

        public void SetDefault()
        {
            // Rebuild from scratch so repeated world loads don't accumulate stale entries.
            // Index order must match BannerUI's filter values: 0 = All, 1 = All Mods (modded
            // only), 2 = Terraria (vanilla), 3.. = each mod of the (alphabetically sorted) ModList.
            filteringText.Clear();
            filteringText.Add("Filter All");
            filteringText.Add("Filter All Mods");
            filteringText.Add("Filter Terraria");
            if (BannerLoad.isModded)
            {
                foreach (var text in BannerLoad.ModList)
                {
                    filteringText.Add("Filter " + text);
                }
            }
            modNum = filteringText.Count;
        }

        public override void Update(GameTime gameTime) { }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                HideMouseOverInteractions();

            base.Draw(spriteBatch);

            texture = BannerCollectorResources.Button_FilterMod;

            Rectangle inner = GetInnerDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen))
            {
                Main.mouseText = true; // 마우스 텍스트 활성화
                Main.hoverItemName = hoverText; // 아이템 이름 설정
                Vector2 downPos = new Vector2(this.Left.Pixels, this.Top.Pixels + 1);
                inner = new Rectangle(0, 0, 24, 24);
                spriteBatch.Draw(texture.Value, downPos, inner, color);
            }
            else
            {
                spriteBatch.Draw(texture.Value, inner, Color.White);
            }
        }


        public void ChangeState(int state)
        {
            filterIndex = state;
            hoverText = filteringText[filterIndex];
        }
    }

    /// <summary>
    /// A single clickable row of the mod-filter dropdown opened from <see cref="ButtonFilterMod"/>.
    /// Holds the filter value it selects (0 = all mods, 1..N = the matching entry of
    /// <see cref="BannerLoad.ModList"/>) and draws its label, highlighting while hovered.
    /// The rows are created and positioned by <see cref="BannerUI"/>.
    /// </summary>
    internal class ButtonModEntry : BannerUIElements
    {
        public const float RowWidth = 300f;
        public const float RowHeight = 30f;

        private readonly string label;

        public ButtonModEntry(string label)
        {
            this.label = label;
            Width.Pixels = RowWidth;
            Height.Pixels = RowHeight;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                HideMouseOverInteractions();

            base.Draw(spriteBatch);

            Rectangle inner = GetInnerDimensions().ToRectangle();
            bool hovered = ContainsPoint(Main.MouseScreen);

            // Only the hovered row is highlighted; the surrounding UIPanel provides the frame.
            if (hovered)
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, inner, new Color(120, 120, 160) * 0.5f);

            var font = FontAssets.MouseText.Value;
            const float scale = 1.2f;
            Vector2 textSize = font.MeasureString(label) * scale;
            Vector2 textPos = new Vector2(inner.X + 6, inner.Y + (inner.Height - textSize.Y) / 2f);
            Color textColor = hovered ? Color.White : new Color(220, 220, 220);
            spriteBatch.DrawString(font, label, textPos, textColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }


    internal class ButtonPage : BannerUIElements
    {
        public int thisPage; //이 컨트롤에 할당된 페이지
        private bool currentPage; //이 컨트롤이 헌재 페이지를 가리킬경우
        Color color = new Color(155, 155, 145);
        public ButtonPage()
        {
            Width.Pixels = 16f;
            Height.Pixels = 16f;
            currentPage = false;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                HideMouseOverInteractions();

            base.Draw(spriteBatch);

            // 패널 그리기
            Rectangle inner = GetInnerDimensions().ToRectangle();
            if (currentPage)
                spriteBatch.Draw(BannerCollectorResources.UI_Page.Value, inner, Color.White);
            else
                spriteBatch.Draw(BannerCollectorResources.UI_Page.Value, inner, color);
        }

        public void SetPage()
        {
            currentPage = true;
        }

        public void UnSetPage()
        {
            currentPage = false;
        }
    }

    internal class ButtonClose : BannerUIElements
    {
        public ButtonClose()
        {
            Width.Pixels = 24f;
            Height.Pixels = 24f;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                HideMouseOverInteractions();

            base.Draw(spriteBatch);

            Color color = new Color(180, 180, 180);
            // 패널 그리기
            Rectangle inner = GetInnerDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen))
            {
                Vector2 downPos = new Vector2(this.Left.Pixels, this.Top.Pixels + 1);
                inner = new Rectangle(0, 0, 24, 24);
                spriteBatch.Draw(BannerCollectorResources.Button_Close.Value, downPos, inner, color);
            }
            else
            {
                spriteBatch.Draw(BannerCollectorResources.Button_Close.Value, inner, Color.White);
            }
        }
    }

    internal class BannerSlot : BannerUIElements
    {
        public BannerInfo bannerInfo = new BannerInfo();
        public BannerSlot()
        {
            Width.Pixels = 16f;
            Height.Pixels = 48f;

            //bannerInfo 기본값 정하기
            bannerInfo.BannerCount = 1;
            bannerInfo.ItemId = -1;
            bannerInfo.ItemName = null;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                HideMouseOverInteractions();

            base.Draw(spriteBatch);

            // 패널 그리기
            Rectangle inner = GetInnerDimensions().ToRectangle();

            if (bannerInfo.UseItemIcon)
            {
                if (bannerInfo.TileType >= 0)
                    DrawTileBanner(spriteBatch);
                else
                    DrawModBannerIcon(spriteBatch);
                DrawBannerOverlay(spriteBatch);
            }
            else if (bannerInfo.Index == -1)
            {
                spriteBatch.Draw(BannerCollectorResources.UI_UndefinedBanner.Value, inner, Color.White);
            }
            else
            {
                Color color = bannerInfo.BannerCount == 0 ? new Color(143, 143, 143) : Color.White;
                Texture2D texture;
                //배너 그리기
                if (bannerInfo.ModName == "CalamityMod")
                {
                    texture = BannerLoad.ModBannerTexture["CalamityMod"].Value;
                }
                else if (bannerInfo.ModName == "CatalystMod")
                {
                    texture = BannerLoad.ModBannerTexture["CatalystMod"].Value;
                }
                else
                {
                    texture = TextureAssets.Tile[91].Value;
                }

                int widthCount = texture.Width / 18; //한줄에 있는 배너 수
                int srcX = (bannerInfo.Index % widthCount) * 18;
                int srcY = (bannerInfo.Index / widthCount) * 54;
                DrawBannerFrames(spriteBatch, texture, srcX, srcY, 18, 16, 16, 3, color);

                if (bannerInfo.BannerCount > 0)
                {
                    // 배너 수량 텍스트 그리기
                    string bannerCountText = bannerInfo.BannerCount.ToString();
                    float scale = 0.85f; // 원하는 크기 비율
                    Vector2 textSize = FontAssets.MouseText.Value.MeasureString(bannerCountText) * scale; // 스케일링된 텍스트 크기

                    // 텍스트 위치 계산 (가운데 정렬)
                    Vector2 textPos = new Vector2(this.Left.Pixels - (textSize.X / 2) + 3, this.Top.Pixels + 36);

                    // 테두리
                    spriteBatch.DrawString(FontAssets.MouseText.Value, bannerCountText, textPos + new Vector2(1, 1), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(FontAssets.MouseText.Value, bannerCountText, textPos + new Vector2(1, -1), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(FontAssets.MouseText.Value, bannerCountText, textPos + new Vector2(-1, -1), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(FontAssets.MouseText.Value, bannerCountText, textPos + new Vector2(-1, 1), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    //글자
                    spriteBatch.DrawString(FontAssets.MouseText.Value, bannerCountText, textPos, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }

                //하드모드 핀 그리기
                if (bannerInfo.IsHardMode)
                {
                    Vector2 spritePos = new Vector2(this.Left.Pixels - 5, this.Top.Pixels - 4);
                    Rectangle spriteFrame = new Rectangle(0, 0, 14, 12);
                    spriteBatch.Draw(BannerCollectorResources.Pin_HardMode.Value, spritePos, spriteFrame, Color.White);
                }
                else if (bannerInfo.IsPostMoonLord)
                {
                    Vector2 spritePos = new Vector2(this.Left.Pixels - 5, this.Top.Pixels - 4);
                    Rectangle spriteFrame = new Rectangle(0, 0, 14, 12);
                    spriteBatch.Draw(BannerCollectorResources.Pin_PostMoonLoard.Value, spritePos, spriteFrame, Color.White);
                }
            }

            if (bannerInfo.ItemId != -1)
            {
                // 마우스 위치 확인
                Vector2 mousePosition = Main.MouseScreen;

                // 마우스가 아이템 위에 있는지 확인
                if (mousePosition.X > this.Left.Pixels && mousePosition.X < this.Left.Pixels + this.Width.Pixels &&
                    mousePosition.Y > this.Top.Pixels && mousePosition.Y < this.Top.Pixels + this.Height.Pixels)
                {
                    // 툴팁 표시
                    Main.hoverItemName = Lang.GetItemNameValue(bannerInfo.ItemId); // 툴팁 텍스트 설정
                    Main.HoverItem = new Item(bannerInfo.ItemId); ; // HoverItem에 설정
                }
            }
        }

        /// <summary>
        /// Draws a banner from a packed banner tile sheet (vanilla Tiles_91, or a mod's banner
        /// tile) onto the slot, with point sampling so the sheet's inter-frame padding never
        /// bleeds into coloured lines. The frames are positioned and sized in whole SCREEN pixels
        /// - the slot's position is taken through <see cref="Main.UIScaleMatrix"/> and the frames
        /// are drawn under an identity matrix - so every frame boundary lands on an exact pixel at
        /// any UI scale and no seam can slip through; the UI's default scaled, linear sampling is
        /// restored afterwards. Shared by the vanilla atlas path and <see cref="DrawTileBanner"/>.
        /// </summary>
        /// <param name="srcX">X of the style's first frame in the sheet.</param>
        /// <param name="srcY">Y of the style's first frame in the sheet.</param>
        /// <param name="strideY">Sheet distance between consecutive frames (frame height + padding).</param>
        /// <param name="cellW">Frame width in pixels.</param>
        /// <param name="frameH">Frame height in pixels.</param>
        /// <param name="frameCount">Number of stacked frames (3 for a banner).</param>
        private void DrawBannerFrames(SpriteBatch spriteBatch, Texture2D texture, int srcX, int srcY, int strideY, int cellW, int frameH, int frameCount, Color color)
        {
            // Fit the whole banner into the slot (downscaling high-res sheets, never cropping),
            // then convert to screen pixels so frame boundaries are pixel-exact at any UI scale.
            float fit = Math.Min(Width.Pixels / cellW, Height.Pixels / (frameCount * frameH));
            float scale = fit * Main.UIScale;                       // sheet pixel -> screen pixel
            int drawW = Math.Max(1, (int)Math.Round(cellW * scale));

            // Top-left of the banner (centred in the slot) in exact screen coordinates, taken
            // through the UI matrix so any offset it carries is respected.
            Vector2 screen = Vector2.Transform(
                new Vector2(this.Left.Pixels + (Width.Pixels - cellW * fit) / 2f, this.Top.Pixels),
                Main.UIScaleMatrix);
            int left = (int)Math.Round(screen.X);
            int top = (int)Math.Round(screen.Y);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

            for (int f = 0; f < frameCount; f++)
            {
                // Whole-pixel, gap-free rows (y1 of one frame == y0 of the next), so a frame edge
                // never lands mid-pixel and samples the neighbouring padding texel.
                int y0 = top + (int)Math.Round(f * frameH * scale);
                int y1 = top + (int)Math.Round((f + 1) * frameH * scale);
                Rectangle dst = new Rectangle(left, y0, drawW, y1 - y0);
                Rectangle src = new Rectangle(srcX, srcY + f * strideY, cellW, frameH);
                spriteBatch.Draw(texture, dst, src, color);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
        }

        /// <summary>
        /// Draws a mod banner from its real banner tile (<see cref="BannerInfo.TileType"/>),
        /// selecting the style with <see cref="BannerInfo.Index"/> (the item's placeStyle). The
        /// cell geometry (frame size and padding) is read from the tile's own
        /// <see cref="TileObjectData"/> so banners authored at any resolution render at full
        /// quality. Falls back to the item icon if the tile texture is missing.
        /// </summary>
        private void DrawTileBanner(SpriteBatch spriteBatch)
        {
            Main.instance.LoadTiles(bannerInfo.TileType);
            Texture2D texture = TextureAssets.Tile[bannerInfo.TileType].Value;
            if (texture == null)
            {
                DrawModBannerIcon(spriteBatch); // tile sheet not usable -> fall back to the icon
                return;
            }

            // Real cell geometry from the tile's own object data (frame size, padding, frame
            // count), instead of assuming a 16px / 18px-stride sheet. Falls back to the standard
            // banner layout when unavailable.
            TileObjectData data = TileObjectData.GetTileData(bannerInfo.TileType, bannerInfo.Index);
            int cellW = Math.Max(1, data?.CoordinateWidth ?? 16);
            int padding = Math.Max(0, data?.CoordinatePadding ?? 2);
            int frameH = Math.Max(1, data?.CoordinateHeights != null && data.CoordinateHeights.Length > 0 ? data.CoordinateHeights[0] : 16);
            int frameCount = Math.Max(1, data?.Height ?? 3);

            int strideX = cellW + padding;
            int strideY = frameH + padding;
            int cellH = frameCount * strideY;                       // full banner height in the sheet
            int widthCount = Math.Max(1, texture.Width / strideX);
            int srcX = (bannerInfo.Index % widthCount) * strideX;
            int srcY = (bannerInfo.Index / widthCount) * cellH;
            Color color = bannerInfo.BannerCount == 0 ? new Color(143, 143, 143) : Color.White;

            DrawBannerFrames(spriteBatch, texture, srcX, srcY, strideY, cellW, frameH, frameCount, color);
        }

        /// <summary>
        /// Draws a mod banner from its own item inventory sprite, scaled to fit the
        /// banner slot while preserving aspect ratio. Fallback for banners with no placeable
        /// tile. Uncollected banners are greyed out, matching the tile-based banners.
        /// </summary>
        private void DrawModBannerIcon(SpriteBatch spriteBatch)
        {
            Main.instance.LoadItem(bannerInfo.ItemId);
            Texture2D texture = TextureAssets.Item[bannerInfo.ItemId].Value;
            Rectangle frame = Main.itemAnimations[bannerInfo.ItemId] != null
                ? Main.itemAnimations[bannerInfo.ItemId].GetFrame(texture)
                : texture.Frame();

            float scale = Math.Min(Width.Pixels / frame.Width, Height.Pixels / frame.Height);
            Vector2 size = new Vector2(frame.Width, frame.Height) * scale;
            Vector2 pos = new Vector2(
                this.Left.Pixels + (Width.Pixels - size.X) / 2f,
                this.Top.Pixels + (Height.Pixels - size.Y) / 2f);

            Color color = bannerInfo.BannerCount == 0 ? new Color(143, 143, 143) : Color.White;
            spriteBatch.Draw(texture, pos, frame, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Draws the collected-count text and the hardmode pin over a banner slot.
        /// Mirrors the overlay drawn by the atlas-based banner path so item-icon
        /// banners look identical.
        /// </summary>
        private void DrawBannerOverlay(SpriteBatch spriteBatch)
        {
            if (bannerInfo.BannerCount > 0)
            {
                string bannerCountText = bannerInfo.BannerCount.ToString();
                float scale = 0.85f;
                Vector2 textSize = FontAssets.MouseText.Value.MeasureString(bannerCountText) * scale;
                Vector2 textPos = new Vector2(this.Left.Pixels - (textSize.X / 2) + 3, this.Top.Pixels + 36);

                spriteBatch.DrawString(FontAssets.MouseText.Value, bannerCountText, textPos + new Vector2(1, 1), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, bannerCountText, textPos + new Vector2(1, -1), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, bannerCountText, textPos + new Vector2(-1, -1), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, bannerCountText, textPos + new Vector2(-1, 1), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, bannerCountText, textPos, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }

            if (bannerInfo.IsHardMode)
            {
                Vector2 spritePos = new Vector2(this.Left.Pixels - 5, this.Top.Pixels - 4);
                Rectangle spriteFrame = new Rectangle(0, 0, 14, 12);
                spriteBatch.Draw(BannerCollectorResources.Pin_HardMode.Value, spritePos, spriteFrame, Color.White);
            }
            else if (bannerInfo.IsPostMoonLord)
            {
                Vector2 spritePos = new Vector2(this.Left.Pixels - 5, this.Top.Pixels - 4);
                Rectangle spriteFrame = new Rectangle(0, 0, 14, 12);
                spriteBatch.Draw(BannerCollectorResources.Pin_PostMoonLoard.Value, spritePos, spriteFrame, Color.White);
            }
        }

        public void SetBannerInfo(BannerInfo bannerInfo)
        {
            this.bannerInfo = bannerInfo;
        }
    }

    //internal class CheckMark : BannerUIElements
    //{
    //    public CheckMark()
    //    {
    //        Width.Pixels = 22f; ;
    //        Height.Pixels = 20f;
    //    }

    //    public override void Update(GameTime gameTime) { }

    //    public override void Draw(SpriteBatch spriteBatch)
    //    {
    //        if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
    //            HideMouseOverInteractions();

    //        base.Draw(spriteBatch);

    //        // 패널 그리기
    //        Rectangle inner = GetInnerDimensions().ToRectangle();
    //        spriteBatch.Draw(BannerCollecterResources.Check_Check.Value, inner, Color.White);

    //        // 화면 중앙 좌표 계산
    //        Vector2 centerScreen = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);

    //        // 아이템 ID 1683의 텍스처 가져오기
    //        Item i = new Item();

    //        Texture2D itemTexture = TextureAssets.Tile[91].Value; // 아이템 텍스처
    //        Rectangle itemFrame = itemTexture.Bounds; // 텍스처의 프레임 정보
    //        Rectangle sourceRectangle = new Rectangle(10, 10, 50, 50);
    //        // 아이템 이미지 를 중앙에 그리기
    //        Vector2 itemPosition = new Vector2(80, 80);
    //        spriteBatch.Draw(itemTexture, itemPosition, sourceRectangle, Color.White);


    //    }
    //}

    //internal class ButtonCheckBox : BannerUIElements
    //{
    //    public ButtonCheckBox()
    //    {
    //        Width.Pixels = 22f; ;
    //        Height.Pixels = 20f;
    //    }

    //    public override void Update(GameTime gameTime) { }

    //    public override void Draw(SpriteBatch spriteBatch)
    //    {
    //        if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
    //            HideMouseOverInteractions();

    //        base.Draw(spriteBatch);

    //        // 패널 그리기
    //        Rectangle inner = GetInnerDimensions().ToRectangle();
    //        spriteBatch.Draw(BannerCollecterResources.Check_Box.Value, inner, Color.White);
    //    }
    //}

}
