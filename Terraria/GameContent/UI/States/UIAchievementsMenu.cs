﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.States.UIAchievementsMenu
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.Achievements;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
  public class UIAchievementsMenu : UIState
  {
    private UIList _achievementsList;
    private List<UIAchievementListItem> _achievementElements = new List<UIAchievementListItem>();
    private List<UIToggleImage> _categoryButtons = new List<UIToggleImage>();
    private UIElement _backpanel;
    private UIElement _outerContainer;

    public void InitializePage()
    {
      this.RemoveAllChildren();
      this._categoryButtons.Clear();
      this._achievementElements.Clear();
      this._achievementsList = (UIList) null;
      bool largeForOtherLanguages = true;
      int num = largeForOtherLanguages.ToInt() * 100;
      UIElement element1 = new UIElement();
      element1.Width.Set(0.0f, 0.8f);
      element1.MaxWidth.Set(800f + (float) num, 0.0f);
      element1.MinWidth.Set(600f + (float) num, 0.0f);
      element1.Top.Set(220f, 0.0f);
      element1.Height.Set(-220f, 1f);
      element1.HAlign = 0.5f;
      this._outerContainer = element1;
      this.Append(element1);
      UIPanel element2 = new UIPanel();
      element2.Width.Set(0.0f, 1f);
      element2.Height.Set(-110f, 1f);
      element2.BackgroundColor = new Color(33, 43, 79) * 0.8f;
      element2.PaddingTop = 0.0f;
      element1.Append((UIElement) element2);
      this._achievementsList = new UIList();
      this._achievementsList.Width.Set(-25f, 1f);
      this._achievementsList.Height.Set(-50f, 1f);
      this._achievementsList.Top.Set(50f, 0.0f);
      this._achievementsList.ListPadding = 5f;
      element2.Append((UIElement) this._achievementsList);
      UITextPanel<LocalizedText> element3 = new UITextPanel<LocalizedText>(Language.GetText("UI.Achievements"), large: true);
      element3.HAlign = 0.5f;
      element3.Top.Set(-33f, 0.0f);
      element3.SetPadding(13f);
      element3.BackgroundColor = new Color(73, 94, 171);
      element1.Append((UIElement) element3);
      UITextPanel<LocalizedText> element4 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
      element4.Width.Set(-10f, 0.5f);
      element4.Height.Set(50f, 0.0f);
      element4.VAlign = 1f;
      element4.HAlign = 0.5f;
      element4.Top.Set(-45f, 0.0f);
      element4.OnMouseOver += new UIElement.MouseEvent(this.FadedMouseOver);
      element4.OnMouseOut += new UIElement.MouseEvent(this.FadedMouseOut);
      element4.OnClick += new UIElement.MouseEvent(this.GoBackClick);
      element1.Append((UIElement) element4);
      this._backpanel = (UIElement) element4;
      List<Achievement> achievementsList = Main.Achievements.CreateAchievementsList();
      for (int index = 0; index < achievementsList.Count; ++index)
      {
        UIAchievementListItem achievementListItem = new UIAchievementListItem(achievementsList[index], largeForOtherLanguages);
        this._achievementsList.Add((UIElement) achievementListItem);
        this._achievementElements.Add(achievementListItem);
      }
      UIScrollbar uiScrollbar = new UIScrollbar();
      uiScrollbar.SetView(100f, 1000f);
      uiScrollbar.Height.Set(-50f, 1f);
      uiScrollbar.Top.Set(50f, 0.0f);
      uiScrollbar.HAlign = 1f;
      element2.Append((UIElement) uiScrollbar);
      this._achievementsList.SetScrollbar(uiScrollbar);
      UIElement element5 = new UIElement();
      element5.Width.Set(0.0f, 1f);
      element5.Height.Set(32f, 0.0f);
      element5.Top.Set(10f, 0.0f);
      Asset<Texture2D> texture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Categories", (AssetRequestMode) 1);
      for (int index = 0; index < 4; ++index)
      {
        UIToggleImage element6 = new UIToggleImage(texture, 32, 32, new Point(34 * index, 0), new Point(34 * index, 34));
        element6.Left.Set((float) (index * 36 + 8), 0.0f);
        element6.SetState(true);
        element6.OnClick += new UIElement.MouseEvent(this.FilterList);
        this._categoryButtons.Add(element6);
        element5.Append((UIElement) element6);
      }
      element2.Append(element5);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
      for (int index = 0; index < this._categoryButtons.Count; ++index)
      {
        if (this._categoryButtons[index].IsMouseHovering)
        {
          string textValue;
          switch (index)
          {
            case -1:
              textValue = Language.GetTextValue("Achievements.NoCategory");
              break;
            case 0:
              textValue = Language.GetTextValue("Achievements.SlayerCategory");
              break;
            case 1:
              textValue = Language.GetTextValue("Achievements.CollectorCategory");
              break;
            case 2:
              textValue = Language.GetTextValue("Achievements.ExplorerCategory");
              break;
            case 3:
              textValue = Language.GetTextValue("Achievements.ChallengerCategory");
              break;
            default:
              textValue = Language.GetTextValue("Achievements.NoCategory");
              break;
          }
          float x = FontAssets.MouseText.Value.MeasureString(textValue).X;
          Vector2 vector2 = new Vector2((float) Main.mouseX, (float) Main.mouseY) + new Vector2(16f);
          if ((double) vector2.Y > (double) (Main.screenHeight - 30))
            vector2.Y = (float) (Main.screenHeight - 30);
          if ((double) vector2.X > (double) Main.screenWidth - (double) x)
            vector2.X = (float) (Main.screenWidth - 460);
          Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, textValue, vector2.X, vector2.Y, new Color((int) Main.mouseTextColor, (int) Main.mouseTextColor, (int) Main.mouseTextColor, (int) Main.mouseTextColor), Color.Black, Vector2.Zero);
          break;
        }
      }
      this.SetupGamepadPoints(spriteBatch);
    }

    public void GotoAchievement(Achievement achievement) => this._achievementsList.Goto((UIList.ElementSearchMethod) (element => element is UIAchievementListItem achievementListItem && achievementListItem.GetAchievement() == achievement));

    private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
    {
      Main.menuMode = 0;
      IngameFancyUI.Close();
    }

    private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
    {
      SoundEngine.PlaySound(12);
      ((UIPanel) evt.Target).BackgroundColor = new Color(73, 94, 171);
      ((UIPanel) evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
    }

    private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
    {
      ((UIPanel) evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
      ((UIPanel) evt.Target).BorderColor = Color.Black;
    }

    private void FilterList(UIMouseEvent evt, UIElement listeningElement)
    {
      SoundEngine.PlaySound(12);
      this._achievementsList.Clear();
      foreach (UIAchievementListItem achievementElement in this._achievementElements)
      {
        if (this._categoryButtons[(int) achievementElement.GetAchievement().Category].IsOn)
          this._achievementsList.Add((UIElement) achievementElement);
      }
      this.Recalculate();
    }

    public override void OnActivate()
    {
      this.InitializePage();
      if (Main.gameMenu)
      {
        this._outerContainer.Top.Set(220f, 0.0f);
        this._outerContainer.Height.Set(-220f, 1f);
      }
      else
      {
        this._outerContainer.Top.Set(120f, 0.0f);
        this._outerContainer.Height.Set(-120f, 1f);
      }
      this._achievementsList.UpdateOrder();
      if (!PlayerInput.UsingGamepadUI)
        return;
      UILinkPointNavigator.ChangePoint(3002);
    }

    private void SetupGamepadPoints(SpriteBatch spriteBatch)
    {
      UILinkPointNavigator.Shortcuts.BackButtonCommand = 3;
      int ID = 3000;
      UILinkPointNavigator.SetPosition(ID, this._backpanel.GetInnerDimensions().ToRectangle().Center.ToVector2());
      UILinkPointNavigator.SetPosition(ID + 1, this._outerContainer.GetInnerDimensions().ToRectangle().Center.ToVector2());
      int key = ID;
      UILinkPoint point1 = UILinkPointNavigator.Points[key];
      point1.Unlink();
      point1.Up = key + 1;
      int num = key + 1;
      UILinkPoint point2 = UILinkPointNavigator.Points[num];
      point2.Unlink();
      point2.Up = num + 1;
      point2.Down = num - 1;
      for (int index = 0; index < this._categoryButtons.Count; ++index)
      {
        ++num;
        UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num;
        UILinkPointNavigator.SetPosition(num, this._categoryButtons[index].GetInnerDimensions().ToRectangle().Center.ToVector2());
        UILinkPoint point3 = UILinkPointNavigator.Points[num];
        point3.Unlink();
        point3.Left = index == 0 ? -3 : num - 1;
        point3.Right = index == this._categoryButtons.Count - 1 ? -4 : num + 1;
        point3.Down = ID;
      }
    }
  }
}
