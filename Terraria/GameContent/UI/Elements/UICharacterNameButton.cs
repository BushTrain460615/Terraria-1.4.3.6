﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UICharacterNameButton
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
  public class UICharacterNameButton : UIElement
  {
    private readonly Asset<Texture2D> _BasePanelTexture;
    private readonly Asset<Texture2D> _selectedBorderTexture;
    private readonly Asset<Texture2D> _hoveredBorderTexture;
    private bool _hovered;
    private bool _soundedHover;
    private readonly LocalizedText _textToShowWhenEmpty;
    private string actualContents;
    private UIText _text;
    private UIText _title;
    public readonly LocalizedText Description;
    public float DistanceFromTitleToOption = 20f;

    public UICharacterNameButton(
      LocalizedText titleText,
      LocalizedText emptyContentText,
      LocalizedText description = null)
    {
      this.Width = StyleDimension.FromPixels(400f);
      this.Height = StyleDimension.FromPixels(40f);
      this.Description = description;
      this._BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", (AssetRequestMode) 1);
      this._selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode) 1);
      this._hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", (AssetRequestMode) 1);
      this._textToShowWhenEmpty = emptyContentText;
      float textScale = 1f;
      UIText uiText1 = new UIText(titleText, textScale);
      uiText1.HAlign = 0.0f;
      uiText1.VAlign = 0.5f;
      uiText1.Left = StyleDimension.FromPixels(10f);
      UIText element1 = uiText1;
      this.Append((UIElement) element1);
      this._title = element1;
      UIText uiText2 = new UIText(Language.GetText("UI.PlayerNameSlot"), textScale);
      uiText2.HAlign = 0.0f;
      uiText2.VAlign = 0.5f;
      uiText2.Left = StyleDimension.FromPixels(150f);
      UIText element2 = uiText2;
      this.Append((UIElement) element2);
      this._text = element2;
      this.SetContents((string) null);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
      if (this._hovered)
      {
        if (!this._soundedHover)
          SoundEngine.PlaySound(12);
        this._soundedHover = true;
      }
      else
        this._soundedHover = false;
      CalculatedStyle dimensions = this.GetDimensions();
      Utils.DrawSplicedPanel(spriteBatch, this._BasePanelTexture.Value, (int) dimensions.X, (int) dimensions.Y, (int) dimensions.Width, (int) dimensions.Height, 10, 10, 10, 10, Color.White * 0.5f);
      if (!this._hovered)
        return;
      Utils.DrawSplicedPanel(spriteBatch, this._hoveredBorderTexture.Value, (int) dimensions.X, (int) dimensions.Y, (int) dimensions.Width, (int) dimensions.Height, 10, 10, 10, 10, Color.White);
    }

    public void SetContents(string name)
    {
      this.actualContents = name;
      if (string.IsNullOrEmpty(this.actualContents))
      {
        this._text.TextColor = Color.Gray;
        this._text.SetText(this._textToShowWhenEmpty);
      }
      else
      {
        this._text.TextColor = Color.White;
        this._text.SetText(this.actualContents);
      }
      this._text.Left = StyleDimension.FromPixels(this._title.GetInnerDimensions().Width + this.DistanceFromTitleToOption);
    }

    public void TrimDisplayIfOverElementDimensions(int padding)
    {
      CalculatedStyle dimensions1 = this.GetDimensions();
      Point point1 = new Point((int) dimensions1.X, (int) dimensions1.Y);
      Point point2 = new Point(point1.X + (int) dimensions1.Width, point1.Y + (int) dimensions1.Height);
      Rectangle rectangle1 = new Rectangle(point1.X, point1.Y, point2.X - point1.X, point2.Y - point1.Y);
      CalculatedStyle dimensions2 = this._text.GetDimensions();
      Point point3 = new Point((int) dimensions2.X, (int) dimensions2.Y);
      Point point4 = new Point(point3.X + (int) dimensions2.Width, point3.Y + (int) dimensions2.Height);
      Rectangle rectangle2 = new Rectangle(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
      int num = 0;
      for (; rectangle2.Right > rectangle1.Right - padding; rectangle2 = new Rectangle(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y))
      {
        this._text.SetText(this._text.Text.Substring(0, this._text.Text.Length - 1));
        ++num;
        this.RecalculateChildren();
        CalculatedStyle dimensions3 = this._text.GetDimensions();
        point3 = new Point((int) dimensions3.X, (int) dimensions3.Y);
        point4 = new Point(point3.X + (int) dimensions3.Width, point3.Y + (int) dimensions3.Height);
      }
      if (num <= 0)
        return;
      this._text.SetText(this._text.Text.Substring(0, this._text.Text.Length - 1) + "…");
    }

    public override void MouseDown(UIMouseEvent evt) => base.MouseDown(evt);

    public override void MouseOver(UIMouseEvent evt)
    {
      base.MouseOver(evt);
      this._hovered = true;
    }

    public override void MouseOut(UIMouseEvent evt)
    {
      base.MouseOut(evt);
      this._hovered = false;
    }
  }
}
