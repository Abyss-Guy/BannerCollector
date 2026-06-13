using Microsoft.Xna.Framework;
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

namespace BannerCollector.Resources
{
    internal class BannerCollectorResources
    {
        public static Asset<Texture2D> UI_Panel;
        public static Asset<Texture2D> UI_BannerBorder;
        public static Asset<Texture2D> UI_UndefinedBanner;
        public static Asset<Texture2D> UI_Page;
        public static Asset<Texture2D> Button_Banner;
        public static Asset<Texture2D> Button_Left;
        public static Asset<Texture2D> Button_Right;
        public static Asset<Texture2D> Button_Sort;
        public static Asset<Texture2D> Button_Filter;
        public static Asset<Texture2D> Button_FilterOn;
        public static Asset<Texture2D> Button_FilterMode;
        public static Asset<Texture2D> Button_FilterPreHard;
        public static Asset<Texture2D> Button_FilterHard;
        public static Asset<Texture2D> Button_FilterPostMoonLord;
        public static Asset<Texture2D> Button_FilterMod;
        public static Asset<Texture2D> Button_FilterModOn;
        public static Asset<Texture2D> Button_Close;
        public static Asset<Texture2D> Pin_HardMode;
        public static Asset<Texture2D> Pin_PostMoonLoard;


        private static Asset<Texture2D> RequestResource(string path) => ModContent.Request<Texture2D>("BannerCollector/Resources/" + path);

        public static void PreloadAssets()
        {
            UI_Panel = RequestResource("UI_Board");
            UI_BannerBorder = RequestResource("Banner_Button_Border");
            UI_UndefinedBanner = RequestResource("UI_UndefinedBanner");
            UI_Page = RequestResource("UI_Page");
            Button_Banner = RequestResource("Banner_Button");
            Button_Left = RequestResource("UI_Left");
            Button_Right = RequestResource("UI_Right");
            Button_Sort = RequestResource("UI_Sort");
            Button_Filter = RequestResource("UI_Filter");
            Button_FilterOn = RequestResource("UI_FilterOn");
            Button_FilterMode = RequestResource("UI_FilterMode");
            Button_FilterPreHard = RequestResource("UI_FilterPreHard");
            Button_FilterHard = RequestResource("UI_FilterHard");
            Button_FilterPostMoonLord = RequestResource("UI_FilterPostMoonLord");
            Button_FilterMod = RequestResource("UI_FilterMod");
            Button_FilterModOn = RequestResource("UI_FilterModOn");
            Button_Close = RequestResource("UI_Close");
            Pin_HardMode = RequestResource("Pin_HardMode");
            Pin_PostMoonLoard = RequestResource("Pin_PostMoonLord");

        }

    }
}
