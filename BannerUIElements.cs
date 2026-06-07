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
            Width.Pixels = 425f;
            Height.Pixels = 203f;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                HideMouseOverInteractions();

            base.Draw(spriteBatch);

            // 패널 그리기
            Rectangle inner = GetInnerDimensions().ToRectangle();
            spriteBatch.Draw(BannerCollectorResources.UI_Panel.Value, inner, Color.White);
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
        private string[] filteringText = { "Filter All", "Filter Pre-Hardmode", "Filter Hardmode" };
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
            filteringText.Add("Filter All Mods");
            modNum = filteringText.Count;
            hoverText = filteringText[filterIndex];
        }

        public void SetDefault()
        {
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
                DrawModBannerIcon(spriteBatch);
                DrawBannerOverlay(spriteBatch);
            }
            else if (bannerInfo.Index == -1)
            {
                spriteBatch.Draw(BannerCollectorResources.UI_UndefinedBanner.Value, inner, Color.White);
            }
            else
            {
                Color color = new Color(143, 143, 143);
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
                
                int WidthCount = texture.Width / 18; //한줄에 있는 배너 수
                int imagePosX = (bannerInfo.Index % WidthCount) * 18;
                int imagePosY = (bannerInfo.Index / WidthCount) * 54;
                Vector2 spritePos = new Vector2(this.Left.Pixels, this.Top.Pixels);
                Rectangle spriteFrame = new Rectangle(imagePosX, imagePosY, 16, 16);
                float textureScale = 1.02f;
                if (bannerInfo.BannerCount == 0)
                    spriteBatch.Draw(texture, spritePos, spriteFrame, color, 0f, Vector2.Zero, textureScale, SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(texture, spritePos, spriteFrame, Color.White, 0f, Vector2.Zero, textureScale, SpriteEffects.None, 0f);

                spritePos = new Vector2(this.Left.Pixels, this.Top.Pixels + 16);
                spriteFrame = new Rectangle(imagePosX, imagePosY + 18, 16, 16);
                if (bannerInfo.BannerCount == 0)
                    spriteBatch.Draw(texture, spritePos, spriteFrame, color, 0f, Vector2.Zero, textureScale, SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(texture, spritePos, spriteFrame, Color.White, 0f, Vector2.Zero, textureScale, SpriteEffects.None, 0f);

                spritePos = new Vector2(this.Left.Pixels, this.Top.Pixels + 32);
                spriteFrame = new Rectangle(imagePosX, imagePosY + 36, 16, 16);
                if (bannerInfo.BannerCount == 0)
                    spriteBatch.Draw(texture, spritePos, spriteFrame, color, 0f, Vector2.Zero, textureScale, SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(texture, spritePos, spriteFrame, Color.White, 0f, Vector2.Zero, textureScale, SpriteEffects.None, 0f);

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
                    spritePos = new Vector2(this.Left.Pixels - 5, this.Top.Pixels - 4);
                    spriteFrame = new Rectangle(0, 0, 14, 12);
                    spriteBatch.Draw(BannerCollectorResources.Pin_HardMode.Value, spritePos, spriteFrame, Color.White);
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
        /// Draws a mod banner from its own item inventory sprite, scaled to fit the
        /// banner slot while preserving aspect ratio. Used for banners flagged with
        /// <see cref="BannerInfo.UseItemIcon"/> (mods without a single banner atlas).
        /// Uncollected banners are greyed out, matching the atlas-based banners.
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
