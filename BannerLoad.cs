using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terraria;
using Terraria.ModLoader;

namespace BannerCollector
{
    internal static partial class BannerLoad
    {
        public static Dictionary<int, BannerInfo> BannerDict = new Dictionary<int, BannerInfo>();
        public static List<string> ModList = new List<string>();
        public static bool isModded = false;
        public static Dictionary<string, Asset<Texture2D>> ModBannerTexture = new Dictionary<string, Asset<Texture2D>>();
        public static void LoadBanners()
        {
            BannerDict.Clear();
            BannerInfo bannerInfo;
            Item item;

            #region Slimes
            bannerInfo = new BannerInfo();
            item = new Item(1683);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 90;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2928);
            bannerInfo.ItemId = 2928;
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 140;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2964);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 176;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1690);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 97;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2968);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 180;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2992);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 204;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2899);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 111;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2955);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 167;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2916);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 128;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2960);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 172;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2940);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 152;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2981);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 193;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2935);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 147;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2980);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 192;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3593);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 270;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2944);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 156;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(5352);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 310;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //HardMode

            bannerInfo = new BannerInfo();
            item = new Item(1689);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 96;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2908);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 120;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2976);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 188;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2910);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 122;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1651);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 58;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2938);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 150;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2966);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 178;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Surface
            bannerInfo = new BannerInfo();
            item = new Item(1622);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 29;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1628);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 35;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1657);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 64;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1701);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 108;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1639);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 46;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1655);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 62;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4688);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 296;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1658);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 65;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //HardMode
            bannerInfo = new BannerInfo();
            item = new Item(2962);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 174;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4977);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 309;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1699);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 106;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1695);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 102;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1700);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 107;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Ocean
            bannerInfo = new BannerInfo();
            item = new Item(1634);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 41;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3447);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 264;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3594);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 271;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1680);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 87;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2983);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 194;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Carvern
            bannerInfo = new BannerInfo();
            item = new Item(1621);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 28;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2906);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 154;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3393);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 210;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3392);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 209;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1698);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 105;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1694);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 101;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3391);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 208;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1681);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 88;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2986);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 198;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2987);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 199;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1665);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 72;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //HardMode
            bannerInfo = new BannerInfo();
            item = new Item(1620);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 27;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2923);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 135;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1630);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 37;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4965);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 297;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2969);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 181;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2973);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 185;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1615);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 22;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3448);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 265;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Granite Cave
            bannerInfo = new BannerInfo();
            item = new Item(3407);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 224;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3408);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 225;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Marble Cave
            bannerInfo = new BannerInfo();
            item = new Item(3406);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 223;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(3405);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 222;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Spider Nest
            bannerInfo = new BannerInfo();
            item = new Item(1685);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 92;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(1623);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 30;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);
            #endregion

            #region Glowing Mushroom biome
            bannerInfo = new BannerInfo();
            item = new Item(1617);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 24;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1649);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 56;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1672);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 79;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4968);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 300;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4967);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 299;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1686);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 93;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(1650);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 57;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Snow biome
            bannerInfo = new BannerInfo();
            item = new Item(2912);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 124;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2933);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 145;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2959);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 171;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1684);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 91;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2988);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 200;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1643);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 50;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(2898);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 110;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1662);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 69;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3452);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 269;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1663);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 70;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1674);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 81;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1696);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 103;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Jungle
            bannerInfo = new BannerInfo();
            item = new Item(2915);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 127;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1661);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 68;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2939);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 151;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2942);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 118;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1668);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 75;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2977);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 189;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1675);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 82;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(2897);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 109;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1640);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 47;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2925);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 137;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1688);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 95;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1666);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 73;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4976);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 308;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1670);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 77;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1619);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 26;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1647);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 54;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1667);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 74;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);
            #endregion

            #region Desert
            bannerInfo = new BannerInfo();
            item = new Item(1618);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 25;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3414);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 231;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4969);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 301;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3413);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 230;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3411);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 228;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1693);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 100;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(3418);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 235;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3417);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 234;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3412);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 229;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3415);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 232;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3416);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 233;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1671);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 78;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3419);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 236;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Underworld
            bannerInfo = new BannerInfo();
            item = new Item(1627);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 34;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1638);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 45;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1664);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 71;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1659);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 66;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(2943);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 155;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3446);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 263;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Dungeon
            bannerInfo = new BannerInfo();
            item = new Item(3451);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 268;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2911);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 123;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1682);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 89;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(2900);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 112;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2970);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 182;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2930);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 142;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2914);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 126;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2956);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 168;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2965);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 177;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2904);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 116;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2974);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 186;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2975);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 187;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2984);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 196;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2917);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 129;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2924);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 136;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2958);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 170;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Corruption
            bannerInfo = new BannerInfo();
            item = new Item(2913);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 125;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1641);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 48;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(2905);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 117;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4973);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 305;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2909);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 121;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1637);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 44;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3449);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 266;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1697);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 104;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Crimson
            bannerInfo = new BannerInfo();
            item = new Item(1626);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 33;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1635);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 42;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1644);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 51;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(1624);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 31;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1625);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 32;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4966);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 298;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1636);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 43;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4974);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 306;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1645);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 52;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1660);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 67;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2936);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 148;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Hallow
            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(1629);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 36;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1642);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 49;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4975);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 307;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2937);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 149;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3450);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 267;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1677);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 84;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1691);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 98;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Meteorite
            bannerInfo = new BannerInfo();
            item = new Item(1669);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 76;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Rain/Blizzard
            bannerInfo = new BannerInfo();
            item = new Item(1646);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 53;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1678);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 85;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(1616);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 23;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2934);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 146;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Windy Day
            bannerInfo = new BannerInfo();
            item = new Item(4687);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 295;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Blood Moon
            bannerInfo = new BannerInfo();
            item = new Item(3409);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 226;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1632);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 39;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1633);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 40;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2907);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 119;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3410);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 227;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2985);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 197;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4541);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 288;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4970);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 302;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4971);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 303;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4972);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 304;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4543);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 290;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4542);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 289;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(1631);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 38;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4545);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 292;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4544);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 291;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4546);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 293;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(4602);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 294;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Goblin Army
            bannerInfo = new BannerInfo();
            item = new Item(2927);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 139;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1654);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 61;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1653);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 60;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1652);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 59;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1656);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 63;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(3390);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 207;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Sandstorm

            bannerInfo = new BannerInfo();
            item = new Item(3793);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 277;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(3780);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 272;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3789);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 273;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3792);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 276;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3790);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 274;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3791);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 275;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Halloween
            bannerInfo = new BannerInfo();
            item = new Item(2967);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 179;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2922);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 134;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(2932);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 144;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Pirate Invasion
            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(1673);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 80;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3441);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 258;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3443);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 260;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3444);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 261;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3442);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 259;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1676);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 83;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Solar Eclipse
            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(3400);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 217;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3395);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 212;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3402);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 219;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3396);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 213;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2920);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 132;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1648);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 55;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3394);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 211;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3397);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 214;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3403);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 220;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3401);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 218;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1679);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 86;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1687);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 94;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3399);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 216;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(1692);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 99;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Frost Legion
            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(2954);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 166;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2978);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 190;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2979);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 191;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Pumpkin Moon
            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(2929);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 141;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2931);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 143;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2961);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 173;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2971);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 183;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2982);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 194;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Frost Moon
            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(2918);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 130;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2919);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 131;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2921);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 133;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2926);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 138;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2941);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 153;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2957);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 169;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2963);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 175;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2993);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 205;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2994);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 206;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Martian Madness
            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(2945);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 157;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2946);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 158;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2947);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 159;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2948);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 160;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2949);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 161;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2950);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 162;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2951);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 163;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2952);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 164;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2953);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 165;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3445);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 262;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2972);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 184;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            #region Lunar Events
            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(2901);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 113;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(2902);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 114;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3438);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 255;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3436);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 253;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3437);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 254;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3440);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 267;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3439);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 256;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3433);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 250;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3435);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 252;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3434);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 251;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3432);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 249;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3422);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 239;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3421);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 238;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3424);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 241;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3425);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 242;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3420);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 237;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3423);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 240;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3426);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 243;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3428);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 245;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3430);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 247;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3429);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 246;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3431);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 248;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3427);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 244;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);


            #endregion

            #region Old One's Army
            bannerInfo = new BannerInfo();
            item = new Item(3845);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 286;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3838);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 279;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3837);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 278;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3844);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 285;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3839);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = false;
            bannerInfo.Index = 280;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            //Hardmode
            bannerInfo = new BannerInfo();
            item = new Item(3840);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 281;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3846);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 287;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3842);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 283;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3841);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 282;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            bannerInfo = new BannerInfo();
            item = new Item(3843);
            bannerInfo.ItemId = item.type;
            bannerInfo.ItemName = item.Name;
            bannerInfo.BannerCount = 0;
            bannerInfo.IsHardMode = true;
            bannerInfo.Index = 284;
            BannerDict.Add(bannerInfo.ItemId, bannerInfo);

            #endregion

            if (isModded)
            {
                LoadModBanners();
            }
        }
        
    }


    /* 윗라인 첫배너 좌표: 47, 50
     * 아래라인 배너 좌표: 47, 114
     * 좌우 간격: 35 
     */

    internal class BannerInfo
    {
        private int itemId = -1;
        private int bannerCount = 0;
        private string itemName = "";
        private bool isHardMode = false;
        private int index = -1; //Tiles_91 파일의 배너 순서
        private string modName = null;
        private bool useItemIcon = false;
        private int tileType = -1;

        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; } // 필요에 따라 유효성 검사 추가 가능
        }

        public int BannerCount
        {
            get { return bannerCount; }
            set
            {
                if (value >= 0) // 예시로, 배너 수는 0 이상이어야 한다는 유효성 검사
                {
                    bannerCount = value;
                }
            }
        }

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; } // 필요에 따라 유효성 검사 추가 가능
        }

        public bool IsHardMode
        {
            get { return isHardMode; }
            set { isHardMode = value; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public string ModName
        {
            get { return modName; }
            set { modName = value; }
        }

        /// <summary>
        /// When true, this is a mod banner resolved at runtime: it is drawn from its actual
        /// banner tile (<see cref="TileType"/> + <see cref="Index"/> read from the item's
        /// createTile/placeStyle), which keeps the authentic 16x48 banner look without any
        /// hardcoded atlas indices. Falls back to the item inventory sprite if the banner has
        /// no placeable tile. Used by Calamity, Catalyst, Thorium, Spirit and Spirit Reforged.
        /// </summary>
        public bool UseItemIcon
        {
            get { return useItemIcon; }
            set { useItemIcon = value; }
        }

        /// <summary>
        /// Tile type this banner places (item.createTile), or -1 if it has none. For
        /// <see cref="UseItemIcon"/> banners the UI draws this tile's atlas frame selected by
        /// <see cref="Index"/> (the item's placeStyle), giving the same look as vanilla banners.
        /// </summary>
        public int TileType
        {
            get { return tileType; }
            set { tileType = value; }
        }
    }
}
