using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using BannerCollector.Dusts;
using Terraria.ID;

namespace BannerCollector.Tiles
{
    /// <summary>
    /// This is tile that only provide banner buffs User in game can't access this tile.
    /// </summary>
    internal class BannerBuffTile : ModTile
    {
        static List<BannerInfo> bannerListToBuff = new List<BannerInfo>();

        static int? preTileX;
        static int? preTileY;
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = false; // 견고한 블록이 아님
            Main.tileBlockLight[Type] = false; // 빛 차단
            Main.tileLighted[Type] = false; // 빛을 발산하지 않음
            Main.tileNoAttach[Type] = true; // 타일에 부착할 수 없음
            Main.tileCut[Type] = false; // 부술 수 없는 타일로 설정
            DustType = ModContent.DustType<BuffTileDust>();
        }

        public override void Load()
        {
            base.Load();

        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (!BannerCollectorConfig.Instance.EnableBannerBuff)
            {
                Main.SceneMetrics.hasBanner = false;
                return;
            }
            foreach (var banner in bannerListToBuff)
            {
                // BannerId is the real engine banner number, resolved once at load for vanilla and
                // modded banners alike (BannerLoad.BuildBannerItemToBannerId). -1 means it could not
                // be resolved (logged into SkippedBanners) and the banner simply grants no buff.
                if (banner.BannerId < 0)
                    continue;
                Main.SceneMetrics.NPCBannerBuff[banner.BannerId] = true;
            }
            Main.SceneMetrics.hasBanner = true;
        }

        public override bool KillSound(int i, int j, bool fail)
        {
            return false;
        }

        public static void UpdateTilePosition(Player player)
        {
            int tileX = (int)(player.position.X / 16);
            int tileY = (int)(player.position.Y / 16);
            //리스트 변경
            bannerListToBuff = BannerLoad.BannerDict.Values.ToList();
            bannerListToBuff.RemoveAll(banner => banner.BannerCount == 0);

            if (!Main.tile[tileX, tileY - 10].HasTile)
                tileY -= 10;
            else if (!Main.tile[tileX, tileY + 10].HasTile)
                tileY += 10;
            else if (!Main.tile[tileX - 10, tileY].HasTile)
                tileX -= 10;
            else if (!Main.tile[tileX + 10, tileY].HasTile)
                tileX += 10;
            else
            {
                return;
            }
            // 현재 위치에 아무런 타일이 없을 때만 배치
            if (preTileX != null && preTileY != null)
            {
                WorldGen.KillTile((int)preTileX, (int)preTileY, false, false, true);
                // 새로운 타일 배치
                WorldGen.PlaceTile(tileX, tileY, ModContent.TileType<BannerBuffTile>(), true, true);
                preTileX = tileX;
                preTileY = tileY;
            }
            else
            {
                WorldGen.PlaceTile(tileX, tileY, ModContent.TileType<BannerBuffTile>(), true, true);
                preTileX = tileX;
                preTileY = tileY;
            }
        }
    }
}
