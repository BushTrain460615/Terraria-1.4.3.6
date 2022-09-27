// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIWorkshopPublishWorldListItem
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
  public class UIWorkshopPublishWorldListItem : UIPanel
  {
    private WorldFileData _data;
    private Asset<Texture2D> _workshopIconTexture;
    private Asset<Texture2D> _innerPanelTexture;
    private UIImage _worldIcon;
    private UIElement _publishButton;
    private int _orderInList;
    private UIState _ownerState;

    public UIWorkshopPublishWorldListItem(UIState ownerState, WorldFileData data, int orderInList)
    {
      this._ownerState = ownerState;
      this._orderInList = orderInList;
      this._data = data;
      this.LoadTextures();
      this.InitializeAppearance();
      this._worldIcon = new UIImage(this.GetIcon());
      this._worldIcon.Left.Set(4f, 0.0f);
      this._worldIcon.VAlign = 0.5f;
      this._worldIcon.OnDoubleClick += new UIElement.MouseEvent(this.PublishButtonClick_ImportWorldToLocalFiles);
      this.Append((UIElement) this._worldIcon);
      this._publishButton = (UIElement) new UIIconTextButton(Language.GetText("Workshop.Publish"), Color.White, "Images/UI/Workshop/Publish");
      this._publishButton.HAlign = 1f;
      this._publishButton.VAlign = 1f;
      this._publishButton.OnClick += new UIElement.MouseEvent(this.PublishButtonClick_ImportWorldToLocalFiles);
      this.OnDoubleClick += new UIElement.MouseEvent(this.PublishButtonClick_ImportWorldToLocalFiles);
      this.Append(this._publishButton);
      this._publishButton.SetSnapPoint("Publish", orderInList);
    }

    private void LoadTextures()
    {
      this._innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", (AssetRequestMode) 1);
      this._workshopIconTexture = TextureAssets.Extra[243];
    }

    private void InitializeAppearance()
    {
      this.Height.Set(82f, 0.0f);
      this.Width.Set(0.0f, 1f);
      this.SetPadding(6f);
      this.SetColorsToNotHovered();
    }

    private void SetColorsToHovered()
    {
      this.BackgroundColor = new Color(73, 94, 171);
      this.BorderColor = new Color(89, 116, 213);
    }

    private void SetColorsToNotHovered()
    {
      this.BackgroundColor = new Color(63, 82, 151) * 0.7f;
      this.BorderColor = new Color(89, 116, 213) * 0.7f;
    }

    private Asset<Texture2D> GetIcon()
    {
      if (this._data.DrunkWorld)
        return Main.Assets.Request<Texture2D>("Images/UI/Icon" + (this._data.IsHardMode ? "Hallow" : "") + "CorruptionCrimson", (AssetRequestMode) 1);
      if (this._data.ForTheWorthy)
        return this.GetSeedIcon("FTW");
      if (this._data.NotTheBees)
        return this.GetSeedIcon("NotTheBees");
      if (this._data.Anniversary)
        return this.GetSeedIcon("Anniversary");
      return this._data.DontStarve ? this.GetSeedIcon("DontStarve") : Main.Assets.Request<Texture2D>("Images/UI/Icon" + (this._data.IsHardMode ? "Hallow" : "") + (this._data.HasCorruption ? "Corruption" : "Crimson"), (AssetRequestMode) 1);
    }

    private Asset<Texture2D> GetSeedIcon(string seed) => Main.Assets.Request<Texture2D>("Images/UI/Icon" + (this._data.IsHardMode ? "Hallow" : "") + (this._data.HasCorruption ? "Corruption" : "Crimson") + seed, (AssetRequestMode) 1);

    private void PublishButtonClick_ImportWorldToLocalFiles(
      UIMouseEvent evt,
      UIElement listeningElement)
    {
      if (listeningElement != evt.Target)
        return;
      Main.MenuUI.SetState((UIState) new WorkshopPublishInfoStateForWorld(this._ownerState, this._data));
    }

    public override int CompareTo(object obj) => obj is UIWorkshopPublishWorldListItem publishWorldListItem ? this._orderInList.CompareTo(publishWorldListItem._orderInList) : base.CompareTo(obj);

    public override void MouseOver(UIMouseEvent evt)
    {
      base.MouseOver(evt);
      this.SetColorsToHovered();
    }

    public override void MouseOut(UIMouseEvent evt)
    {
      base.MouseOut(evt);
      this.SetColorsToNotHovered();
    }

    private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width, float height) => Utils.DrawSplicedPanel(spriteBatch, this._innerPanelTexture.Value, (int) position.X, (int) position.Y, (int) width, (int) height, 10, 10, 10, 10, Color.White);

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
      base.DrawSelf(spriteBatch);
      CalculatedStyle innerDimensions = this.GetInnerDimensions();
      CalculatedStyle dimensions = this._worldIcon.GetDimensions();
      float num1 = dimensions.X + dimensions.Width;
      Color color1 = this._data.IsValid ? Color.White : Color.Gray;
      Utils.DrawBorderString(spriteBatch, this._data.Name, new Vector2(num1 + 6f, innerDimensions.Y + 3f), color1);
      float num2 = (float) (((double) innerDimensions.Width - 22.0 - (double) dimensions.Width - (double) this._publishButton.GetDimensions().Width) / 2.0);
      float height = this._publishButton.GetDimensions().Height;
      Vector2 position = new Vector2(num1 + 6f, innerDimensions.Y + innerDimensions.Height - height);
      float width1 = num2;
      this.DrawPanel(spriteBatch, position, width1, height);
      Color color2 = Color.White;
      string textValue1;
      switch (this._data.GameMode)
      {
        case 1:
          textValue1 = Language.GetTextValue("UI.Expert");
          color2 = Main.mcColor;
          break;
        case 2:
          textValue1 = Language.GetTextValue("UI.Master");
          color2 = Main.hcColor;
          break;
        case 3:
          textValue1 = Language.GetTextValue("UI.Creative");
          color2 = Main.creativeModeColor;
          break;
        default:
          textValue1 = Language.GetTextValue("UI.Normal");
          break;
      }
      Vector2 vector2_1 = FontAssets.MouseText.Value.MeasureString(textValue1);
      float x1 = vector2_1.X;
      float y1 = vector2_1.Y;
      float x2 = (float) ((double) width1 * 0.5 - (double) x1 * 0.5);
      float num3 = (float) ((double) height * 0.5 - (double) y1 * 0.5);
      Utils.DrawBorderString(spriteBatch, textValue1, position + new Vector2(x2, num3 + 3f), color2);
      position.X += width1 + 5f;
      float width2 = num2;
      if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
        width2 += 40f;
      this.DrawPanel(spriteBatch, position, width2, height);
      string textValue2 = Language.GetTextValue("UI.WorldSizeFormat", (object) this._data.WorldSizeName);
      Vector2 vector2_2 = FontAssets.MouseText.Value.MeasureString(textValue2);
      float x3 = vector2_2.X;
      float y2 = vector2_2.Y;
      float x4 = (float) ((double) width2 * 0.5 - (double) x3 * 0.5);
      float num4 = (float) ((double) height * 0.5 - (double) y2 * 0.5);
      Utils.DrawBorderString(spriteBatch, textValue2, position + new Vector2(x4, num4 + 3f), Color.White);
      position.X += width2 + 5f;
    }
  }
}
