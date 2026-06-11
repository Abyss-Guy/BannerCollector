using System;

namespace BannerCollector
{
    /// <summary>
    /// Single source of truth for the collection window's geometry.
    ///
    /// The window is one baked art sheet (Resources/UI_Board.png, 425x203) whose 10x2 grid is
    /// painted in. To support any column/row count it is rebuilt from that sheet as a true 9-slice
    /// (see <see cref="BannerPanel.Draw"/>): the four corners are kept 1:1, the four edges are
    /// stretched/tiled along a single axis, and the centre (the pill cells) is tiled in both. Every
    /// pixel constant below was measured directly from the sheet.
    ///
    /// The centre is pure pills+beige with NO frame pixels, so tiling can never repeat a frame
    /// fragment between cells. The right edge is the left edge MIRRORED, so all four corner
    /// "thickenings" are exact symmetric copies (the original sheet draws the left and right corners
    /// slightly differently; mirroring makes them match). Because the pill sits off-centre in its
    /// cell, the right edge is slid inward by RightEdgeOverlap so the pill-to-frame gap is identical
    /// on both sides and the whole window is mirror-symmetric. The window is then Columns*35 + 73
    /// (423 at 10 columns) - a couple of px narrower than the original 425, which only adds margin.
    /// </summary>
    internal static class BannerGrid
    {
        // --- Slot placement (panel-relative). The banner sits inside the pill's grey recess (sheet
        // x 45..64, centre 54.5), so the 16px-wide slot is centred there: 54.5 - 16/2 = 46.5, which
        // rounds to 47 (slot pixels 47..62 leave an even 2px of recess on each side). Note the pill's
        // painted outline is thicker on the left than the right (4px vs 2px dark border, an asymmetry
        // baked into the sheet), so its outer centre (53.5) sits 1px left of the recess centre; the
        // banner is centred on the recess it actually occupies. ---
        public const int FirstSlotX = 47; // left of the first column's slot (centres the banner in the pill recess)
        public const int FirstSlotY = 51; // top of the first row's slot
        public const int CellPitchX = 35; // horizontal distance between columns
        public const int CellPitchY = 64; // vertical distance between rows

        // --- Horizontal layout: [left edge | N pill columns | right edge]. Both edges are the SAME
        // art (sheet x 0..38); the right edge is the left one mirrored, so all four corners are exact
        // symmetric copies. The pill sits off-centre in its cell (2px of padding on the left, 7px on
        // the right), so the right edge is slid inward by that 5px difference (RightEdgeOverlap),
        // which makes the pill-to-frame gap identical on both sides (8px) and the whole window
        // mirror-symmetric. The slide overlaps the last pill column's (beige) right padding, which is
        // harmless. ---
        public const int EdgeW = 39;            // left/right edge strip: sheet x 0..38 (border + inner vertical + beige)
        public const int RightEdgeOverlap = 5;  // = right pill padding (7) - left pill padding (2); see above
        public const int CellW = 35;    // one pill column (tiled); pitch matches CellPitchX
        public const int CellArtX = 39; // pill column source x (one pill, beige padding only -> x 39..73)

        // --- Vertical layout: [top edge | M pill rows | footer]. ---
        public const int TopEdgeH = 44; // top border + frame line (sheet y 0..43)
        public const int CellH = 64;    // one pill row (tiled); pitch matches CellPitchY
        public const int CellArtY = 44; // pill row source y (one row of pills, beige padding only -> y 44..107)
        public const int FooterH = 31;  // bottom frame line + page-dot band + border (sheet y 172..202)

        // Footer sub-bands, measured from the footer's top (sheet y 172).
        public const int FooterArtY = 172;
        public const int FooterLineH = 4;    // bottom frame line (sheet y 172..175)
        public const int FooterBandH = 16;   // page-dot band     (sheet y 176..191) - the stretched slice
        public const int FooterBottomH = 11; // bottom border     (sheet y 192..202)

        // --- Left/right edge strip. It is a vertical 9-slice of its own, drawn from the LEFT edge art
        // (sheet x 0..38); the right edge draws the same art mirrored horizontally, so all four corner
        // bulges are symmetric. The inner vertical line bulges outward at the top (sheet y 44..51) and
        // bottom (y 166..171) corners and is straight (a uniform 1px row) in between - so the corner
        // pieces are kept 1:1 and only the straight middle is stretched. This stops the bulge from
        // repeating at every row (the side "sticks") and keeps the real corners. ---
        public const int EdgeTopCornerH = 52; // sheet y 0..51    : top border + frame line + top bulge
        public const int EdgeMidArtY = 100;   // a uniform row of the straight vertical (stretched, 1px tall)
        public const int EdgeBulgeArtY = 166; // sheet y 166..171 : bottom bulge, drawn just above the footer
        public const int EdgeBulgeH = 6;
        public const int EdgeBandCapH = 2;    // sheet y 176..177 : bottom corner block at the band's top.
                                              // Kept 1:1 so it never thickens; only the beige below it
                                              // stretches when the page-dot band grows for extra dot rows.

        // A column that is horizontally uniform within every frame band (top edge + footer sub-bands),
        // used to stretch those bands across the centre width without repeating any detail.
        public const int FrameMidArtX = 200;

        // --- Configurable counts. Only a lower bound is enforced, as a safety floor: the minimum
        // column count keeps the header buttons (which stay on the left) from overlapping the close
        // button (right edge), and a grid needs at least one row. The upper bounds are intentionally
        // far beyond any reasonable on-screen size, so there is effectively no cap on how many
        // columns/rows the player can set. ---
        public const int MinColumns = 5;
        public const int MaxColumns = 100;
        public const int MinRows = 1;
        public const int MaxRows = 100;
        public const int DefaultColumns = 10;
        public const int DefaultRows = 2;

        public static int Columns => Math.Clamp(BannerCollectorConfig.Instance?.BannerColumns ?? DefaultColumns, MinColumns, MaxColumns);
        public static int Rows => Math.Clamp(BannerCollectorConfig.Instance?.BannerRows ?? DefaultRows, MinRows, MaxRows);

        /// <summary>Banners shown per page (one full grid).</summary>
        public static int PageSize => Columns * Rows;

        /// <summary>Panel width that fits the current column count.</summary>
        public static float PanelWidth => EdgeW * 2 + Columns * CellW - RightEdgeOverlap; // Columns*35 + 73

        /// <summary>Panel height for the current row count with a single page-dot row.</summary>
        public static float DesignHeight => TopEdgeH + Rows * CellH + FooterH; // Rows*64 + 75

        /// <summary>Panel-relative Y of the page-dot band (after the footer's frame line).</summary>
        public static float PageDotBandTop => TopEdgeH + Rows * CellH + FooterLineH;
    }
}
