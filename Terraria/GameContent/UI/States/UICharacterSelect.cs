﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.States.UICharacterSelect
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
  public class UICharacterSelect : UIState
  {
    private UIList _playerList;
    private UITextPanel<LocalizedText> _backPanel;
    private UITextPanel<LocalizedText> _newPanel;
    private UIPanel _containerPanel;
    private UIScrollbar _scrollbar;
    private bool _isScrollbarAttached;
    private List<Tuple<string, bool>> favoritesCache = new List<Tuple<string, bool>>();
    private bool skipDraw;

    public override void OnInitialize()
    {
      UIElement element1 = new UIElement();
      element1.Width.Set(0.0f, 0.8f);
      element1.MaxWidth.Set(650f, 0.0f);
      element1.Top.Set(220f, 0.0f);
      element1.Height.Set(-220f, 1f);
      element1.HAlign = 0.5f;
      UIPanel element2 = new UIPanel();
      element2.Width.Set(0.0f, 1f);
      element2.Height.Set(-110f, 1f);
      element2.BackgroundColor = new Color(33, 43, 79) * 0.8f;
      this._containerPanel = element2;
      element1.Append((UIElement) element2);
      this._playerList = new UIList();
      this._playerList.Width.Set(0.0f, 1f);
      this._playerList.Height.Set(0.0f, 1f);
      this._playerList.ListPadding = 5f;
      element2.Append((UIElement) this._playerList);
      this._scrollbar = new UIScrollbar();
      this._scrollbar.SetView(100f, 1000f);
      this._scrollbar.Height.Set(0.0f, 1f);
      this._scrollbar.HAlign = 1f;
      this._playerList.SetScrollbar(this._scrollbar);
      UITextPanel<LocalizedText> element3 = new UITextPanel<LocalizedText>(Language.GetText("UI.SelectPlayer"), 0.8f, true);
      element3.HAlign = 0.5f;
      element3.Top.Set(-40f, 0.0f);
      element3.SetPadding(15f);
      element3.BackgroundColor = new Color(73, 94, 171);
      element1.Append((UIElement) element3);
      UITextPanel<LocalizedText> element4 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
      element4.Width.Set(-10f, 0.5f);
      element4.Height.Set(50f, 0.0f);
      element4.VAlign = 1f;
      element4.Top.Set(-45f, 0.0f);
      element4.OnMouseOver += new UIElement.MouseEvent(this.FadedMouseOver);
      element4.OnMouseOut += new UIElement.MouseEvent(this.FadedMouseOut);
      element4.OnClick += new UIElement.MouseEvent(this.GoBackClick);
      element4.SetSnapPoint("Back", 0);
      element1.Append((UIElement) element4);
      this._backPanel = element4;
      UITextPanel<LocalizedText> element5 = new UITextPanel<LocalizedText>(Language.GetText("UI.New"), 0.7f, true);
      element5.CopyStyle((UIElement) element4);
      element5.HAlign = 1f;
      element5.OnMouseOver += new UIElement.MouseEvent(this.FadedMouseOver);
      element5.OnMouseOut += new UIElement.MouseEvent(this.FadedMouseOut);
      element5.OnClick += new UIElement.MouseEvent(this.NewCharacterClick);
      element1.Append((UIElement) element5);
      element4.SetSnapPoint("New", 0);
      this._newPanel = element5;
      this.Append(element1);
    }

    public override void Recalculate()
    {
      if (this._scrollbar != null)
      {
        if (this._isScrollbarAttached && !this._scrollbar.CanScroll)
        {
          this._containerPanel.RemoveChild((UIElement) this._scrollbar);
          this._isScrollbarAttached = false;
          this._playerList.Width.Set(0.0f, 1f);
        }
        else if (!this._isScrollbarAttached && this._scrollbar.CanScroll)
        {
          this._containerPanel.Append((UIElement) this._scrollbar);
          this._isScrollbarAttached = true;
          this._playerList.Width.Set(-25f, 1f);
        }
      }
      base.Recalculate();
    }

    private void NewCharacterClick(UIMouseEvent evt, UIElement listeningElement)
    {
      SoundEngine.PlaySound(10);
      Main.PendingPlayer = new Player();
      Main.menuMode = 888;
      Main.MenuUI.SetState((UIState) new UICharacterCreation(Main.PendingPlayer));
    }

    private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
    {
      SoundEngine.PlaySound(11);
      Main.menuMode = 0;
    }

    private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
    {
      SoundEngine.PlaySound(12);
      ((UIPanel) evt.Target).BackgroundColor = new Color(73, 94, 171);
      ((UIPanel) evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
    }

    private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
    {
      ((UIPanel) evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
      ((UIPanel) evt.Target).BorderColor = Color.Black;
    }

    public override void OnActivate()
    {
      Main.LoadPlayers();
      Main.ActivePlayerFileData = new PlayerFileData();
      this.UpdatePlayersList();
      if (!PlayerInput.UsingGamepadUI)
        return;
      UILinkPointNavigator.ChangePoint(3000 + (this._playerList.Count == 0 ? 1 : 2));
    }

    private void UpdatePlayersList()
    {
      this._playerList.Clear();
      List<PlayerFileData> playerFileDataList = new List<PlayerFileData>((IEnumerable<PlayerFileData>) Main.PlayerList);
      playerFileDataList.Sort((Comparison<PlayerFileData>) ((x, y) =>
      {
        if (x.IsFavorite && !y.IsFavorite)
          return -1;
        if (!x.IsFavorite && y.IsFavorite)
          return 1;
        return x.Name.CompareTo(y.Name) != 0 ? x.Name.CompareTo(y.Name) : x.GetFileName().CompareTo(y.GetFileName());
      }));
      int num = 0;
      foreach (PlayerFileData data in playerFileDataList)
        this._playerList.Add((UIElement) new UICharacterListItem(data, num++));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (this.skipDraw)
      {
        this.skipDraw = false;
      }
      else
      {
        if (this.UpdateFavoritesCache())
        {
          this.skipDraw = true;
          Main.MenuUI.Draw(spriteBatch, new GameTime());
        }
        base.Draw(spriteBatch);
        this.SetupGamepadPoints(spriteBatch);
      }
    }

    private bool UpdateFavoritesCache()
    {
      List<PlayerFileData> playerFileDataList = new List<PlayerFileData>((IEnumerable<PlayerFileData>) Main.PlayerList);
      playerFileDataList.Sort((Comparison<PlayerFileData>) ((x, y) =>
      {
        if (x.IsFavorite && !y.IsFavorite)
          return -1;
        if (!x.IsFavorite && y.IsFavorite)
          return 1;
        return x.Name.CompareTo(y.Name) != 0 ? x.Name.CompareTo(y.Name) : x.GetFileName().CompareTo(y.GetFileName());
      }));
      bool flag = false;
      if (!flag && playerFileDataList.Count != this.favoritesCache.Count)
        flag = true;
      if (!flag)
      {
        for (int index = 0; index < this.favoritesCache.Count; ++index)
        {
          Tuple<string, bool> tuple = this.favoritesCache[index];
          if (!(playerFileDataList[index].Name == tuple.Item1) || playerFileDataList[index].IsFavorite != tuple.Item2)
          {
            flag = true;
            break;
          }
        }
      }
      if (flag)
      {
        this.favoritesCache.Clear();
        foreach (PlayerFileData playerFileData in playerFileDataList)
          this.favoritesCache.Add(Tuple.Create<string, bool>(playerFileData.Name, playerFileData.IsFavorite));
        this.UpdatePlayersList();
      }
      return flag;
    }

    private void SetupGamepadPoints(SpriteBatch spriteBatch)
    {
      UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
      int num1 = 3000;
      UILinkPointNavigator.SetPosition(num1, this._backPanel.GetInnerDimensions().ToRectangle().Center.ToVector2());
      UILinkPointNavigator.SetPosition(num1 + 1, this._newPanel.GetInnerDimensions().ToRectangle().Center.ToVector2());
      int key1 = num1;
      UILinkPoint point1 = UILinkPointNavigator.Points[key1];
      point1.Unlink();
      point1.Right = key1 + 1;
      int key2 = num1 + 1;
      UILinkPoint point2 = UILinkPointNavigator.Points[key2];
      point2.Unlink();
      point2.Left = key2 - 1;
      float num2 = 1f / Main.UIScale;
      Rectangle clippingRectangle = this._containerPanel.GetClippingRectangle(spriteBatch);
      Vector2 minimum = clippingRectangle.TopLeft() * num2;
      Vector2 maximum = clippingRectangle.BottomRight() * num2;
      List<SnapPoint> snapPoints = this.GetSnapPoints();
      for (int index = 0; index < snapPoints.Count; ++index)
      {
        if (!snapPoints[index].Position.Between(minimum, maximum))
        {
          snapPoints.Remove(snapPoints[index]);
          --index;
        }
      }
      int length = 5;
      SnapPoint[,] snapPointArray = new SnapPoint[this._playerList.Count, length];
      foreach (SnapPoint snapPoint in snapPoints.Where<SnapPoint>((Func<SnapPoint, bool>) (a => a.Name == "Play")))
        snapPointArray[snapPoint.Id, 0] = snapPoint;
      foreach (SnapPoint snapPoint in snapPoints.Where<SnapPoint>((Func<SnapPoint, bool>) (a => a.Name == "Favorite")))
        snapPointArray[snapPoint.Id, 1] = snapPoint;
      foreach (SnapPoint snapPoint in snapPoints.Where<SnapPoint>((Func<SnapPoint, bool>) (a => a.Name == "Cloud")))
        snapPointArray[snapPoint.Id, 2] = snapPoint;
      foreach (SnapPoint snapPoint in snapPoints.Where<SnapPoint>((Func<SnapPoint, bool>) (a => a.Name == "Rename")))
        snapPointArray[snapPoint.Id, 3] = snapPoint;
      foreach (SnapPoint snapPoint in snapPoints.Where<SnapPoint>((Func<SnapPoint, bool>) (a => a.Name == "Delete")))
        snapPointArray[snapPoint.Id, 4] = snapPoint;
      int num3 = num1 + 2;
      int[] numArray = new int[this._playerList.Count];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = -1;
      for (int index1 = 0; index1 < length; ++index1)
      {
        int key3 = -1;
        for (int index2 = 0; index2 < snapPointArray.GetLength(0); ++index2)
        {
          if (snapPointArray[index2, index1] != null)
          {
            UILinkPoint point3 = UILinkPointNavigator.Points[num3];
            point3.Unlink();
            UILinkPointNavigator.SetPosition(num3, snapPointArray[index2, index1].Position);
            if (key3 != -1)
            {
              point3.Up = key3;
              UILinkPointNavigator.Points[key3].Down = num3;
            }
            if (numArray[index2] != -1)
            {
              point3.Left = numArray[index2];
              UILinkPointNavigator.Points[numArray[index2]].Right = num3;
            }
            point3.Down = num1;
            if (index1 == 0)
              UILinkPointNavigator.Points[num1].Up = UILinkPointNavigator.Points[num1 + 1].Up = num3;
            key3 = num3;
            numArray[index2] = num3;
            UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num3;
            ++num3;
          }
        }
      }
      if (!PlayerInput.UsingGamepadUI || this._playerList.Count != 0 || UILinkPointNavigator.CurrentPoint <= 3001)
        return;
      UILinkPointNavigator.ChangePoint(3001);
    }
  }
}
